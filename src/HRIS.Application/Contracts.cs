using HRIS.Domain;

namespace HRIS.Application;

public interface IEmployeeRepository
{
    Task<Employee?> FindAsync(string t, string c, string s, string no, CancellationToken ct);
    Task AddAsync(Employee e, CancellationToken ct);
    Task<int> SaveAsync(CancellationToken ct);
}

public sealed record EmployeeRequest(
    string TenantId,
    string CompanyId,
    string SiteId,
    string EmployeeNo,
    string EmpName,
    int EmpCategoryId,
    string EmpCategoryName,
    int EmpTypeId,
    string EmpTypeName,
    string NFCUId,
    DateOnly EmpJoinedDate,
    string? BandId = null,
    string? EmpNameSecondLanguage = null,
    string? Alias = null,
    string? AliasSecondLanguage = null,
    string? Email = null,
    string? MobileNo = null,
    string? UserId = null,
    string? Image = null,
    string? PreviousEmployeeNo = null
);

public sealed record ResignRequest(
    string TenantId,
    string CompanyId,
    string SiteId,
    string EmployeeNo,
    DateOnly ResignDate
);

public sealed record ItemResult(string EmployeeNo, bool Success, string Message);

public sealed record ApiMessage(string MessageCode, string Text, string Type, int HttpMessageCode);

public sealed record ApiResponse<T>(IReadOnlyList<ApiMessage> Messages, T Data)
{
    public static ApiResponse<T> Ok(T d, string text) =>
        new([new("2000", text, "Success", 200)], d);
}

public interface IEmployeeService
{
    Task<IReadOnlyList<ItemResult>> UpsertAsync(
        IReadOnlyList<EmployeeRequest> x,
        string actor,
        CancellationToken ct
    );
    Task<IReadOnlyList<ItemResult>> UpdateAsync(
        IReadOnlyList<EmployeeRequest> x,
        string actor,
        CancellationToken ct
    );
    Task<IReadOnlyList<ItemResult>> ResignAsync(
        IReadOnlyList<ResignRequest> x,
        string actor,
        CancellationToken ct
    );
}
