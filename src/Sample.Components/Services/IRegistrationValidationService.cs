namespace Sample.Components.Services;

public interface IRegistrationValidationService
{
    Task ValidateRegistration(string eventId, string memberId, Guid registrationId);
}