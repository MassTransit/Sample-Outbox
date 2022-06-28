namespace Sample.Components;

using Contracts;
using MassTransit;
using MassTransit.MongoDbIntegration;
using MongoDB.Driver;


public class RegistrationService :
    IRegistrationService
{
    readonly MongoDbContext _dbContext;
    readonly IMongoCollection<Registration> _registrations;
    readonly IPublishEndpoint _publishEndpoint;

    public RegistrationService(MongoDbContext dbContext, IMongoCollection<Registration> registrations, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _registrations = registrations;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Registration> SubmitRegistration(string eventId, string memberId, decimal payment)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        var registration = new Registration
        {
            RegistrationId = NewId.NextGuid(),
            RegistrationDate = DateTime.UtcNow,
            MemberId = memberId,
            EventId = eventId,
            Payment = payment
        };

        await _dbContext.BeginTransaction(cts.Token);

        await _registrations.InsertOneAsync(_dbContext.Session, registration, null, cts.Token);

        await _publishEndpoint.Publish(new RegistrationSubmitted
        {
            RegistrationId = registration.RegistrationId,
            RegistrationDate = registration.RegistrationDate,
            MemberId = registration.MemberId,
            EventId = registration.EventId,
            Payment = payment
        }, cts.Token);

        try
        {
            await _dbContext.CommitTransaction(cts.Token);
        }
        catch (MongoCommandException exception) when (exception.CodeName == "DuplicateKey")
        {
            throw new DuplicateRegistrationException("Duplicate registration", exception);
        }

        return registration;
    }
}