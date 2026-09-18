using Domain.Entities.Academic;

namespace Domain.Entities.QuestionBank;

public sealed class ExamMatrix
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ulong? TaskId { get; set; }
    public ulong? SemesterId { get; set; }
    public ulong AcademicContextId { get; set; }

    public WorkTask? Task { get; set; }
    public Semester? Semester { get; set; }
    public AcademicContext AcademicContext { get; set; } = null!;
    public ICollection<MatrixDetail> Details { get; set; } = new List<MatrixDetail>();

    public uint TotalQuestions =>
        Details.Aggregate(0u, (total, detail) => checked(total + detail.QuestionCount));

    public decimal TotalScore => Details.Sum(detail => detail.AllocatedScore);

    public bool CanEdit(MatrixActor actor)
    {
        return Status switch
        {
            MatrixStatusCodes.Draft => actor.Role == MatrixActorRole.Pht || IsAssignedTeamLead(actor),
            MatrixStatusCodes.Submitted => actor.Role == MatrixActorRole.Pht,
            _ => false
        };
    }

    public bool CanHardDelete(MatrixActor actor)
    {
        return Status == MatrixStatusCodes.Draft &&
            (actor.Role == MatrixActorRole.Pht || IsAssignedTeamLead(actor));
    }

    public void ReplaceDetails(IEnumerable<MatrixDetailValue> values, MatrixActor actor)
    {
        if (!CanEdit(actor))
        {
            throw new MatrixDomainException(
                "MatrixNotEditable",
                "Bạn không thể chỉnh sửa ma trận ở trạng thái hiện tại.");
        }

        var normalizedDetails = new List<MatrixDetail>();
        var keys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var value in values ?? throw new ArgumentNullException(nameof(values)))
        {
            if (value.LessonId == 0 ||
                value.QuestionCount == 0 ||
                value.AllocatedScore <= 0)
            {
                throw new MatrixDomainException(
                    "InvalidDetail",
                    "Mỗi dòng chi tiết phải có bài học, số câu và điểm lớn hơn 0.");
            }

            var cognitiveLevel = Normalize(value.CognitiveLevel);
            var questionType = Normalize(value.QuestionType);
            if (questionType.Length == 0)
            {
                questionType = MatrixQuestionTypes.MultipleChoice;
            }

            if (!MatrixCognitiveLevels.IsKnown(cognitiveLevel) ||
                questionType != MatrixQuestionTypes.MultipleChoice)
            {
                throw new MatrixDomainException(
                    "InvalidDetail",
                    "Mỗi dòng chi tiết phải dùng mức nhận thức hợp lệ (Nhận biết, Thông hiểu, Vận dụng) và loại câu hỏi trắc nghiệm.");
            }

            var key = $"{value.LessonId}:{cognitiveLevel}:{questionType}";
            if (!keys.Add(key))
            {
                throw new MatrixDomainException(
                    "DuplicateDetail",
                    "Ma trận không được có hai dòng trùng bài học, mức nhận thức và loại câu hỏi.");
            }

            normalizedDetails.Add(new MatrixDetail
            {
                ExamMatrixId = Id,
                LessonId = value.LessonId,
                CognitiveLevel = cognitiveLevel,
                QuestionType = questionType,
                QuestionCount = value.QuestionCount,
                AllocatedScore = value.AllocatedScore,
                ExamMatrix = this
            });
        }

        Details.Clear();
        foreach (var detail in normalizedDetails)
        {
            Details.Add(detail);
        }
    }

    public void Submit(MatrixActor actor)
    {
        EnsureDraft();
        if (actor.Role != MatrixActorRole.Pht && !IsAssignedTeamLead(actor))
        {
            throw new MatrixDomainException(
                "Forbidden",
                "Chỉ PHT hoặc Tổ trưởng được giao mới được nộp ma trận này.");
        }

        if (Details.Count == 0)
        {
            throw new MatrixDomainException(
                "EmptyMatrix",
                "Ma trận phải có ít nhất một dòng chi tiết trước khi nộp hoặc xác nhận.");
        }

        Status = MatrixStatusCodes.Submitted;
    }

    public void Withdraw(MatrixActor actor)
    {
        if (Status != MatrixStatusCodes.Submitted)
        {
            throw InvalidTransition("Chỉ thu hồi được ma trận đã nộp.");
        }

        if (!IsAssignedTeamLead(actor))
        {
            throw new MatrixDomainException(
                "Forbidden",
                "Chỉ Tổ trưởng được giao mới được thu hồi ma trận này.");
        }

        Status = MatrixStatusCodes.Draft;
    }

    public void Approve(MatrixActor actor)
    {
        if (actor.Role != MatrixActorRole.Pht)
        {
            throw new MatrixDomainException(
                "Forbidden",
                "Chỉ PHT mới được duyệt ma trận.");
        }

        if (Status != MatrixStatusCodes.Submitted)
        {
            throw InvalidTransition("Chỉ duyệt được ma trận đã nộp.");
        }

        Status = MatrixStatusCodes.Approved;
    }

    public void ConfirmDirect(MatrixActor actor)
    {
        if (actor.Role != MatrixActorRole.Pht)
        {
            throw new MatrixDomainException(
                "Forbidden",
                "Chỉ PHT mới được xác nhận ma trận trực tiếp.");
        }

        if (TaskId is not null)
        {
            throw new MatrixDomainException(
                "DirectMatrixRequired",
                "Chỉ xác nhận trực tiếp được ma trận không thuộc nhiệm vụ nào.");
        }

        Submit(actor);
        Approve(actor);
    }

    public void Archive(MatrixActor actor)
    {
        if (actor.Role != MatrixActorRole.Pht)
        {
            throw new MatrixDomainException(
                "Forbidden",
                "Chỉ PHT mới được lưu trữ ma trận.");
        }

        if (Status != MatrixStatusCodes.Approved)
        {
            throw InvalidTransition("Chỉ lưu trữ được ma trận đã duyệt.");
        }

        Status = MatrixStatusCodes.Archived;
    }

    public ExamMatrix CloneAsDraft(MatrixActor actor)
    {
        if (actor.Role != MatrixActorRole.Pht)
        {
            throw new MatrixDomainException(
                "Forbidden",
                "Chỉ PHT mới được tạo bản sao ma trận.");
        }

        if (Status is not (MatrixStatusCodes.Approved or MatrixStatusCodes.Archived))
        {
            throw InvalidTransition("Chỉ sao chép được ma trận đã duyệt hoặc đã lưu trữ.");
        }

        var clone = new ExamMatrix
        {
            Id = 0,
            Name = Name,
            Status = MatrixStatusCodes.Draft,
            TaskId = null,
            SemesterId = SemesterId,
            AcademicContextId = AcademicContextId
        };

        foreach (var detail in Details)
        {
            clone.Details.Add(new MatrixDetail
            {
                Id = 0,
                ExamMatrixId = 0,
                LessonId = detail.LessonId,
                CognitiveLevel = detail.CognitiveLevel,
                QuestionType = detail.QuestionType,
                QuestionCount = detail.QuestionCount,
                AllocatedScore = detail.AllocatedScore,
                ExamMatrix = clone,
                Lesson = detail.Lesson
            });
        }

        return clone;
    }

    private bool IsAssignedTeamLead(MatrixActor actor)
    {
        return actor.Role == MatrixActorRole.TeamLead &&
            TaskId.HasValue &&
            Task is not null &&
            Task.AssignedToUserId == actor.UserId;
    }

    private void EnsureDraft()
    {
        if (Status != MatrixStatusCodes.Draft)
        {
            throw InvalidTransition("Chỉ ma trận ở trạng thái Nháp mới thực hiện được thao tác này.");
        }
    }

    private static string Normalize(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }

    private static MatrixDomainException InvalidTransition(string message)
    {
        return new MatrixDomainException("InvalidTransition", message);
    }
}
