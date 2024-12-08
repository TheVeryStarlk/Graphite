namespace Graphite.Networking.Packets.Outgoing;

public sealed class WorldPacket : IOutgoingPacket
{
	public byte Type => 0x03;

	public int Length => 1027;

	public required byte[] Blocks { get; init; }

	public required byte PercentComplete { get; init; }

	public void Write(ref SpanWriter writer)
	{
		writer.WriteShort((short) Blocks.Length);
		writer.Write(Blocks);
		writer.Write(new byte[1024 - Blocks.Length]);
		writer.WriteByte(PercentComplete);
	}
}