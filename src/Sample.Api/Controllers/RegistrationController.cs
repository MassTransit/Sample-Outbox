namespace Sample.Api.Controllers;

using Components;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class RegistrationController :
    ControllerBase
{
    readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] RegistrationModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var registration = await _registrationService.SubmitRegistration(model.EventId, model.MemberId, model.Payment);

            return Ok(new
            {
                registration.RegistrationId,
                registration.RegistrationDate,
                registration.MemberId,
                registration.EventId,
            });
        }
        catch (DuplicateRegistrationException)
        {
            return Conflict(new
            {
                model.MemberId,
                model.EventId,
            });
        }
    }
}