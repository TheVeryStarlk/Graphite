using Graphite.Abstractions;
using Graphite.Abstractions.Eventing;
using Graphite.Abstractions.Eventing.Sources.Listener;
using Graphite.Abstractions.Eventing.Sources.Player;
using Graphite.Abstractions.Worlds;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace Graphite.Example;

internal sealed class DefaultController(
	ILogger<DefaultController> logger,
	IWorldContainer worldContainer,
	IPlayerStore playerStore) : Controller
{
	public override void Register(Registry registry)
	{
		registry.For<IListener>(subscriber =>
			subscriber.On<Starting>((_, _) =>
				worldContainer.Create("Default")));

		registry.For<IPlayer>(subscriber =>
		{
			logger.LogInformation("Registering player events...");

			subscriber.On<Joining>((player, _) =>
			{
				if (playerStore.Players.ContainsKey(player.Name))
				{
					player.Kick("Duplicate name!");
				}
			});
		});
	}
}