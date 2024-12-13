namespace Graphite.Abstractions.Networking.Packets.Outgoing;

public sealed class WorldFinalizePacket : IOutgoingPacket
{
	public byte Type => 0x04;

	int IOutgoingPacket.Length => 6;

	public required short Width { get; init; }

	public required short Height { get; init; }

	public required short Length { get; init; }

	public void Write(ref SpanWriter writer)
	{
		writer.WriteShort(Width);
		writer.WriteShort(Height);
		writer.WriteShort(Length);
	}
}