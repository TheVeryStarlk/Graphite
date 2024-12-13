namespace Graphite.Abstractions.Networking.Packets.Outgoing;

public sealed class DisconnectPacket : IOutgoingPacket
{
    public byte Type => 0x0E;

    public int Length => 64;

    public required string Reason { get; init; }

    public void Write(ref SpanWriter writer)
    {
        writer.WriteString(Reason);
    }
}