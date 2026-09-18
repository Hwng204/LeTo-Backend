using Application.Common.Security;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/matrix-reference-data")]
public sealed class MatrixReferenceDataController(
    IMatrixCurrentUser currentUser,
    IMatrixTaskReferenceReader reader) : ControllerBase
{
    [HttpGet]
    public Task<MatrixReferenceData> Get(
        [FromQuery] ulong? academicContextId = null,
        CancellationToken cancellationToken = default)
    {
        return reader.GetAsync(currentUser.Actor, academicContextId, cancellationToken);
    }
}
