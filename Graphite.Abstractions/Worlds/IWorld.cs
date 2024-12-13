namespace Graphite.Abstractions.Worlds;

public interface IWorld
{
    public string Name { get; }

    public short Width { get; }

    public short Height { get; }

    public short Length { get; }

    public Block this[short x, short y, short z] { get; }
}