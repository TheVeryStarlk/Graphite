namespace Graphite.Abstractions.Networking.Packets.Outgoing;

public sealed class SpawnPlayerPacket : IOutgoingPacket
{
    public byte Type => 0x07;

    public int Length => 73;

    public required byte Identifier { get; init; }

    public required string Username { get; init; }

    public required float X { get; init; }

    public required float Y { get; init; }

    public required float Z { get; init; }

    public required byte Yaw { get; init; }

    public required byte Pitch { get; init; }

    public void Write(ref SpanWriter writer)
    {
        writer.WriteByte(Identifier);
        writer.WriteString(Username);
        writer.WriteFixedShort(X);
        writer.WriteFixedShort(Y);
        writer.WriteFixedShort(Z);
        writer.WriteByte(Yaw);
        writer.WriteByte(Pitch);
    }
}