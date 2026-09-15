using Application.Interfaces;

namespace Infrastructure.Data;

public class ApplicationDbContext : IApplicationDbContext
{
    // Cấu hình DbContext kết nối Database (ví dụ SQL Server)
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return 1;
    }
}
