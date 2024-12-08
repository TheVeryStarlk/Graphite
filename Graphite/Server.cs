using System.Net;
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

	private CancellationTokenSource? source;

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

		var endPoint = new IPEndPoint(IPAddress.Any, 25565);

		var starting = await eventDispatcher
			.DispatchAsync(new Starting(this, endPoint), source.Token)
			.ConfigureAwait(false);

		await using var listener = await listenerFactory
			.BindAsync(starting.EndPoint, source.Token)
			.ConfigureAwait(false);

		logger.LogInformation("Started listening on: {endPoint}", starting.EndPoint);

		var stopping = await eventDispatcher
			.DispatchAsync(new Stopping(this), source.Token)
			.ConfigureAwait(false);

		await listener.UnbindAsync(CancellationToken.None).ConfigureAwait(false);

		logger.LogInformation("Unbound the listener. Stopped listening");
		logger.LogInformation("Server stopped. Reason: {reason}", stopping.Reason);
	}

	public void Dispose()
	{
		source?.Dispose();
		loggerFactory.Dispose();
	}
}