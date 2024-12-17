using Graphite.Abstractions;
using Graphite.Abstractions.Eventing;
using Graphite.Abstractions.Eventing.Sources.Listener;
using Graphite.Abstractions.Eventing.Sources.Player;
using Graphite.Abstractions.Worlds;

namespace Graphite.Web;

internal sealed class DefaultController(
    ILogger<DefaultController> logger,
    IWorldContainer worldContainer) : Controller
{
    public override void Register(Registry registry)
    {
        registry.For<IServer>(subscriber =>
            subscriber.On<Starting>((_, _) =>
                worldContainer.Create("Default")));

        registry.For<IPlayer>(subscriber =>
        {
            // logger.LogInformation("Registering player events...");

            subscriber.On<Joining>(async (player, _, _) =>
            {
                logger.LogInformation("Someone is joining");

                var world = worldContainer.Worlds["Default"];
                await player.SpawnAsync(world);
            });
        });
    }
}