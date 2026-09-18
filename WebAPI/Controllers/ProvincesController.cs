using Application.Common;
using Application.Provinces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Authorize(Policy = "OperationalAdmin")]
[Route("api/provinces")]
public sealed class ProvincesController(IProvinceCatalogService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProvinceOption>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var provinces = await service.ListAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ProvinceOption>>.Ok(provinces));
    }
}
