using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implement;

public sealed class MatrixReferenceReader(ApplicationDbContext db) : IMatrixReferenceReader
{
    public async Task EnsureValidAsync(
        ulong academicContextId,
        ulong? semesterId,
        IReadOnlyCollection<MatrixDetailRequest> details,
        CancellationToken cancellationToken,
        ulong? requiredBranchId = null)
    {
        if (academicContextId == 0)
        {
            throw InvalidReference("Ngữ cảnh học thuật là bắt buộc.");
        }

        var context = await db.AcademicContexts
            .AsNoTracking()
            .Where(item => item.Id == academicContextId)
            .Select(item => new
            {
                item.AcademicYearId,
                item.TextbookId,
                item.SchoolBranchId
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (context is null)
        {
            throw InvalidReference("Không tìm thấy ngữ cảnh học thuật.");
        }

        if (requiredBranchId is not null && context.SchoolBranchId != requiredBranchId)
        {
            throw new MatrixApplicationException(
                "Forbidden",
                "Ngữ cảnh học thuật thuộc chi nhánh khác.");
        }

        if (semesterId is not null)
        {
            if (semesterId == 0)
            {
                throw InvalidReference("Học kỳ không hợp lệ.");
            }

            var semesterYearId = await db.Semesters
                .AsNoTracking()
                .Where(item => item.Id == semesterId.Value)
                .Select(item => (ulong?)item.AcademicYearId)
                .SingleOrDefaultAsync(cancellationToken);

            if (semesterYearId is null)
            {
                throw InvalidReference("Không tìm thấy học kỳ.");
            }

            if (semesterYearId != context.AcademicYearId)
            {
                throw InvalidReference(
                    "Học kỳ không thuộc năm học của ngữ cảnh học thuật.");
            }
        }

        var lessonIds = details
            .Select(detail => detail.LessonId)
            .Distinct()
            .ToArray();

        if (lessonIds.Length == 0)
        {
            return;
        }

        var validLessonCount = await db.TextbookLessons
            .AsNoTracking()
            .Where(lesson =>
                lessonIds.Contains(lesson.Id) &&
                lesson.Chapter.TextbookId == context.TextbookId)
            .CountAsync(cancellationToken);

        if (validLessonCount != lessonIds.Length)
        {
            throw InvalidReference(
                "Mọi bài học trong ma trận phải thuộc sách giáo khoa của ngữ cảnh học thuật.");
        }
    }

    public async Task<MatrixExportInfo> GetExportInfoAsync(
        ulong academicContextId,
        ulong? semesterId,
        IReadOnlyCollection<ulong> lessonIds,
        CancellationToken cancellationToken)
    {
        var context = await db.AcademicContexts
            .AsNoTracking()
            .Where(item => item.Id == academicContextId)
            .Select(item => new
            {
                Subject = item.Subject.Name,
                Grade = item.GradeLevel.Name,
                Year = item.AcademicYear.Name,
                School = item.School.Name,
                Branch = item.SchoolBranch.Name
            })
            .SingleOrDefaultAsync(cancellationToken);

        var semesterName = semesterId is null
            ? null
            : await db.Semesters
                .AsNoTracking()
                .Where(item => item.Id == semesterId.Value)
                .Select(item => item.Name)
                .SingleOrDefaultAsync(cancellationToken);

        var lessons = await db.TextbookLessons
            .AsNoTracking()
            .Where(lesson => lessonIds.Contains(lesson.Id))
            .Select(lesson => new { lesson.Id, lesson.Title, Chapter = lesson.Chapter.Title })
            .ToListAsync(cancellationToken);

        var label = context is null
            ? academicContextId.ToString()
            : $"{context.Subject} - {context.Grade} - {context.Year} - {context.School} / {context.Branch}";

        return new MatrixExportInfo(
            label,
            semesterName,
            lessons.ToDictionary(lesson => lesson.Id, lesson => $"{lesson.Chapter} / {lesson.Title}"));
    }

    private static MatrixApplicationException InvalidReference(string message)
    {
        return new MatrixApplicationException("InvalidReference", message);
    }
}
