using System.Threading;
using System.Threading.Tasks;
using IziHardGames.AsyncCommunication.Application.Models;
using IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign;

namespace IziHardGames.AsyncCommunication.Application.EventDrivenDesign
{
    public class Router(EventsMap eventMap) : IRouter
    {
        public Task RouteAsync<TEvent>(TEvent e, CancellationToken ct = default)
        {
            var t = typeof(TEvent);
            if (eventMap.TryGetValue(t, out var grp))
            {
                var faf = grp.ConsumeAsync(e!);
            }
            return Task.CompletedTask;
        }
    }
}
