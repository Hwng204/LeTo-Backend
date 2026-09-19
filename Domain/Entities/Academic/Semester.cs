namespace Domain.Entities.Academic;

public sealed class Semester
{
    public ulong Id { get; set; }
    public ulong AcademicYearId { get; set; }
    public byte Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = "PLANNED";
    public uint Version { get; set; } = 1;

    public AcademicYear AcademicYear { get; set; } = null!;

    public void EnsureCanConfigure()
    {
        if (Status == "CLOSED")
        {
            throw new AcademicCalendarDomainException(
                "TERM_CLOSED",
                $"Học kỳ {Order} đã đóng không thể sửa.");
        }
    }

    public void Configure(string name, DateOnly? startDate, DateOnly? endDate)
    {
        EnsureCanConfigure();
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Version++;
    }

    public void Close()
    {
        if (Status == "CLOSED")
        {
            throw new AcademicCalendarDomainException(
                "TERM_ALREADY_CLOSED",
                "Học kỳ đã đóng từ trước.");
        }

        Status = "CLOSED";
        Version++;
    }

    internal void CloseWithAcademicYear()
    {
        if (Status == "CLOSED")
        {
            return;
        }

        Status = "CLOSED";
        Version++;
    }
}
