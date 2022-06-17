namespace Sample.Components.StateMachines;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class RegistrationStateMap :
    SagaClassMap<RegistrationState>
{
    protected override void Configure(EntityTypeBuilder<RegistrationState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState);
        
        entity.Property(x => x.RegistrationDate);
        entity.Property(x => x.EventId);
        entity.Property(x => x.MemberId);
        entity.Property(x => x.Payment);
    }
}