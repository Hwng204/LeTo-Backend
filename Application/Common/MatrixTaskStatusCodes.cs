namespace Application.Common;

public static class MatrixTaskStatusCodes
{
    public const string Assigned = "ASSIGNED";
    public const string Submitted = "SUBMITTED";
    public const string Completed = "COMPLETED";

    public static string Label(string? status) => status switch
    {
        Assigned => "Đã giao",
        Submitted => "Đã nộp",
        Completed => "Hoàn thành",
        _ => status ?? string.Empty
    };
}
