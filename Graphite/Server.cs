using System.Collections.Concurrent;
using Graphite.Eventing;
using Graphite.Eventing.Sources.Server;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Graphite;

public sealed class Server(
	ILoggerFactory loggerFactory,
	IConnectionListenerFactory listenerFactory,
	EventDispatcher eventDispatcher) : IDisposable
{
	private readonly ILogger<Server> logger = loggerFactory.CreateLogger<Server>();
	private readonly ConcurrentDictionary<byte, (Client Client, Task Exceution)> pairs = [];

	private string reason = "Server stopped.";
	private CancellationTokenSource? source;

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

		var starting = await eventDispatcher
			.DispatchAsync(new Starting(this), source.Token)
			.ConfigureAwait(false);

		await using var listener = await listenerFactory
			.BindAsync(starting.EndPoint, source.Token)
			.ConfigureAwait(false);

		logger.LogInformation("Started listening on: {endPoint}", starting.EndPoint);

		byte identifier = 0;

		while (!source.Token.IsCancellationRequested)
		{
			try
			{
				var connection = await listener.AcceptAsync(source.Token).ConfigureAwait(false);

				var client = new Client(
					loggerFactory.CreateLogger<Client>(),
					connection!,
					eventDispatcher,
					identifier);

				pairs[identifier] = (client, ExecuteAsync(client));

				identifier++;
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
			.DispatchAsync(new Stopping(this, reason), source.Token)
			.ConfigureAwait(false);

		await listener.UnbindAsync(CancellationToken.None).ConfigureAwait(false);

		logger.LogInformation("Unbound the listener. Stopped listening");

		foreach (var pair in pairs.Values)
		{
			pair.Client.Stop(stopping.Reason);
		}

		await Task.WhenAll(pairs.Values.Select(pair => pair.Exceution))
			.TimeoutAfter(stopping.Timeout)
			.ConfigureAwait(false);

		logger.LogInformation("Server stopped. Reason: {reason}", stopping.Reason);

		return;

		async Task ExecuteAsync(Client client)
		{
			await Task.Yield();

			await client.StartAsync().ConfigureAwait(false);
			await client.DisposeAsync().ConfigureAwait(false);

			if (!pairs.TryRemove(client.Identifier, out _))
			{
				logger.LogWarning("Failed to remove client");
			}
		}
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