using Microsoft.Extensions.Hosting;

namespace Graphite;

internal sealed class WorkerService(Listener listener) : BackgroundService
{
	protected override Task ExecuteAsync(CancellationToken cancellationToken)
	{
		return listener.StartAsync(cancellationToken);
	}
}