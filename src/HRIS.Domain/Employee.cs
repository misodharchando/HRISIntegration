namespace HRIS.Domain;

public sealed class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string TenantId { get; set; }
    public required string CompanyId { get; set; }
    public required string SiteId { get; set; }
    public required string EmployeeNo { get; set; }
    public required string EmpName { get; set; }
    public string? EmpNameSecondLanguage { get; set; }
    public string? Alias { get; set; }
    public string? AliasSecondLanguage { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    public required string NFCUId { get; set; }
    public int EmpCategoryId { get; set; }
    public required string EmpCategoryName { get; set; }
    public int EmpTypeId { get; set; }
    public required string EmpTypeName { get; set; }
    public DateOnly EmpJoinedDate { get; set; }
    public string? UserId { get; set; }
    public string? Image { get; set; }
    public string? PreviousEmployeeNo { get; set; }
    public string? BandId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateOnly? ResignDate { get; set; }
    public string CreatedBy { get; set; } = "system";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
