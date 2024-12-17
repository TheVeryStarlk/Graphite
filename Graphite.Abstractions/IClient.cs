using Graphite.Abstractions.Networking;

namespace Graphite.Abstractions;

public interface IClient
{
    public byte Identifier { get; }

    public IServer Server { get; }

    public IPlayer? Player { get; }

    public ValueTask WriteAsync(params IOutgoingPacket[] packets);

    public void Stop(string reason);
}