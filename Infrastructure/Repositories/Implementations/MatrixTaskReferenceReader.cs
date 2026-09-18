using Application.Common;
using Application.Common.Security;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.QuestionBank;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implement;

public sealed class MatrixTaskReferenceReader(
    ApplicationDbContext db,
    IMatrixRoleCatalog roleCatalog) : IMatrixTaskReferenceReader
{
    public async Task EnsureAssignmentValidAsync(
        MatrixActor actor,
        ulong assignedToUserId,
        ulong academicContextId,
        ulong? semesterId,
        CancellationToken cancellationToken)
    {
        var actorBranchId = await db.Users
            .AsNoTracking()
            .Where(user => user.Id == actor.UserId && user.Status == "ACTIVE")
            .Select(user => user.SchoolBranchId)
            .SingleOrDefaultAsync(cancellationToken);

        var context = await db.AcademicContexts
            .AsNoTracking()
            .Where(item => item.Id == academicContextId)
            .Select(item => new { item.AcademicYearId, item.SchoolBranchId })
            .SingleOrDefaultAsync(cancellationToken);

        if (context is null)
        {
            throw new MatrixApplicationException(
                "InvalidReference",
                "Không tìm thấy ngữ cảnh học thuật.");
        }

        if (!actor.IsPrincipal &&
            (actorBranchId is null || actorBranchId.Value != context.SchoolBranchId))
        {
            throw new MatrixApplicationException(
                "Forbidden",
                "Ngữ cảnh học thuật nằm ngoài chi nhánh của bạn.");
        }

        await EnsureSemesterAsync(
            context.AcademicYearId,
            semesterId,
            cancellationToken);

        var assignee = await db.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
            .SingleOrDefaultAsync(
                user => user.Id == assignedToUserId && user.Status == "ACTIVE",
                cancellationToken);

        if (assignee is null ||
            assignee.SchoolBranchId != context.SchoolBranchId ||
            !assignee.UserRoles.Any(userRole =>
                roleCatalog.IsTeamLead(userRole.Role.Code)))
        {
            throw new MatrixApplicationException(
                "InvalidAssignee",
                "Người nhận phải là Tổ trưởng đang hoạt động thuộc cùng chi nhánh.");
        }
    }

    public async Task<MatrixReferenceData> GetAsync(
        MatrixActor actor,
        ulong? academicContextId,
        CancellationToken cancellationToken)
    {
        var actorBranchId = await db.Users
            .AsNoTracking()
            .Where(user => user.Id == actor.UserId && user.Status == "ACTIVE")
            .Select(user => user.SchoolBranchId)
            .SingleOrDefaultAsync(cancellationToken);

        if (actor.Role == MatrixActorRole.TeamLead && actorBranchId is null)
        {
            throw new MatrixApplicationException(
                "Forbidden",
                "Tài khoản Tổ trưởng chưa được gán chi nhánh.");
        }

        if (actor.IsPrincipal)
        {
            actorBranchId = null;
        }
        else if (actor.Role == MatrixActorRole.Pht && actorBranchId is null)
        {
            throw new MatrixApplicationException(
                "Forbidden",
                "Tài khoản PHT chưa được gán chi nhánh.");
        }

        var contexts = db.AcademicContexts
            .AsNoTracking()
            .Where(context =>
                context.AcademicYear.Status == "ACTIVE" &&
                context.School.Status == "ACTIVE" &&
                context.SchoolBranch.Status == "ACTIVE");

        if (actorBranchId is not null)
        {
            contexts = contexts.Where(context => context.SchoolBranchId == actorBranchId.Value);
        }

        if (academicContextId is not null)
        {
            contexts = contexts.Where(context => context.Id == academicContextId.Value);
        }

        var contextRows = await contexts
            .Select(context => new
            {
                context.Id,
                context.AcademicYearId,
                context.SchoolBranchId,
                context.TextbookId,
                context.SubjectId,
                context.GradeLevelId,
                AcademicYearName = context.AcademicYear.Name,
                SchoolName = context.School.Name,
                BranchName = context.SchoolBranch.Name,
                SubjectName = context.Subject.Name,
                GradeLevelName = context.GradeLevel.Name
            })
            .OrderBy(context => context.Id)
            .ToListAsync(cancellationToken);

        var contextOptions = contextRows
            .Select(context => new MatrixAcademicContextOption(
                context.Id,
                $"{context.SubjectName} - {context.GradeLevelName} - {context.AcademicYearName} - {context.SchoolName} / {context.BranchName}",
                context.AcademicYearId,
                context.SchoolBranchId,
                context.TextbookId,
                context.SubjectId,
                context.GradeLevelId))
            .ToArray();

        var academicYearIds = contextRows
            .Select(context => context.AcademicYearId)
            .Distinct()
            .ToArray();
        var semesters = await db.Semesters
            .AsNoTracking()
            .Where(semester =>
                academicYearIds.Contains(semester.AcademicYearId) &&
                semester.AcademicYear.Status == "ACTIVE")
            .OrderBy(semester => semester.StartDate)
            .Select(semester => new MatrixSemesterOption(
                semester.Id,
                semester.AcademicYearId,
                semester.Name,
                semester.StartDate,
                semester.EndDate))
            .ToListAsync(cancellationToken);

        var textbookIds = academicContextId is null
            ? Array.Empty<ulong>()
            : contextRows.Select(context => context.TextbookId).Distinct().ToArray();
        var lessonRows = await db.TextbookLessons
            .AsNoTracking()
            .Where(lesson => textbookIds.Contains(lesson.Chapter.TextbookId))
            .OrderBy(lesson => lesson.Chapter.SortOrder)
            .ThenBy(lesson => lesson.SortOrder)
            .Select(lesson => new
            {
                lesson.Id,
                TextbookId = lesson.Chapter.TextbookId,
                lesson.ChapterId,
                lesson.Title,
                lesson.SortOrder
            })
            .ToListAsync(cancellationToken);
        var lessons = lessonRows
            .Select(lesson => new MatrixLessonOption(
                lesson.Id,
                contextRows.First(context => context.TextbookId == lesson.TextbookId).Id,
                lesson.ChapterId,
                lesson.Title,
                lesson.SortOrder))
            .ToArray();

        var teamLeads = actor.Role == MatrixActorRole.Pht
            ? await db.Users
                .AsNoTracking()
                .Where(user =>
                    user.Status == "ACTIVE" &&
                    (actorBranchId == null || user.SchoolBranchId == actorBranchId.Value) &&
                    user.UserRoles.Any(userRole =>
                        roleCatalog.TeamLeadRoleCodes.Contains(userRole.Role.Code)))
                .OrderBy(user => user.FullName)
                .Select(user => new MatrixTeamLeadOption(
                    user.Id,
                    user.Username,
                    user.FullName,
                    user.SchoolBranchId))
                .ToListAsync(cancellationToken)
            : [];

        var cognitiveLevels = MatrixCognitiveLevels.All
            .Select(level => new MatrixCognitiveLevelOption(level.Code, level.Label))
            .ToArray();

        return new MatrixReferenceData(contextOptions, semesters, lessons, teamLeads, cognitiveLevels);
    }

    private async Task EnsureSemesterAsync(
        ulong academicYearId,
        ulong? semesterId,
        CancellationToken cancellationToken)
    {
        if (semesterId is null)
        {
            return;
        }

        var semesterYearId = await db.Semesters
            .AsNoTracking()
            .Where(semester => semester.Id == semesterId.Value)
            .Select(semester => (ulong?)semester.AcademicYearId)
            .SingleOrDefaultAsync(cancellationToken);

        if (semesterYearId is null || semesterYearId.Value != academicYearId)
        {
            throw new MatrixApplicationException(
                "InvalidReference",
                "Học kỳ không thuộc năm học của ngữ cảnh học thuật.");
        }
    }
}
