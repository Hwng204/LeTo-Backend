using Application.DTOs;
using Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/matrix-reference-data")]
public sealed class MatrixReferenceDataController(
    IMatrixTaskApplicationService service) : ControllerBase
{
    [HttpGet]
    public Task<MatrixReferenceData> Get(
        [FromQuery] ulong? academicContextId = null,
        CancellationToken cancellationToken = default)
    {
        return service.GetReferenceDataAsync(academicContextId, cancellationToken);
    }
}
