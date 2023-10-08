using Microsoft.EntityFrameworkCore;
using NghiaSoft.FileServerAPI.Data.Entities;

namespace NghiaSoft.FileServerAPI.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<AppFileInfo> AppFileInfos { get; set; }
    public DbSet<AppFileActionDetail> AppFileActionDetails { get; set; }

    // dotnet ef migrations add "Init"
    // dotnet ef migrations remove
    // dotnet ef database update
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}