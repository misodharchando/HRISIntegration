using HRIS.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.Api.Controllers;

[ApiController, Route("api/[controller]"), Authorize]
public sealed class HRISIntegrationController(IEmployeeService s) : ControllerBase
{
    string Actor => User.Identity?.Name ?? "api";

    [HttpPost("InsertEmployees")]
    public async Task<IActionResult> Insert(List<EmployeeRequest> x, CancellationToken ct) =>
        Ok(
            ApiResponse<IReadOnlyList<ItemResult>>.Ok(
                await s.UpsertAsync(x, Actor, ct),
                "Processing completed."
            )
        );

    [HttpPut("UpdateEmployees")]
    public async Task<IActionResult> Update(List<EmployeeRequest> x, CancellationToken ct) =>
        Ok(
            ApiResponse<IReadOnlyList<ItemResult>>.Ok(
                await s.UpdateAsync(x, Actor, ct),
                "Processing completed."
            )
        );

    [HttpPost("ResignEmployees")]
    public async Task<IActionResult> Resign(List<ResignRequest> x, CancellationToken ct) =>
        Ok(
            ApiResponse<IReadOnlyList<ItemResult>>.Ok(
                await s.ResignAsync(x, Actor, ct),
                "Processing completed."
            )
        );
}
