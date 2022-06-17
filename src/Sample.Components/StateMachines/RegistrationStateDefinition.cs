namespace Sample.Components.StateMachines;

using MassTransit;


public class RegistrationStateDefinition :
    SagaDefinition<RegistrationState>
{
    readonly IServiceProvider _provider;

    public RegistrationStateDefinition(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<RegistrationState> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

        endpointConfigurator.UseEntityFrameworkOutbox<RegistrationDbContext>(_provider);
    }
}