using FluentValidation;
using HRIS.Domain;

namespace HRIS.Application;

public sealed class EmployeeService(
    IEmployeeRepository repo,
    IValidator<EmployeeRequest> ev,
    IValidator<ResignRequest> rv
) : IEmployeeService
{
    static string K(string t, string c, string s, string n) => $"{t}|{c}|{s}|{n}";

    public async Task<IReadOnlyList<ItemResult>> UpsertAsync(
        IReadOnlyList<EmployeeRequest> a,
        string actor,
        CancellationToken ct
    ) => await Process(a, actor, true, ct);

    public async Task<IReadOnlyList<ItemResult>> UpdateAsync(
        IReadOnlyList<EmployeeRequest> a,
        string actor,
        CancellationToken ct
    ) => await Process(a, actor, false, ct);

    async Task<IReadOnlyList<ItemResult>> Process(
        IReadOnlyList<EmployeeRequest> a,
        string actor,
        bool insert,
        CancellationToken ct
    )
    {
        var o = new List<ItemResult>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var x in a)
        {
            var v = await ev.ValidateAsync(x, ct);
            if (!v.IsValid)
            {
                o.Add(
                    new(
                        x.EmployeeNo,
                        false,
                        string.Join("; ", v.Errors.Select(e => e.ErrorMessage))
                    )
                );
                continue;
            }
            if (!seen.Add(K(x.TenantId, x.CompanyId, x.SiteId, x.EmployeeNo)))
            {
                o.Add(new(x.EmployeeNo, false, "Duplicate employee in request."));
                continue;
            }
            var e = await repo.FindAsync(x.TenantId, x.CompanyId, x.SiteId, x.EmployeeNo, ct);
            if (e is null && !insert)
            {
                o.Add(new(x.EmployeeNo, false, "Employee not found."));
                continue;
            }
            if (e is not null && !e.IsActive)
            {
                o.Add(new(x.EmployeeNo, false, "Inactive employee cannot be updated."));
                continue;
            }
            if (e is null)
            {
                e = New(x, actor);
                await repo.AddAsync(e, ct);
            }
            else
                Apply(e, x, actor);
            await repo.SaveAsync(ct);
            o.Add(
                new(
                    x.EmployeeNo,
                    true,
                    insert ? "Inserted/updated successfully." : "Updated successfully."
                )
            );
        }
        return o;
    }

    public async Task<IReadOnlyList<ItemResult>> ResignAsync(
        IReadOnlyList<ResignRequest> a,
        string actor,
        CancellationToken ct
    )
    {
        var o = new List<ItemResult>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var x in a)
        {
            var v = await rv.ValidateAsync(x, ct);
            if (!v.IsValid)
            {
                o.Add(
                    new(
                        x.EmployeeNo,
                        false,
                        string.Join("; ", v.Errors.Select(e => e.ErrorMessage))
                    )
                );
                continue;
            }
            if (!seen.Add(K(x.TenantId, x.CompanyId, x.SiteId, x.EmployeeNo)))
            {
                o.Add(new(x.EmployeeNo, false, "Duplicate employee in request."));
                continue;
            }
            var e = await repo.FindAsync(x.TenantId, x.CompanyId, x.SiteId, x.EmployeeNo, ct);
            if (e is null)
            {
                o.Add(
                    new(
                        x.EmployeeNo,
                        false,
                        "Employee not found - resignation cannot be processed."
                    )
                );
                continue;
            }
            e.IsActive = false;
            e.ResignDate = x.ResignDate;
            e.ModifiedBy = actor;
            e.ModifiedDate = DateTime.UtcNow;
            await repo.SaveAsync(ct);
            o.Add(new(x.EmployeeNo, true, "Resigned successfully."));
        }
        return o;
    }

    static Employee New(EmployeeRequest x, string actor)
    {
        var e = new Employee
        {
            TenantId = x.TenantId,
            CompanyId = x.CompanyId,
            SiteId = x.SiteId,
            EmployeeNo = x.EmployeeNo,
            EmpName = x.EmpName,
            NFCUId = x.NFCUId,
            EmpCategoryId = x.EmpCategoryId,
            EmpCategoryName = x.EmpCategoryName,
            EmpTypeId = x.EmpTypeId,
            EmpTypeName = x.EmpTypeName,
            EmpJoinedDate = x.EmpJoinedDate,
            CreatedBy = actor,
        };
        Apply(e, x, actor);
        return e;
    }

    static void Apply(Employee e, EmployeeRequest x, string actor)
    {
        e.EmpName = x.EmpName;
        e.EmpNameSecondLanguage = x.EmpNameSecondLanguage;
        e.Alias = x.Alias;
        e.AliasSecondLanguage = x.AliasSecondLanguage;
        e.Email = x.Email;
        e.MobileNo = x.MobileNo;
        e.NFCUId = x.NFCUId;
        e.EmpCategoryId = x.EmpCategoryId;
        e.EmpCategoryName = x.EmpCategoryName;
        e.EmpTypeId = x.EmpTypeId;
        e.EmpTypeName = x.EmpTypeName;
        e.EmpJoinedDate = x.EmpJoinedDate;
        e.UserId = x.UserId;
        e.Image = x.Image;
        e.PreviousEmployeeNo = x.PreviousEmployeeNo;
        e.BandId = x.BandId;
        e.ModifiedBy = actor;
        e.ModifiedDate = DateTime.UtcNow;
    }
}
