using Microsoft.Extensions.Logging;

namespace Graphite.Worlds;

public sealed class WorldContainer(ILogger<WorldContainer> logger, Server server)
{
	public IReadOnlyDictionary<string, World> Worlds => worlds;

	private readonly Dictionary<string, World> worlds = [];

	public async ValueTask CreateAsync(
		string name,
		IGenerator generator,
		short width = 128,
		short height = 64,
		short length = 128)
	{
		if (worlds.ContainsKey(name))
		{
			throw new InvalidOperationException("World is already registered.");
		}

		var world = new World(server, name, width, height, length);

		await generator.GenerateAsync(world).ConfigureAwait(false);

		worlds[name] = world;

		logger.LogInformation("Created world: \"{Name}\"", name);
	}

	public void Delete(string name)
	{
		worlds.Remove(name);
	}
}