namespace Sample.Components;

using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;


public class RegistrationService :
    IRegistrationService
{
    readonly RegistrationDbContext _dbContext;
    readonly IPublishEndpoint _publishEndpoint;

    public RegistrationService(RegistrationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Registration> SubmitRegistration(string eventId, string memberId, decimal payment)
    {
        var registration = new Registration
        {
            RegistrationId = NewId.NextGuid(),
            RegistrationDate = DateTime.UtcNow,
            MemberId = memberId,
            EventId = eventId,
            Payment = payment
        };

        await _dbContext.Set<Registration>().AddAsync(registration);

        await _publishEndpoint.Publish(new RegistrationSubmitted
        {
            RegistrationId = registration.RegistrationId,
            RegistrationDate = registration.RegistrationDate,
            MemberId = registration.MemberId,
            EventId = registration.EventId,
            Payment = payment
        });

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new DuplicateRegistrationException("Duplicate registration", exception);
        }

        return registration;
    }
}