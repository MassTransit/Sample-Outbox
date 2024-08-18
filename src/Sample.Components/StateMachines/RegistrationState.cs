namespace Sample.Components.StateMachines;

using MassTransit;


public class RegistrationState :
    SagaStateMachineInstance,
    ISagaVersion
{
    public string CurrentState { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public string EventId { get; set; } = null!;
    public string MemberId { get; set; } = null!;
    public decimal Payment { get; set; }
    public int Version { get; set; }
    public Guid CorrelationId { get; set; }
}