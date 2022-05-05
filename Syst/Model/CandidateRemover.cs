using Core;

public class CandidateRemover: IHostedService, IDisposable
{
    private Timer? _timer;
    private IServiceProvider _services;


    public CandidateRemover(IServiceProvider services)
    {
        _services = services;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // timer repeates call to RemoveScheduledAccounts every 24 hours.
        _timer = new Timer(
            RemoveOldCandidates,
            null, 
            TimeSpan.Zero, 
            TimeSpan.FromHours(24) //Delete interval
        );

        return Task.CompletedTask;
    }

    // Call the Stop async method if required from within the app.
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async void RemoveOldCandidates(Object? state) {
        using (var scope = _services.CreateScope()) {
            var repo = scope.ServiceProvider.GetRequiredService<ICandidateRepository>();

            await repo.DeleteOldCandidates();
        }
    }
}