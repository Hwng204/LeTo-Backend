using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public static class StudentMappingExtensions
{
    public static StudentDto ToDto(this Student student) => new()
    {
        Id = student.Id,
        StudentCode = student.StudentCode,
        FullName = student.FullName,
        ClassName = student.ClassName,
        Gender = student.Gender,
        DateOfBirth = student.DateOfBirth,
        IsActive = student.IsActive
    };
}
