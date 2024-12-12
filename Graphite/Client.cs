using Graphite.Abstractions;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Graphite;

internal sealed class Client(ILogger<Client> logger, ConnectionContext connection) : IClient, IDisposable
{
	private string reason = "No reason specified.";
	private CancellationTokenSource? source;

	public void Stop(string stop)
	{
		reason = stop;
	}

	public void Dispose()
	{
		source?.Dispose();
	}
}