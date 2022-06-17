namespace Sample.Components;

public interface IRegistrationService
{
    Task<Registration> SubmitRegistration(string eventId, string memberId, decimal payment);
}