using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Graphite.Hosting;

internal sealed class WorkerService(ILogger<WorkerService> logger, Server server) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		try
		{
			await server.StartAsync(cancellationToken).ConfigureAwait(false);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "An exception has occurred.");
		}
		finally
		{
			server.Dispose();
		}
	}
}