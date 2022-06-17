using System.Reflection;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Logging;
using Microsoft.EntityFrameworkCore;
using Sample.Api;
using Sample.Components;
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

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<RegistrationDbContext>(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    x.UseNpgsql(connectionString, options =>
    {
        options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        options.MigrationsHistoryTable($"__{nameof(RegistrationDbContext)}");

        options.MinBatchSize(1);
    });
});

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOnRamp<RegistrationDbContext>();

    x.UsingRabbitMq((_, cfg) => { cfg.AutoStart = true; });
});

builder.Services.AddHostedService<RecreateDatabaseHostedService<RegistrationDbContext>>();
builder.Services.AddEntityFrameworkOnRampDeliveryService<RegistrationDbContext>(options => options.SweepInterval = TimeSpan.FromSeconds(1));

builder.Services.AddSingleton<ILockStatementProvider, PostgresLockStatementProvider>();

builder.Services.AddOptions<TextWriterLoggerOptions>().Configure(options => options.Disable("Microsoft"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();