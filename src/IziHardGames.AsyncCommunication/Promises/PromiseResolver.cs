using System;
using System.Collections.Generic;
using IziHardGames.AsyncCommunication.Contracts;
using Registry = System.Collections.Generic.Dictionary<System.Type, System.Func<System.Guid, IziHardGames.AsyncCommunication.Contracts.IFailedOperation, bool>>;

namespace IziHardGames.AsyncCommunication.Promises
{
    internal class PromiseResolver
    {
        private readonly Registry queue = new Registry();
        public IEnumerable<Func<Guid, IFailedOperation, bool>> Queue => queue.Values;

        internal void Regist<T>(Func<Guid, IFailedOperation, bool> func)
        {
            queue.Add(typeof(T), func);
        }

        internal bool TryToSetFail<TTarget>(IFailedOperation error)
        {
            if (queue.TryGetValue(typeof(TTarget), out var handler))
            {
                return handler(error.OperationId, error);
            }
            return false;
        }
    }
}
