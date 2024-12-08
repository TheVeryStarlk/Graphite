namespace Graphite.Networking.Packets.Outgoing;

public sealed class WorldInitializePacket : IOutgoingPacket
{
	public byte Type => 0x02;

	public int Length => 0;

	public void Write(ref SpanWriter writer)
	{
	}
}