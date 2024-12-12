using System.Buffers.Binary;
using System.Text;

namespace Graphite.Abstractions.Networking;

public ref struct SpanWriter(Span<byte> source)
{
	private int position;

	private readonly Span<byte> source = source;

	public void Write(in ReadOnlySpan<byte> value)
	{
		value.CopyTo(source[position..(position += value.Length)]);
	}

	public void WriteByte(byte value)
	{
		source[position++] = value;
	}

	public void WriteShort(short value)
	{
		BinaryPrimitives.WriteInt16BigEndian(
			source[position..(position += sizeof(short))],
			value);
	}

	public void WriteFixedShort(float value)
	{
		BinaryPrimitives.WriteInt16BigEndian(
			source[position..(position += sizeof(short))],
			(short) (value * 32));
	}

	public void WriteString(string value)
	{
		Span<byte> buffer = stackalloc byte[64];

		// Pad it with spaces.
		buffer.Fill(32);

		var span = value.AsSpan();
		Encoding.UTF8.GetBytes(span.Length > 64 ? span[..64] : span, buffer);

		buffer.CopyTo(source[position..(position += 64)]);
	}
}