using Graphite.Abstractions.Worlds;
using System.Buffers.Binary;
using System.IO.Compression;

namespace Graphite.Worlds;

internal sealed class World(string name, short width, short height, short length) : IWorld
{
    public string Name => name;

    public short Width => width;

    public short Height => height;

    public short Length => length;

    public Block this[short x, short y, short z] => blocks[x + Width * (z + y * Length)];

    internal Block[] Blocks => blocks;

    private readonly Block[] blocks = new Block[width * height * length];

    public byte[][] Serialize()
    {
        using var result = new MemoryStream();
        using var compression = new GZipStream(result, CompressionMode.Compress);

        Span<byte> buffer = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(buffer, blocks.Length);

        compression.Write(buffer);
        compression.Write(blocks.Cast<byte>().ToArray());

        compression.Close();

        return result.ToArray().Chunk(1024).ToArray();
    }
}