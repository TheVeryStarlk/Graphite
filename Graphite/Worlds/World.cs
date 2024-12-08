using System.IO.Compression;

namespace Graphite.Worlds;

public sealed class World
{
	public string Name { get; }

	public short Width { get; }

	public short Height { get; }

	public short Length { get; }

	public Block this[short x, short y, short z]
	{
		get => blocks[x + Width * (z + y * Length)];
		private set => blocks[x + Width * (z + y * Length)] = value;
	}

	private readonly Block[] blocks;

	public World(string name, short width = 128, short height = 64, short length = 128)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

		Name = name;
		Width = width;
		Height = height;
		Length = length;

		blocks = new Block[width * height * length];
	}

	public byte[][] Serialize()
	{
		using var result = new MemoryStream();
		using var compression = new GZipStream(result, CompressionMode.Compress);

		compression.WriteInteger(blocks.Length);
		compression.Write(blocks.Cast<byte>().ToArray());

		compression.Close();

		return result.ToArray().Chunk(1024).ToArray();
	}
}