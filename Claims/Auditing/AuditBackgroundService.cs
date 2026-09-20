namespace Claims.Auditing
{
    /// <summary>
    /// Processes queued audit entries and persists them to the database
    /// </summary>
    public class AuditBackgroundService : BackgroundService
    {
        private readonly IAuditQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AuditBackgroundService> _logger;
        public AuditBackgroundService(IAuditQueue queue, IServiceScopeFactory scopeFactory, ILogger<AuditBackgroundService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var auditEntry = await _queue.DequeueAsync(stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuditContext>();

                context.Add(auditEntry);
                await context.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Audit entry persisted: {AuditEntry}", auditEntry);
            }
        }
    }
}