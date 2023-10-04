using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NghiaSoft.AuthServer.Data;

public class ApplicationDbContext : IdentityDbContext
{
    // dotnet ef migrations add "Init"
    // dotnet ef migrations remove
    // dotnet ef database update
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}