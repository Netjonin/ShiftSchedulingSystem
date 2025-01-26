using Entities.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using ShiftSchedulingSystem.Presentation.ActionFilters;
using ShiftSchedulingSystem.Presentation.Validations;
using System.Text.Json;

namespace ShiftSchedulingSystem.Presentation.Controllers;

[Route("api/workers")]
[ApiController]
public class ShiftsController : ControllerBase
{
    #region Properties
    private readonly IServiceManager _service;
    #endregion

    #region Constructor
    public ShiftsController(IServiceManager service) => _service = service ?? throw new ArgumentNullException(nameof(service));

    #endregion

    #region Methods
    [HttpGet("{id}/shifts")]
    public async Task<IActionResult> GetShiftsForWorker(Guid id, [FromQuery] ShiftParameters parameters)
    {
        var pagedResult = await _service.ShiftService.GetShiftsAsync(id, parameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
        return Ok(pagedResult.shifts);
    }

    [HttpGet("{workerid}/shifts/{id:guid}", Name = "GetShiftsForWorker")]
    public async Task<IActionResult> GetShiftForWorker(Guid workerId, Guid id)
    {
        var worker = await _service.ShiftService.GetShiftAsync(workerId, id,
        trackChanges: false);
        return Ok(worker);
    }

    [HttpPost("{workerid}/shifts")]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateShiftForWorker(Guid workerId, [FromBody] ShiftForCreationDto shift)
    {
        var validator = new ShiftForCreationValidator();
        var validationResult = await validator.ValidateAsync(shift);
        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors.Select(x => x.ErrorMessage));

        var shiftToReturn =
        await _service.ShiftService.CreateShiftAsync(workerId, shift, trackChanges: false);
        return CreatedAtRoute("GetShiftsForWorker", new { workerId, id = shiftToReturn.Id }, shiftToReturn);
    }

    [HttpDelete("{workerid}/shifts/{id:guid}")]
    public async Task<IActionResult> DeleteShiftForWorker(Guid workerId, Guid id)
    {
        await _service.ShiftService.DeleteShiftAsync(workerId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{workerid}/shifts/{id:guid}")]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateShiftForWorker(Guid workerId, Guid id, [FromBody] ShiftForUpdateDto shift)
    {
        var validator = new ShiftForUpdateValidator();
        var validationResult = await validator.ValidateAsync(shift);
        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors.Select(x => x.ErrorMessage));

        await _service.ShiftService.UpdateShiftAsync(workerId, id, shift,
        workerTrackChanges: false, shiftTrackChanges: true);
        return NoContent();
    }
    #endregion
}
