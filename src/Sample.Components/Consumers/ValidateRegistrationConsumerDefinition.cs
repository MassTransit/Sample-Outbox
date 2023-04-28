namespace Sample.Components.Consumers;

using MassTransit;

public class ValidateRegistrationConsumerDefinition :
    ConsumerDefinition<ValidateRegistrationConsumer>
{
    readonly IServiceProvider _provider;

    public ValidateRegistrationConsumerDefinition(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ValidateRegistrationConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

        endpointConfigurator.UseEntityFrameworkOutbox<RegistrationDbContext>(context);
    }
}
