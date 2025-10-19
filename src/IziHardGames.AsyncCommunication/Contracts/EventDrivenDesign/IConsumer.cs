using System.Threading.Tasks;

namespace IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign
{
    public interface IConsumer
    {

    }

    public interface IConsumer<TEvent>: IConsumer
    {
        Task ConsumeAsync(TEvent e);
    }
}
