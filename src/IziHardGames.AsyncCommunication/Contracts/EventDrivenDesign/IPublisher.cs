using System.Threading;
using System.Threading.Tasks;

namespace IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign
{
    public interface IPublisher
    {
        Task PublishAsync<TMsg>(TMsg msg, CancellationToken ct = default);
    }
}
