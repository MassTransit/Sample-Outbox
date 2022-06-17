namespace Sample.Components.Consumers;

using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;


public class NotifyRegistrationConsumer :
    IConsumer<RegistrationSubmitted>
{
    readonly ILogger<NotifyRegistrationConsumer> _logger;

    public NotifyRegistrationConsumer(ILogger<NotifyRegistrationConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RegistrationSubmitted> context)
    {
        _logger.LogInformation("Member {MemberId} registered for event {EventId} on {RegistrationDate}", context.Message.MemberId, context.Message.EventId,
            context.Message.RegistrationDate);

        return Task.CompletedTask;
    }
}