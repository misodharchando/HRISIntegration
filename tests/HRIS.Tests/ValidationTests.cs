using HRIS.Application;

namespace HRIS.Tests;

public sealed class ValidationTests
{
    [Fact]
    public async Task Invalid_identity_fails()
    {
        var x = new EmployeeRequest("A", "B", "", "", "", 0, "", 0, "", "", DateOnly.MinValue);
        var r = await new EmployeeValidator().ValidateAsync(x);
        Assert.False(r.IsValid);
    }
}
