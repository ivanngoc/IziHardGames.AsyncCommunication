namespace IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign
{
    public interface IConsumerTrackable<TEvent> : IConsumer<TEvent>
        where TEvent : IEventTrackable
    {

    }
}
