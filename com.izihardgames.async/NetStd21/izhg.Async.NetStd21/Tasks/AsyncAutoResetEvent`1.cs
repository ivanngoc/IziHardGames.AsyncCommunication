using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IziHardGames.Async.Contracts;

namespace IziHardGames.Async.Tasks
{
    public class AsyncAutoResetEvent<T> : IAwaitComntrol, ITaskBased
    {
        private static readonly InvalidOperationException exIncompleted = new InvalidOperationException($"Event is not completed. You must call Complete() explicitly to notify about ending");
        private static readonly InvalidOperationException exCompleted = new InvalidOperationException($"Event is completed. Use Reset to be able await again");
        private static readonly InvalidOperationException exInvalidOperation = new InvalidOperationException($"There is still waits in queue");
        private static readonly NotSupportedException exNotSupported = new NotSupportedException($"Tokens must be from one source");

        private readonly Queue<TaskCompletionSource<T>> waits = new Queue<TaskCompletionSource<T>>();
        private bool isCanceled;
        /// <summary>
        /// <see langword="true"/> - there won't be additional waits. After last wait is consumed Reset() must be called
        /// </summary>
        private bool isCompleted;
        private CancellationToken? cancellationToken;
        public int Count => waits.Count;
        public bool IsComplete => isCompleted;
        public void Reset()
        {
            if (!isCompleted) throw exIncompleted;
            isCompleted = false;
            cancellationToken = default;
            isCanceled = false;
            isCompleted = false;
            Exception? exception = null;
            lock (waits)
            {
                if (waits.Count > 0) exception = exInvalidOperation;
            }
            if (exception != null) throw exception;
        }

        public Task<T> WaitAsync(CancellationToken token = default)
        {
#if DEBUG
            //Console.WriteLine($"Wait");
#endif
            lock (waits)
            {
                if (token != default)
                {
                    if (cancellationToken == null)
                    {
                        cancellationToken = token;
                        token.Register(Cancel);
                    }
                    if (cancellationToken != token) throw exNotSupported;
                }

                //Console.WriteLine($"ARE. wait. waits:{waits.Count}");
                if (waits.Count > 0)
                {
                    return waits.Dequeue().Task;
                }
                else
                {
                    var tcs = new TaskCompletionSource<T>();
                    if (isCanceled) { tcs.SetCanceled(); return tcs.Task; }
                    waits.Enqueue(tcs);
                    return tcs.Task;
                }
            }

        }

        public void Cancel()
        {
            lock (waits)
            {   // do not dequeue for consumers to consume awaits
                isCanceled = true;
                foreach (var item in waits)
                {
                    item.SetCanceled();
                }
            }
        }

        /// <summary>
        /// Call Complete() and Set. In that order its guaranted that awaiter on set result would gain IsCompleted=true in check at any time.
        /// Возможна ситуация когда TaskCompletionSource.SetResult(result) вызовет выполнение awaiter'а который выполнится раньше чем завершится Set(value). 
        /// Поэтому сперва нужно выставить IsComplete=true а потом вызвать Set(value)
        /// </summary>
        /// <param name="value"></param>
        public void SetLast(T value)
        {
            Complete();
            SetUnsafe(value);
        }

        public void Set(T value, bool isComplete)
        {
            Set(value);
            if (isComplete) Complete();
        }
        private void SetUnsafe(T value)
        {
#if DEBUG
            //Console.WriteLine($"Set");
#endif
            lock (waits)
            {
                //Console.WriteLine($"ARE. Set. waits:{waits.Count}");
                if (waits.Count > 0)
                {
                    TaskCompletionSource<T> toRelease = waits.Dequeue();
                    if (isCanceled)
                    {
                        toRelease.SetCanceled();
                        return;
                    }
                    toRelease.SetResult(value);
                }
                else
                {
                    var res = new TaskCompletionSource<T>();
                    if (isCanceled)
                    {
                        res.SetCanceled();
                    }
                    else
                    {
                        res.SetResult(value);
                    }
                    waits.Enqueue(res);
                }
            }
        }
        public void Set(T value)
        {
            if (isCompleted) throw exCompleted;
            SetUnsafe(value);
        }

        public void Complete()
        {
            if (isCompleted) throw exCompleted;
            isCompleted = true;
        }
    }
}
