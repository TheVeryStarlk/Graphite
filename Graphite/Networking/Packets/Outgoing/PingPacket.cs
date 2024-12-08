namespace Graphite.Networking.Packets.Outgoing;

public sealed class PingPacket : IOutgoingPacket
{
	public byte Type => 0x01;

	public int Length => 0;

	public void Write(ref SpanWriter writer)
	{
	}
}