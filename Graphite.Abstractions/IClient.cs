using Graphite.Abstractions.Networking;

namespace Graphite.Abstractions;

public interface IClient
{
    public ValueTask WriteAsync(params IOutgoingPacket[] packets);

    public void Stop(string reason);
}