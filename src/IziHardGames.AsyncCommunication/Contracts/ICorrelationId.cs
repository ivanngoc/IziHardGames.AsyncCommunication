using System;

namespace IziHardGames.AsyncCommunication.Contracts
{
    public interface ICorrelationId
    {
        Guid? CorrelationId { get; }
    }
}
