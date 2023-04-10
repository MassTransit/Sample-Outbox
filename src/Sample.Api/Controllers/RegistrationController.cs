using MassTransit;
using MassTransit.Mediator;
using Sample.Api.MediatorContracts;

namespace Sample.Api.Controllers;

using Components;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class RegistrationController :
    ControllerBase
{
    private readonly IScopedMediator _scopedMediator;

    public RegistrationController(

        // If uncomment IPublishEndpoint here, a correct Transactional Outbox aware implementation is injected.
        // And then, the same correct instance is injected in all other places including MediatorConsumer.
        // But, if MediatorConsumer is the first time where IPublishEndpoint is resolved, it gets a default implementation w/o Transactional Outbox.

        // IPublishEndpoint publishEndpoint,
        IScopedMediator scopedMediator)
    {
        _scopedMediator = scopedMediator;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] RegistrationModel model)
    {
        var response = await _scopedMediator
                .CreateRequest<MediatorRequest>(new MediatorRequest($"My request with Event ID = {model.EventId} and Member ID = {model.MemberId}"))
                .GetResponse<MediatorResponse>();
        return Ok(response.Message);
    }
}