namespace Graphite.Abstractions.Networking.Packets;

public sealed class PositionOrientationPacket : IIngoingPacket<PositionOrientationPacket>, IOutgoingPacket
{
    public static byte Type => 0x08;

    public static int Length => 9;

    byte IOutgoingPacket.Type => 0x08;

    int IOutgoingPacket.Length => Length;

    public required byte Identifier { get; init; }

    public required float X { get; init; }

    public required float Y { get; init; }

    public required float Z { get; init; }

    public required byte Yaw { get; init; }

    public required byte Pitch { get; init; }

    public static PositionOrientationPacket Create(SpanReader reader)
    {
        return new PositionOrientationPacket
        {
            Identifier = reader.ReadByte(),
            X = reader.ReadFixedShort(),
            Y = reader.ReadFixedShort(),
            Z = reader.ReadFixedShort(),
            Yaw = reader.ReadByte(),
            Pitch = reader.ReadByte()
        };
    }

    public void Write(ref SpanWriter writer)
    {
        writer.WriteByte(Identifier);
        writer.WriteFixedShort(X);
        writer.WriteFixedShort(Y);
        writer.WriteFixedShort(Z);
        writer.WriteByte(Yaw);
        writer.WriteByte(Pitch);
    }
}