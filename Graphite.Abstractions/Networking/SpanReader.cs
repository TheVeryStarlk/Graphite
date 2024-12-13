using System.Buffers.Binary;
using System.Text;

namespace Graphite.Abstractions.Networking;

public ref struct SpanReader(ReadOnlySpan<byte> source)
{
    private int position;

    private readonly ReadOnlySpan<byte> source = source;

    public void Read(Span<byte> value)
    {
        source[position..(position += value.Length)].CopyTo(value);
    }

    public byte ReadByte()
    {
        return source[position++];
    }

    public short ReadShort()
    {
        return BinaryPrimitives.ReadInt16BigEndian(
            source[position..(position += sizeof(short))]);
    }

    public float ReadFixedShort()
    {
        return BinaryPrimitives.ReadInt16BigEndian(
            source[position..(position += sizeof(short))]) / 32F;
    }

    public int ReadInteger()
    {
        return BinaryPrimitives.ReadInt32BigEndian(
            source[position..(position += sizeof(int))]);
    }

    public string ReadString()
    {
        return Encoding.UTF8
            .GetString(source[position..(position += 64)])
            .TrimEnd();
    }
}