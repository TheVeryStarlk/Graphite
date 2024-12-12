using Graphite.Abstractions.Worlds;
using Microsoft.Extensions.Logging;

namespace Graphite.Worlds;

internal sealed class WorldContainer(ILogger<WorldContainer> logger) : IWorldContainer
{
	public IReadOnlyDictionary<string, IWorld> Worlds => worlds;

	private readonly Dictionary<string, IWorld> worlds = [];

	public void Create(string name)
	{
		if (worlds.ContainsKey(name))
		{
			throw new InvalidOperationException("World already exists.");
		}

		logger.LogInformation("Created world: \"{Name}\"", name);
	}
}