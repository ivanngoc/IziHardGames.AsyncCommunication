using System.Threading;
using System.Threading.Tasks;

namespace IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign
{
    public interface IRouter
    {
        Task RouteAsync<TEvent>(TEvent e, CancellationToken ct = default);
    }
}
