using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Graphite.Hosting;

internal sealed class WorkerService(ILogger<WorkerService> logger, Listener listener) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await listener.StartAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error has occurred.");
        }
    }
}