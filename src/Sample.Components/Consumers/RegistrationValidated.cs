namespace Sample.Components.Consumers;

public record RegistrationValidated
{
    public Guid RegistrationId { get; init; }
}