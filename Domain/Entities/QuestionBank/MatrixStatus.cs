namespace Domain.Entities.QuestionBank;

public static class MatrixStatusCodes
{
    public const string Draft = "DRAFT";
    public const string Submitted = "SUBMITTED";
    public const string Approved = "APPROVED";
    public const string Archived = "ARCHIVED";

    public static string Label(string? status) => status switch
    {
        Draft => "Nháp",
        Submitted => "Đã nộp",
        Approved => "Đã duyệt",
        Archived => "Đã lưu trữ",
        _ => status ?? string.Empty
    };

    public static bool IsKnown(string? status)
    {
        return status is Draft or Submitted or Approved or Archived;
    }
}

public enum MatrixActorRole
{
    Pht,
    TeamLead
}

// BranchId is the PHT's own branch; null for a Principal (IsPrincipal) who manages every branch.
public readonly record struct MatrixActor(
    ulong UserId,
    MatrixActorRole Role,
    ulong? BranchId = null,
    bool IsPrincipal = false);

public static class MatrixCognitiveLevels
{
    public const string Knowledge = "NHAN_BIET";
    public const string Understanding = "THONG_HIEU";
    public const string Application = "VAN_DUNG";

    public static readonly IReadOnlyList<(string Code, string Label)> All =
    [
        (Knowledge, "Nhận biết"),
        (Understanding, "Thông hiểu"),
        (Application, "Vận dụng")
    ];

    public static bool IsKnown(string code) => All.Any(level => level.Code == code);
}

public static class MatrixQuestionTypes
{
    // Every matrix question is multiple choice; the request may omit the type.
    public const string MultipleChoice = "MULTIPLE_CHOICE";
}

