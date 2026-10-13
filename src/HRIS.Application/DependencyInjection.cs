using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace HRIS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection s)
    {
        s.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
        s.AddScoped<IEmployeeService, EmployeeService>();
        return s;
    }
}
