using Domain.Entities.Academic;
using Domain.Entities.QuestionBank;

namespace Domain.Entities.Examination;

public sealed class ExamSubjectGradeLevel
{
    public ulong Id { get; set; }
    public ulong ExamSubjectId { get; set; }
    public ulong GradeLevelId { get; set; }
    public ulong PrimaryExamSetId { get; set; }
    public ulong? BackupExamSetId { get; set; }
    public string Status { get; set; } = string.Empty;

    public ExamSubject ExamSubject { get; set; } = null!;
    public GradeLevel GradeLevel { get; set; } = null!;
    public ExamSet PrimaryExamSet { get; set; } = null!;
    public ExamSet? BackupExamSet { get; set; }
    public ICollection<ExamSession> Sessions { get; set; } = new List<ExamSession>();
    public ICollection<ExamRegistration> Registrations { get; set; } = new List<ExamRegistration>();
}
