namespace Graphite.Networking.Packets;

public sealed class MessagePacket : IIngoingPacket<MessagePacket>, IOutgoingPacket
{
	public static byte Type => 0x0D;

	public static int Length => 65;

	byte IOutgoingPacket.Type => Type;

	int IOutgoingPacket.Length => Length;

	public required byte Identifier { get; init; }

	public required string Message { get; init; }

	public static MessagePacket Create(SpanReader reader)
	{
		return new MessagePacket
		{
			Identifier = reader.ReadByte(),
			Message = reader.ReadString()
		};
	}

	public void Write(ref SpanWriter writer)
	{
		writer.WriteByte(Type);
		writer.WriteString(Message);
	}
}