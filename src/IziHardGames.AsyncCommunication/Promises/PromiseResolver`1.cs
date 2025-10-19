using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IziHardGames.AsyncCommunication.Contracts;

namespace IziHardGames.AsyncCommunication.Promises
{
    public class PromiseResolver<T>
    {
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<T>> promises = new ConcurrentDictionary<Guid, TaskCompletionSource<T>>();
        private readonly PromiseResolver resolver = new PromiseResolver();
        public PromiseResolver()
        {
            resolver.Regist<T>(TryToSetAsFailed);
        }

        /// <exception cref="TaskCanceledException">When timeouted or canceled</exception>
        public Task<T> Promise(Guid guid, TimeSpan timeout = default, CancellationToken ct = default)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            if (timeout != default)
            {
                cts.CancelAfter(timeout);
            }
            var promise = Promise(guid, cts.Token);
            return promise;
        }

        public Task<T> Promise(Guid guid, CancellationToken ct = default)
        {
            var source = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            promises.TryAdd(guid, source);
            if (ct != default)
            {
                ct.Register((x) =>
                {
                    if (!source.Task.IsCompleted)
                    {
                        source.SetCanceled();
                    }
                }, ct);
            }
            return source.Task;
        }

        public bool Resolve(Guid guid, T result)
        {
            if (promises.TryRemove(guid, out var source))
            {
                source.SetResult(result!);
                return true;
            }
            return false;
        }


        public void SetCanceled(Guid operationId)
        {
            if (promises.TryRemove(operationId, out var cts))
            {
                cts.SetCanceled();
            }
            else
            {
                throw new KeyNotFoundException(operationId.ToString());
            }
        }

        public bool TryToSetAsFailed(Guid operationId, IFailedOperation operation)
        {
            if (promises.TryGetValue(operationId, out var tcs))
            {
                tcs.SetException(new Exception("Task failed")
                {
                    Data = { [nameof(Exception.Data)] = operation }
                });
                return true;
            }
            return false;
        }

        public void Fail<TError>(TError error) where TError : IFailedOperation
        {
            // hotpath
            if (resolver.TryToSetFail<T>(error))
            {
                return;
            }
            foreach (var item in resolver.Queue)
            {
                if (item(error.OperationId, error))
                {
                    return;
                }
            }
        }
    }
}
