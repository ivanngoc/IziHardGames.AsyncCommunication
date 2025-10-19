using System;
using System.Threading;
using System.Threading.Tasks;
using IziHardGames.Async.Contracts;

namespace IziHardGames.Async.Tasks
{
    /// <summary>
    /// Задача - Дождаться значения в счетчике. Когда значение достигает необходимого, то await завершается
    /// </summary>
    public class AsyncAutoResetCounter : IDisposable, ITaskBased, IAwaitComntrol
    {
        public int counter;
        private int value;
        private TaskCompletionSource<bool>? tcs;

        public AsyncAutoResetCounter()
        {
            InitReset();
        }

        public void InitReset()
        {
            tcs = new TaskCompletionSource<bool>();
            counter = 0;
            value = -1;
        }
        public void Increment()
        {
            Interlocked.Increment(ref counter);
            if (counter == value)
            {
                tcs!.SetResult(true);
                InitReset();
            }
        }
        public void Decrement()
        {
            Interlocked.Decrement(ref counter);
            if (counter == value)
            {
                tcs!.SetResult(true);
                InitReset();
            }
        }
        public Task Await(int val, CancellationToken token = default)
        {
            if (token != default)
                token.Register(() =>
                {
                    tcs!.SetCanceled();
                });

            if (counter == val) return Task.CompletedTask;
            value = val;
            return tcs!.Task;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
