namespace Graphite.Networking.Packets.Ingoing;

public sealed class PlayerIdentificationPacket : IIngoingPacket<PlayerIdentificationPacket>, IPacket
{
	public static byte Type => 0x00;

	public static int Length => 130;

	public required byte ProtocolVersion { get; init; }

	public required string Username { get; init; }

	public required string VerificationKey { get; init; }

	public required byte Unused { get; init; }

	public static PlayerIdentificationPacket Create(SpanReader reader)
	{
		return new PlayerIdentificationPacket
		{
			ProtocolVersion = reader.ReadByte(),
			Username = reader.ReadString(),
			VerificationKey = reader.ReadString(),
			Unused = reader.ReadByte()
		};
	}
}