namespace Graphite.Networking.Packets.Outgoing;

public sealed class DespawnPacket : IOutgoingPacket
{
	public byte Type => 0x0C;

	public int Length => 1;

	public required byte Identifier { get; init; }

	public void Write(ref SpanWriter writer)
	{
		writer.WriteByte(Identifier);
	}
}