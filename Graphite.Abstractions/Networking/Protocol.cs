using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Graphite.Abstractions.Networking.Packets;
using Graphite.Abstractions.Networking.Packets.Ingoing;

namespace Graphite.Abstractions.Networking;

public static class Protocol
{
    public static byte Version => 7;

    public static bool TryRead(ref ReadOnlySequence<byte> sequence, [NotNullWhen(true)] out IPacket? packet)
    {
        var sequenceReader = new SequenceReader<byte>(sequence);

        packet = null;

        if (!sequenceReader.TryRead(out var type))
        {
            return false;
        }

        var length = type switch
        {
            _ when type == PlayerIdentificationPacket.Type => PlayerIdentificationPacket.Length,
            _ when type == PositionOrientationPacket.Type => PositionOrientationPacket.Length,
            _ when type == BlockPlacementPacket.Type => BlockPlacementPacket.Length,
            _ when type == MessagePacket.Type => MessagePacket.Length,
            _ => -1
        };

        if (length is -1 || !sequenceReader.TryReadExact(length, out var buffer))
        {
            return false;
        }

        var spanReader = new SpanReader(buffer.IsSingleSegment ? buffer.FirstSpan : buffer.ToArray());

        packet = type switch
        {
            _ when type == PlayerIdentificationPacket.Type => PlayerIdentificationPacket.Create(spanReader),
            _ when type == PositionOrientationPacket.Type => PositionOrientationPacket.Create(spanReader),
            _ when type == BlockPlacementPacket.Type => BlockPlacementPacket.Create(spanReader),
            _ when type == MessagePacket.Type => MessagePacket.Create(spanReader),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid packet type.")
        };

        sequence = sequence.Slice(length + 1);

        return true;
    }

    public static void Write(Span<byte> buffer, IOutgoingPacket packet)
    {
        var writer = new SpanWriter(buffer);

        writer.WriteByte(packet.Type);
        packet.Write(ref writer);
    }
}