using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Sample.Components;
using Sample.Components.Consumers;
using Sample.Components.StateMachines;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<RegistrationDbContext>(x =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("Default");

            x.UseNpgsql(connectionString, options =>
            {
                options.MinBatchSize(1);
            });
        });

        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<RegistrationDbContext>(o =>
            {
                o.UsePostgres();
                
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });
            
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<NotifyRegistrationConsumer>();
            x.AddConsumer<SendRegistrationEmailConsumer>();
            x.AddConsumer<AddEventAttendeeConsumer>();
            x.AddSagaStateMachine<RegistrationStateMachine, RegistrationState, RegistrationStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<RegistrationDbContext>();
                    r.UsePostgres();
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .UseSerilog()
    .Build();

await host.RunAsync();