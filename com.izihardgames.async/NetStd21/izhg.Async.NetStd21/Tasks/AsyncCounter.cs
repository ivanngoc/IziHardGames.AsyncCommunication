using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using IziHardGames.Async.Contracts;
using IziHardGames.Async.ValueTasks;
using IziHardGames.Pools.Contracts;

namespace IziHardGames.Async.Tasks
{
    /// <summary>
    /// ValueTask based. First you set value to await, then you regist continuations. 
    /// Each time continuation fired there woulb be increment or decrement. After current counter reach target value await will be finished.
    /// 
    /// </summary>
    public sealed class AsyncCounter : IValueTaskSource, IDisposable, IPoolable<AsyncCounter>, IValueTaskBased, IAwaitComntrol, ITaskBased
    {
        private ManualResetValueTaskSourceCore<bool> cts;
        public IPool<AsyncCounter> Pool { get; private set; } = null!;
        private bool isRunning;
        private int target;
        private int counter;
        private readonly Action<Task, object?> actionContinueDecrement;
        private readonly Action actionSetException;

        public static OperationCanceledException Exception => AsyncSignaler.exception;

        public AsyncCounter()
        {
            actionSetException = SetException;
            actionContinueDecrement = Decrement;
        }

        private void Decrement(Task arg1, object? arg2)
        {
            lock (this)
            {
                counter--;
                if (counter == target)
                {
                    FinishAwaiting();
                }
            }
        }

        private void SetException()
        {
            cts.SetException(Exception);
            cts.Reset();
        }

        public void Init(int count)
        {
            if (isRunning) throw new InvalidOperationException("Counter is already running");
            target = count;
            isRunning = true;
        }

        public void Decrement(Task task)
        {
            task.ContinueWith(actionContinueDecrement, this);
        }
        public void Decrement(ValueTask task)
        {
            throw new NotImplementedException();
        }
        public void Decrement<T>(ValueTask<T> task)
        {
            throw new NotImplementedException();
        }

        public void GetResult(short token)
        {
            cts.GetResult(token);
        }

        public ValueTaskSourceStatus GetStatus(short token)
        {
            return cts.GetStatus(token);
        }

        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            cts.OnCompleted(continuation, state, token, flags);
        }

        public void Dispose()
        {
            Pool!.Return(this);
            if (isRunning) throw new InvalidOperationException("All Awaiting must be finished");
            isRunning = false;
        }

        public void ReturnTo(IPoolReturn<AsyncCounter> pool)
        {
            Pool = pool as IPool<AsyncCounter> ?? throw new NotImplementedException();
        }

        public static AsyncCounter Rent()
        {
            var pool = IPool<AsyncCounter>.Shared!;
            var rent = pool!.Rent();
            rent.ReturnTo(pool);
            return rent;
        }

        public ValueTask Await(CancellationToken ct = default)
        {
            if (ct != default) ct.Register(actionSetException);
            return new ValueTask(this, cts.Version);
        }

        private void FinishAwaiting()
        {
            isRunning = false;
            cts.SetResult(true);
            cts.Reset();
        }
    }
}
