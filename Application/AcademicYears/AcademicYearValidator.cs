using System.Text.RegularExpressions;

namespace Application.AcademicYears;

public sealed record CreateAcademicYearRequest(
    string ProvinceCode,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record AcademicYearValidationResult(
    IReadOnlyDictionary<string, string[]> Errors)
{
    public bool IsValid => Errors.Count == 0;
}

public static partial class AcademicYearValidator
{
    public static AcademicYearValidationResult Validate(CreateAcademicYearRequest request)
    {
        var errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        if (!ProvinceCodePattern().IsMatch(request.ProvinceCode.Trim()))
        {
            AddError(errors, "provinceCode", "Mã tỉnh phải gồm đúng 2 chữ số.");
        }

        var name = request.Name.Trim();
        var nameMatch = AcademicYearNamePattern().Match(name);
        if (name.Length == 0)
        {
            AddError(errors, "name", "Tên năm học là bắt buộc.");
        }
        else if (!nameMatch.Success)
        {
            AddError(errors, "name", "Tên năm học phải có định dạng YYYY-YYYY.");
        }
        else
        {
            var startYear = int.Parse(nameMatch.Groups[1].Value);
            var endYear = int.Parse(nameMatch.Groups[2].Value);

            if (endYear != startYear + 1)
            {
                AddError(errors, "name", "Năm kết thúc phải ngay sau năm bắt đầu.");
            }

            if (request.StartDate.Year != startYear)
            {
                AddError(errors, "startDate", $"Ngày bắt đầu phải thuộc năm {startYear}.");
            }

            if (request.EndDate.Year != endYear)
            {
                AddError(errors, "endDate", $"Ngày kết thúc phải thuộc năm {endYear}.");
            }
        }

        if (request.EndDate <= request.StartDate)
        {
            AddError(errors, "endDate", "Ngày kết thúc phải sau ngày bắt đầu.");
        }

        return new AcademicYearValidationResult(
            errors.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.ToArray(),
                StringComparer.OrdinalIgnoreCase));
    }

    private static void AddError(
        IDictionary<string, List<string>> errors,
        string field,
        string message)
    {
        if (!errors.TryGetValue(field, out var fieldErrors))
        {
            fieldErrors = [];
            errors[field] = fieldErrors;
        }

        fieldErrors.Add(message);
    }

    [GeneratedRegex(@"^\d{2}$", RegexOptions.CultureInvariant)]
    private static partial Regex ProvinceCodePattern();

    [GeneratedRegex(@"^(\d{4})-(\d{4})$", RegexOptions.CultureInvariant)]
    private static partial Regex AcademicYearNamePattern();
}
