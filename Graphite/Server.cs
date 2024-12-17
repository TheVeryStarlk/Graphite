using Graphite.Abstractions;
using Graphite.Abstractions.Eventing.Sources.Listener;
using Graphite.Eventing;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Graphite;

internal sealed class Server(
    ILoggerFactory loggerFactory,
    IConnectionListenerFactory listenerFactory,
    EventDispatcher eventDispatcher,
    Func<ConnectionContext, byte, Client> clientFactory) : IServer, IDisposable
{
    public IReadOnlyDictionary<string, IPlayer> Players => pairs.Values
        .Where(pair => pair.Client.Player is not null)
        .Select(pair => pair.Client.Player!)
        .ToDictionary(selector => selector.Username, selector => selector);

    private string reason = "No reason specified.";
    private CancellationTokenSource? source;

    private readonly ILogger<Server> logger = loggerFactory.CreateLogger<Server>();
    private readonly ConcurrentDictionary<byte, (Client Client, Task Exceution)> pairs = [];

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var starting = await eventDispatcher
            .DispatchAsync(this, new Starting(), source.Token)
            .ConfigureAwait(false);

        await using var listener = await listenerFactory
            .BindAsync(starting.EndPoint, source.Token)
            .ConfigureAwait(false);

        logger.LogInformation("Started listening on: \"{endPoint}\"", starting.EndPoint);

        byte identifier = 0;

        while (!source.Token.IsCancellationRequested)
        {
            try
            {
                var connection = await listener.AcceptAsync(source.Token).ConfigureAwait(false);
                var client = clientFactory(connection!, identifier);

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
            .DispatchAsync(this, new Stopping(reason), source.Token)
            .ConfigureAwait(false);

        await listener.UnbindAsync(CancellationToken.None).ConfigureAwait(false);

        logger.LogInformation("Unbound the listener. Because: \"{Reason}\"", stopping.Reason);

        foreach (var pair in pairs.Values)
        {
            pair.Client.Stop(stopping.Reason);
        }

        logger.LogInformation("Waiting for clients to disconnect");

        await Task.WhenAll(pairs.Values.Select(pair => pair.Exceution))
            .TimeoutAfter(stopping.Timeout)
            .ConfigureAwait(false);

        return;

        async Task ExecuteAsync(Client client)
        {
            await Task.Yield();

            await client.StartAsync().ConfigureAwait(false);
            client.Dispose();

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