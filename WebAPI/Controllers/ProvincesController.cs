using Application.Common;
using Application.DTOs;
using Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Authorize(Policy = "OperationalAdmin")]
[Route("api/provinces")]
public sealed class ProvincesController(
    IProvinceCatalogService catalogService,
    IProvinceSyncService syncService,
    ILogger<ProvincesController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProvinceOption>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var provinces = await catalogService.ListAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ProvinceOption>>.Ok(provinces));
    }

    [HttpPost("sync")]
    [ProducesResponseType(typeof(ApiResponse<ProvinceSyncResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Synchronize(CancellationToken cancellationToken)
    {
        try
        {
            var result = await syncService.SynchronizeAsync(cancellationToken);
            logger.LogInformation(
                "Province catalog synchronized from {Provider}; {ProvinceCount} records at {SynchronizedAt}",
                result.Provider,
                result.ProvinceCount,
                result.SynchronizedAt);
            return Ok(ApiResponse<ProvinceSyncResult>.Ok(result));
        }
        catch (Exception exception) when (
            exception is InvalidDataException or HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(exception, "Province catalog synchronization failed");
            return StatusCode(
                StatusCodes.Status502BadGateway,
                ApiResponse<object>.Fail(
                    "PROVINCE_PROVIDER_UNAVAILABLE",
                    "Không thể cập nhật danh mục tỉnh. Dữ liệu gần nhất vẫn được giữ nguyên."));
        }
    }
}
