namespace Sample.Components.Consumers;

using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Services;


public class ValidateRegistrationConsumer :
    IConsumer<AddEventAttendee>
{
    readonly ILogger<ValidateRegistrationConsumer> _logger;
    readonly IRegistrationValidationService _validationService;

    public ValidateRegistrationConsumer(ILogger<ValidateRegistrationConsumer> logger, IRegistrationValidationService validationService)
    {
        _logger = logger;
        _validationService = validationService;
    }

    public async Task Consume(ConsumeContext<AddEventAttendee> context)
    {
        await _validationService.ValidateRegistration(context.Message.EventId, context.Message.MemberId, context.Message.RegistrationId);
    }
}