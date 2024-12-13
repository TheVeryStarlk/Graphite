using Graphite.Abstractions;
using Graphite.Abstractions.Eventing;
using Graphite.Abstractions.Eventing.Sources.Listener;
using Graphite.Abstractions.Eventing.Sources.Player;
using Graphite.Abstractions.Worlds;

namespace Graphite.Web;

internal sealed class DefaultController(
	ILogger<DefaultController> logger,
	IWorldContainer worldContainer,
	IPlayerStore playerStore) : Controller
{
	public override void Register(Registry registry)
	{
		registry.For<IListener>(subscriber =>
			subscriber.On<Starting>(_ =>
				worldContainer.Create("Default")));

		registry.For<IPlayer>(subscriber =>
		{
			logger.LogInformation("Registering player events...");

			subscriber.On<Joining>(joining =>
			{
				logger.LogInformation("Someone is joining");

				var count = playerStore.Players.Keys.Count(key => key == joining.Username);

				if (count > 1)
				{
					logger.LogWarning("Duplicate found!");
				}
			});
		});
	}
}