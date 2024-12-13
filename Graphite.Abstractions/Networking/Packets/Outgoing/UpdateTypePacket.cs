namespace Graphite.Abstractions.Networking.Packets.Outgoing;

public sealed class UpdateTypePacket : IOutgoingPacket
{
    public byte Type => 0x0F;

    public int Length => 1;

    public required byte IsOperator { get; init; }

    public void Write(ref SpanWriter writer)
    {
        writer.WriteByte(IsOperator);
    }
}