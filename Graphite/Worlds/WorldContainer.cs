using Graphite.Abstractions.Worlds;
using Microsoft.Extensions.Logging;

namespace Graphite.Worlds;

internal sealed class WorldContainer(ILogger<WorldContainer> logger) : IWorldContainer
{
	public IReadOnlyDictionary<string, IWorld> Worlds => worlds;

	private readonly Dictionary<string, IWorld> worlds = [];

	public void Create(string name, short width, short height, short length)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

		if (worlds.ContainsKey(name))
		{
			throw new InvalidOperationException("World already exists.");
		}

		var world = new World(name, width, height, length);
		worlds[name] = world;

		logger.LogInformation("Created world: \"{Name}\"", name);
	}
}