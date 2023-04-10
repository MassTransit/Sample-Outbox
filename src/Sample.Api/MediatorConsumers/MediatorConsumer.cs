using MassTransit;
using Sample.Api.MediatorContracts;

namespace Sample.Api.MediatorConsumers;

public class MediatorConsumer : IConsumer<MediatorRequest>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MediatorConsumer(
        // If this is the first time resolution for IPublishEndpoint, we get a default implementation w/o Transactional Outbox.
        IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<MediatorRequest> context)
    {
        Console.WriteLine($"Publish Endpoint resolution inside Mediator consumer: {_publishEndpoint.GetType()}");
        await context.RespondAsync(new MediatorResponse("My response"));
    }
}