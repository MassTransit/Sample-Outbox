namespace Sample.Components.StateMachines;

using MassTransit;


public class RegistrationStateDefinition :
    SagaDefinition<RegistrationState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<RegistrationState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

        endpointConfigurator.UseEntityFrameworkOutbox<RegistrationDbContext>(context);
    }
}