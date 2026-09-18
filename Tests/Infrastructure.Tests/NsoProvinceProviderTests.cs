using System.Net;
using System.Text;
using Infrastructure.External.Provinces;
using Xunit;

namespace Infrastructure.Tests;

public sealed class NsoProvinceProviderTests
{
    [Fact]
    public async Task FetchAsync_ParsesOfficialSoapPayloadAndPreservesLeadingZero()
    {
        using var httpClient = CreateClient(SoapResponse(
            Row("01", "Thành phố Hà Nội", "Thành phố Trung ương"),
            Row("04", "Tỉnh Cao Bằng", "Tỉnh")), HttpStatusCode.OK);
        var provider = new NsoProvinceProvider(httpClient, minimumExpectedCount: 2);

        var result = await provider.FetchAsync(new DateOnly(2026, 9, 18), CancellationToken.None);

        Assert.Collection(
            result,
            province =>
            {
                Assert.Equal("01", province.Code);
                Assert.Equal("Thành phố Hà Nội", province.Name);
                Assert.Equal("Thành phố Trung ương", province.DivisionType);
            },
            province => Assert.Equal("04", province.Code));
    }

    [Fact]
    public async Task FetchAsync_RejectsDuplicateProvinceCodes()
    {
        using var httpClient = CreateClient(SoapResponse(
            Row("01", "Thành phố Hà Nội", "Thành phố Trung ương"),
            Row("01", "Tỉnh bị trùng", "Tỉnh")), HttpStatusCode.OK);
        var provider = new NsoProvinceProvider(httpClient, minimumExpectedCount: 2);

        var exception = await Assert.ThrowsAsync<InvalidDataException>(() =>
            provider.FetchAsync(new DateOnly(2026, 9, 18), CancellationToken.None));

        Assert.Contains("duplicate", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FetchAsync_RejectsSuspiciouslyIncompleteCatalog()
    {
        using var httpClient = CreateClient(
            SoapResponse(Row("01", "Thành phố Hà Nội", "Thành phố Trung ương")),
            HttpStatusCode.OK);
        var provider = new NsoProvinceProvider(httpClient, minimumExpectedCount: 2);

        var exception = await Assert.ThrowsAsync<InvalidDataException>(() =>
            provider.FetchAsync(new DateOnly(2026, 9, 18), CancellationToken.None));

        Assert.Contains("at least 2", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FetchAsync_RejectsMalformedXmlAsInvalidProviderData()
    {
        using var httpClient = CreateClient("<not-closed>", HttpStatusCode.OK);
        var provider = new NsoProvinceProvider(httpClient, minimumExpectedCount: 2);

        var exception = await Assert.ThrowsAsync<InvalidDataException>(() =>
            provider.FetchAsync(new DateOnly(2026, 9, 18), CancellationToken.None));

        Assert.Contains("malformed XML", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FetchAsync_RetriesOneTransientServerFailure()
    {
        var handler = new SequenceHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(SoapResponse(
                    Row("01", "Thành phố Hà Nội", "Thành phố Trung ương"),
                    Row("04", "Tỉnh Cao Bằng", "Tỉnh")), Encoding.UTF8, "text/xml"),
            });
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://danhmuchanhchinh.nso.gov.vn/"),
        };
        var provider = new NsoProvinceProvider(httpClient, minimumExpectedCount: 2);

        var result = await provider.FetchAsync(new DateOnly(2026, 9, 18), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, handler.RequestCount);
    }

    private static HttpClient CreateClient(string responseBody, HttpStatusCode statusCode) =>
        new(new StubHttpMessageHandler(responseBody, statusCode))
        {
            BaseAddress = new Uri("https://danhmuchanhchinh.nso.gov.vn/"),
        };

    private static string SoapResponse(params string[] rows) => $$"""
        <?xml version="1.0" encoding="utf-8"?>
        <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
          <soap:Body>
            <DanhMucTinhResponse xmlns="http://tempuri.org/">
              <DanhMucTinhResult>
                <diffgr:diffgram xmlns:diffgr="urn:schemas-microsoft-com:xml-diffgram-v1">
                  <DocumentElement xmlns="">{{string.Concat(rows)}}</DocumentElement>
                </diffgr:diffgram>
              </DanhMucTinhResult>
            </DanhMucTinhResponse>
          </soap:Body>
        </soap:Envelope>
        """;

    private static string Row(string code, string name, string divisionType) => $$"""
        <TABLE><MaTinh>{{code}}</MaTinh><TenTinh>{{name}}</TenTinh><LoaiHinh>{{divisionType}}</LoaiHinh></TABLE>
        """;

    private sealed class StubHttpMessageHandler(string responseBody, HttpStatusCode statusCode)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "text/xml"),
            });
    }

    private sealed class SequenceHttpMessageHandler(params HttpResponseMessage[] responses)
        : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses = new(responses);

        public int RequestCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestCount++;
            return Task.FromResult(_responses.Dequeue());
        }
    }
}
