using Graphite.Abstractions;
using Graphite.Abstractions.Eventing.Sources.Listener;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Graphite;

internal sealed class Listener(
	ILoggerFactory loggerFactory,
	IConnectionListenerFactory listenerFactory,
	EventDispatcher eventDispatcher,
	Func<ConnectionContext, Client> clientFactory) : IListener, IDisposable
{
	private string reason = "No reason specified.";
	private CancellationTokenSource? source;

	private readonly ILogger<Listener> logger = loggerFactory.CreateLogger<Listener>();

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

		var starting = await eventDispatcher
			.DispatchAsync(new Starting(), source.Token)
			.ConfigureAwait(false);

		await using var listener = await listenerFactory
			.BindAsync(starting.EndPoint, source.Token)
			.ConfigureAwait(false);

		logger.LogInformation("Started listening on: \"{endPoint}\"", starting.EndPoint);

		while (!source.Token.IsCancellationRequested)
		{
			try
			{
				await using var connection = await listener.AcceptAsync(source.Token).ConfigureAwait(false);
				var client = clientFactory(connection!);
			}
			catch (OperationCanceledException)
			{
				// Nothing.
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Unexpected exception while listening");
				break;
			}
		}

		var stopping = await eventDispatcher
			.DispatchAsync(new Stopping(reason), source.Token)
			.ConfigureAwait(false);

		await listener.UnbindAsync(CancellationToken.None).ConfigureAwait(false);

		logger.LogInformation("Unbound the listener. Because: \"{Reason}\"", stopping.Reason);
	}

	public void Stop(string stop)
	{
		reason = stop;
		source?.Cancel();
	}

	public void Dispose()
	{
		source?.Dispose();
		loggerFactory.Dispose();
	}
}