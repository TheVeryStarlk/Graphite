using System.Buffers.Binary;

namespace Graphite.Worlds;

internal static class StreamExtensions
{
	public static void WriteInteger(this Stream stream, int value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(int)];
		BinaryPrimitives.WriteInt32BigEndian(buffer, value);
		stream.Write(buffer);
	}
}