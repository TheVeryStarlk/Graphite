using Graphite.Abstractions.Worlds;

namespace Graphite.Abstractions.Networking.Packets.Ingoing;

public sealed class BlockPlacementPacket : IIngoingPacket<BlockPlacementPacket>, IPacket
{
	public static byte Type => 0x05;

	public static int Length => 8;

	public required short X { get; init; }

	public required short Y { get; init; }

	public required short Z { get; init; }

	public required PlacementMode Mode { get; init; }

	public required Block Block { get; init; }

	public static BlockPlacementPacket Create(SpanReader reader)
	{
		return new BlockPlacementPacket
		{
			X = reader.ReadShort(),
			Y = reader.ReadShort(),
			Z = reader.ReadShort(),
			Mode = (PlacementMode) reader.ReadByte(),
			Block = (Block) reader.ReadByte()
		};
	}
}