using FluentValidation;

namespace HRIS.Application;

public sealed class EmployeeValidator : AbstractValidator<EmployeeRequest>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().Length(4);
        RuleFor(x => x.CompanyId).NotEmpty().Length(4);
        RuleFor(x => x.SiteId).NotEmpty().MaximumLength(10);
        RuleFor(x => x.EmployeeNo).NotEmpty().MaximumLength(10);
        RuleFor(x => x.EmpName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NFCUId).NotEmpty().MaximumLength(30);
        RuleFor(x => x.EmpCategoryId).GreaterThan(0);
        RuleFor(x => x.EmpCategoryName).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EmpTypeId).GreaterThan(0);
        RuleFor(x => x.EmpTypeName).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}

public sealed class ResignValidator : AbstractValidator<ResignRequest>
{
    public ResignValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().Length(4);
        RuleFor(x => x.CompanyId).NotEmpty().Length(4);
        RuleFor(x => x.SiteId).NotEmpty().MaximumLength(10);
        RuleFor(x => x.EmployeeNo).NotEmpty().MaximumLength(10);
    }
}
