namespace Graphite.Worlds;

/// <summary>
/// A <see cref="World"/> generator.
/// </summary>
public interface IGenerator
{
	public ValueTask GenerateAsync(World world);
}