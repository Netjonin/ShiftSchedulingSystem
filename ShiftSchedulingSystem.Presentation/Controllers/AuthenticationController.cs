using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using ShiftSchedulingSystem.Presentation.ActionFilters;
using ShiftSchedulingSystem.Presentation.Validations;

namespace ShiftSchedulingSystem.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;
    public AuthenticationController(IServiceManager service) => _service = service;

    [HttpPost]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var validator = new UserRegistrationValidator();
        var validationResult = await validator.ValidateAsync(userForRegistration);
        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors.Select(x => x.ErrorMessage));

        var result = await _service.AuthenticationService.RegisterUser(userForRegistration);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        var validator = new UserLoginValidator();
        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return UnprocessableEntity(validationResult.Errors.Select(x => x.ErrorMessage));

        if (!await _service.AuthenticationService.ValidateUser(user))
            return Unauthorized();
        return Ok(new
        {
            Token = await _service
        .AuthenticationService.CreateToken()
        });
    }
}

