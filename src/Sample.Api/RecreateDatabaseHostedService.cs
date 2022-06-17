using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Sample.Api;

public class RecreateDatabaseHostedService<TDbContext> :
    IHostedService
    where TDbContext : DbContext
{
    readonly ILogger<RecreateDatabaseHostedService<TDbContext>> _logger;
    readonly IServiceScopeFactory _scopeFactory;
    TDbContext? _context;
    IServiceScope? _scope;

    public RecreateDatabaseHostedService(IServiceScopeFactory scopeFactory, ILogger<RecreateDatabaseHostedService<TDbContext>> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Applying migrations for {DbContext}", TypeCache<TDbContext>.ShortName);

        _scope = _scopeFactory.CreateScope();

        _context = _scope.ServiceProvider.GetRequiredService<TDbContext>();

        await _context.Database.EnsureDeletedAsync(cancellationToken);
        await _context.Database.EnsureCreatedAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}