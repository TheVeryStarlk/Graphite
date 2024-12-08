namespace Graphite.Worlds;

public sealed class EmptyGenerator : IGenerator
{
	public ValueTask GenerateAsync(World world)
	{
		return ValueTask.CompletedTask;
	}
}