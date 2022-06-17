namespace Sample.Components.Contracts;

public record AddEventAttendee
{
    public Guid RegistrationId { get; init; }
    public string MemberId { get; init; } = null!;
    public string EventId { get; init; } = null!;
}