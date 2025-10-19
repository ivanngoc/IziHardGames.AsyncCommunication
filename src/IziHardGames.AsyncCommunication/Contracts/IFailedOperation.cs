using IziHardGames.AsyncCommunication.Enums;

namespace IziHardGames.AsyncCommunication.Contracts
{
    public interface IFailedOperation : IOperationId, ICorrelationId
    {
        public EContentType ContentType { get; }
        public string? Content { get; }
    }
}
