using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using ShiftSchedulingSystem.Presentation.Validations;
using System.Text.Json;

namespace ShiftSchedulingSystem.Presentation.Controllers;

[Route("api/workers")]
[ApiController]
[ResponseCache(CacheProfileName = "120SecondsDuration")]
[ApiExplorerSettings(GroupName = "v1")]
//[OutputCache(PolicyName = "120SecondsDuration")]
public class WorkersController : ControllerBase
{
    #region Properties
    private readonly IServiceManager _service;
    #endregion

    #region Constructor
    public WorkersController(IServiceManager service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }
    #endregion

    #region Methods
    [HttpGet]
    [Authorize(Roles = "Manager")]
    //[DisableRateLimiting]
    public async Task<IActionResult> GetWorkers([FromQuery] WorkerParameters parameters)
    {
        var pagedResult = await _service.WorkerService.GetWorkersAsync(parameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
        return Ok(pagedResult.workers);
    }

    [HttpGet("{id:guid}", Name = "WorkerById")]
    [ResponseCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")]
    //[OutPutCache(Duration = 60)]
    public async Task<IActionResult> GetWorker(Guid id)
    {
        var shift = await _service.WorkerService.GetWorkerAsync(id, trackChanges: false);
        return Ok(shift);
    }

    [HttpPost]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateWorker([FromBody] WorkerForCreationDto worker)
    {
        var validator = new WorkerForCreationValidator();
        var validationResult = await validator.ValidateAsync(worker);
        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors.Select(x => x.ErrorMessage));

        var createdWorker = await _service.WorkerService.CreateWorkerAsync(worker);
        return CreatedAtRoute("WorkerById", new { id = createdWorker.Id },
        createdWorker);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteWorker(Guid id)
    {
        await _service.WorkerService.DeleteWorkerAsync(id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateWorker(Guid id, [FromBody] WorkerForUpdateDto worker)
    {
        var validator = new WorkerForUpdateValidator();
        var validationResult = await validator.ValidateAsync(worker);
        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors.Select(x => x.ErrorMessage));

        await _service.WorkerService.UpdateWorkerAsync(id, worker, trackChanges: true);
        return NoContent();
    }
    #endregion
}
