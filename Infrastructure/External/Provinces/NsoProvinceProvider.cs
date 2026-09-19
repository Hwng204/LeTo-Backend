using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Infrastructure.External.Provinces;

public sealed partial class NsoProvinceProvider(
    HttpClient httpClient,
    int minimumExpectedCount = 34) : IProvinceProvider
{
    private const string EndpointPath = "DMDVHC.asmx";
    private const int MaximumExpectedCount = 100;
    private const long MaximumResponseCharacters = 1_000_000;
    private static readonly HashSet<string> AcceptedDivisionTypes =
        new(StringComparer.OrdinalIgnoreCase) { "Tỉnh", "Thành phố Trung ương" };

    public string Name => "NSO_DMDVHC";

    public async Task<IReadOnlyList<ProvinceCatalogRecord>> FetchAsync(
        DateOnly asOfDate,
        CancellationToken cancellationToken)
    {
        using var response = await SendWithRetryAsync(asOfDate, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = XmlReader.Create(stream, new XmlReaderSettings
        {
            Async = true,
            DtdProcessing = DtdProcessing.Prohibit,
            MaxCharactersInDocument = MaximumResponseCharacters,
            XmlResolver = null,
        });
        XDocument document;
        try
        {
            document = await XDocument.LoadAsync(reader, LoadOptions.None, cancellationToken);
        }
        catch (XmlException exception)
        {
            throw new InvalidDataException("Province provider returned malformed XML.", exception);
        }

        var provinces = document
            .Descendants()
            .Where(element => element.Name.LocalName == "TABLE")
            .Select(ParseProvince)
            .ToArray();

        Validate(provinces);
        return provinces;
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(
        DateOnly asOfDate,
        CancellationToken cancellationToken)
    {
        const int maximumAttempts = 2;
        for (var attempt = 1; attempt <= maximumAttempts; attempt++)
        {
            try
            {
                using var request = CreateRequest(asOfDate);
                var response = await httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);
                if (attempt < maximumAttempts && (int)response.StatusCode >= 500)
                {
                    response.Dispose();
                    continue;
                }

                return response;
            }
            catch (HttpRequestException) when (attempt < maximumAttempts)
            {
            }
            catch (TaskCanceledException) when (
                attempt < maximumAttempts && !cancellationToken.IsCancellationRequested)
            {
            }
        }

        throw new InvalidOperationException("Province provider retry loop ended unexpectedly.");
    }

    private static HttpRequestMessage CreateRequest(DateOnly asOfDate)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, EndpointPath);
        request.Headers.Add("SOAPAction", "http://tempuri.org/DanhMucTinh");
        request.Content = new StringContent(
            CreateSoapRequest(asOfDate),
            Encoding.UTF8,
            "text/xml");
        return request;
    }

    private static ProvinceCatalogRecord ParseProvince(XElement row)
    {
        string Value(string elementName) => row.Elements()
            .FirstOrDefault(element => element.Name.LocalName == elementName)?
            .Value.Trim() ?? string.Empty;

        return new ProvinceCatalogRecord(
            Value("MaTinh"),
            Value("TenTinh"),
            Value("LoaiHinh"));
    }

    private void Validate(IReadOnlyList<ProvinceCatalogRecord> provinces)
    {
        if (provinces.Count < minimumExpectedCount || provinces.Count > MaximumExpectedCount)
        {
            throw new InvalidDataException(
                $"Province catalog must contain at least {minimumExpectedCount} and at most " +
                $"{MaximumExpectedCount} records; received {provinces.Count}.");
        }

        foreach (var province in provinces)
        {
            if (!ProvinceCodePattern().IsMatch(province.Code))
            {
                throw new InvalidDataException($"Province code '{province.Code}' is invalid.");
            }

            if (string.IsNullOrWhiteSpace(province.Name) || province.Name.Length > 150)
            {
                throw new InvalidDataException($"Province '{province.Code}' has an invalid name.");
            }

            if (!AcceptedDivisionTypes.Contains(province.DivisionType))
            {
                throw new InvalidDataException(
                    $"Province '{province.Code}' has unsupported division type '{province.DivisionType}'.");
            }
        }

        var duplicateCode = provinces
            .GroupBy(province => province.Code, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1)?.Key;
        if (duplicateCode is not null)
        {
            throw new InvalidDataException($"Province catalog contains duplicate code '{duplicateCode}'.");
        }
    }

    private static string CreateSoapRequest(DateOnly asOfDate) => $$"""
        <?xml version="1.0" encoding="utf-8"?>
        <soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                       xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                       xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
          <soap:Body>
            <DanhMucTinh xmlns="http://tempuri.org/">
              <DenNgay>{{asOfDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}}</DenNgay>
            </DanhMucTinh>
          </soap:Body>
        </soap:Envelope>
        """;

    [GeneratedRegex("^[0-9]{2}$", RegexOptions.CultureInvariant)]
    private static partial Regex ProvinceCodePattern();
}
