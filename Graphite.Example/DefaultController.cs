using Graphite.Abstractions;
using Graphite.Abstractions.Eventing.Sources.Listener;
using Graphite.Abstractions.Worlds;

namespace Graphite.Example;

internal sealed class DefaultController(IWorldContainer worldContainer) : Controller
{
	public override void Register(Registry registry)
	{
		registry.For<IListener>(subscriber =>
			subscriber.On<Starting>((_, _) =>
				worldContainer.Create("Default")));
	}
}