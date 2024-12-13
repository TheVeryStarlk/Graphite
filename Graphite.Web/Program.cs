using System.Net.WebSockets;
using Graphite.Hosting;
using Graphite.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphite<DefaultController>();

var application = builder.Build();

application.UseHttpsRedirection();

application.MapGet("/ws", async (HttpContext context) =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        return Results.BadRequest();
    }

    using var socket = await context.WebSockets.AcceptWebSocketAsync();

    await socket.CloseAsync(
        WebSocketCloseStatus.NormalClosure,
        string.Empty,
        CancellationToken.None);

    return Results.Ok();
});

application.Run();