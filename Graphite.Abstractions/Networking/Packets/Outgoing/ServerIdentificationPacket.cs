namespace Graphite.Abstractions.Networking.Packets.Outgoing;

public sealed class ServerIdentificationPacket : IOutgoingPacket
{
	public byte Type => 0x00;

	public int Length => 130;

	public required string Name { get; init; }

	public required string MessageOfTheDay { get; init; }

	public required byte IsOperator { get; init; }

	public void Write(ref SpanWriter writer)
	{
		writer.WriteByte(Protocol.Version);
		writer.WriteString(Name);
		writer.WriteString(MessageOfTheDay);
		writer.WriteByte(IsOperator);
	}
}