using HRIS.Application;
using HRIS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRIS.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        var e = b.Entity<Employee>();
        e.ToTable("ADMINEmployeeMaster");
        e.HasKey(x => x.Id);
        e.HasIndex(x => new
            {
                x.TenantId,
                x.CompanyId,
                x.SiteId,
                x.EmployeeNo,
            })
            .IsUnique();
        e.Property(x => x.TenantId).HasMaxLength(4).IsFixedLength();
        e.Property(x => x.CompanyId).HasMaxLength(4).IsFixedLength();
        e.Property(x => x.SiteId).HasMaxLength(10);
        e.Property(x => x.EmployeeNo).HasMaxLength(10);
        e.Property(x => x.EmpName).HasMaxLength(100);
        e.Property(x => x.NFCUId).HasMaxLength(30);
        e.Property(x => x.EmpCategoryName).HasMaxLength(20);
        e.Property(x => x.EmpTypeName).HasMaxLength(20);
        e.Property(x => x.Email).HasMaxLength(100);
        e.Property(x => x.Image).HasMaxLength(500);
    }
}

public sealed class EmployeeRepository(AppDbContext db) : IEmployeeRepository
{
    public Task<Employee?> FindAsync(
        string t,
        string c,
        string s,
        string n,
        CancellationToken ct
    ) =>
        db.Employees.SingleOrDefaultAsync(
            x => x.TenantId == t && x.CompanyId == c && x.SiteId == s && x.EmployeeNo == n,
            ct
        );

    public async Task AddAsync(Employee e, CancellationToken ct) =>
        await db.Employees.AddAsync(e, ct);

    public Task<int> SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection s, IConfiguration c)
    {
        s.AddDbContext<AppDbContext>(x =>
            x.UseSqlServer(c.GetConnectionString("DefaultConnection"))
        );
        s.AddScoped<IEmployeeRepository, EmployeeRepository>();
        return s;
    }
}
