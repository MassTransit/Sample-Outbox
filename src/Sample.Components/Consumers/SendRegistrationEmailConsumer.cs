namespace Sample.Components.Consumers;

using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;


public class SendRegistrationEmailConsumer :
    IConsumer<SendRegistrationEmail>
{
    readonly ILogger<SendRegistrationEmailConsumer> _logger;

    public SendRegistrationEmailConsumer(ILogger<SendRegistrationEmailConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SendRegistrationEmail> context)
    {
        _logger.LogInformation("Notifying Member {MemberId} that they registered for event {EventId} on {RegistrationDate}", context.Message.MemberId,
            context.Message.EventId, context.Message.RegistrationDate);

        return Task.CompletedTask;
    }
}