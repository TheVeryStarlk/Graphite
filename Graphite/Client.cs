using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Graphite;

public sealed class Client(
	ILogger<Client> logger,
	ConnectionContext connection,
	byte identifier) : IDisposable
{
	public byte Identifier => identifier;

	private readonly CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(connection.ConnectionClosed);

	private string reason = "You have been disconnected from the server.";

	public async Task StartAsync()
	{
		while (!source.IsCancellationRequested)
		{
		}

		connection.Abort(new ConnectionAbortedException(reason));
		await connection.DisposeAsync().ConfigureAwait(false);
	}

	public void Stop(string stop)
	{
		reason = stop;
		source.Cancel();
	}

	public void Dispose()
	{
		source.Dispose();
	}
}