using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IziHardGames.Async.Contracts;
using TaskCompletionSource = System.Threading.Tasks.TaskCompletionSource<bool>;
namespace IziHardGames.Async.Tasks
{
    /// <summary>
    /// Поддерживает множественный вызов ожиданий подряд: например было вызвано первое ожидание и не дождавшись его заверешния вызывается следом второе ожидание
    /// </summary>
    public class AsyncAutoResetEvent : IAwaitComntrol, ITaskBased
    {
        private readonly Queue<TaskCompletionSource> waits = new Queue<TaskCompletionSource>();
        private int signals;
        private bool isCanceled;
        private CancellationToken? cancellationToken;
        private Task<bool>? cachedCanceled;

        private Exception? exception;
        private Exception? exception2;
        private static readonly InvalidOperationException invalidOperationException = new InvalidOperationException($"There is still waits in queue");

        public void Reset()
        {
            cachedCanceled = default;
            cancellationToken = default;

            isCanceled = false;

            Exception? exception = null;
            lock (waits)
            {
                if (waits.Count > 0 || signals > 0) exception = invalidOperationException;
            }
            signals = 0;
            if (exception != null) throw exception;
        }

        public Task WaitAsync(CancellationToken token = default)
        {
            try
            {
                if (token != default)   // regist uniq cancelation once
                {
                    if (cancellationToken == null)
                    {
                        cancellationToken = token;

                        token.Register(() =>
                        {
                            lock (waits)
                            {   // do not dequeue for consumers to consume awaits
                                isCanceled = true;
                                cachedCanceled = Task.FromCanceled<bool>(token);
                                foreach (var item in waits)
                                {
                                    item.SetCanceled();
                                }
                            }
                        });
                    }
                    if (cancellationToken != token) throw new NotSupportedException($"Tokens must be from one source");
                }

                lock (waits)
                {
                    Console.WriteLine($"ARE. wait. signals:{signals} waits:{waits.Count}");

                    if (signals > 0)
                    {
                        signals--;
                        return isCanceled ? cachedCanceled! : Task.FromResult(true);
                    }
                    else
                    {
                        var tcs = new TaskCompletionSource();
                        waits.Enqueue(tcs);
                        return tcs.Task;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                throw ex;
            }
        }
        public void Set()
        {
            lock (waits)
            {
                try
                {
                    Console.WriteLine($"ARE. Set. signals:{signals} waits:{waits.Count}");
                    if (isCanceled)
                    {
                        signals++; return;
                    }
                    if (waits.Count > 0)
                    {
                        TaskCompletionSource toRelease = waits.Dequeue();
                        toRelease.SetResult(true);
                    }
                    else
                    {
                        signals++;
                    }
                }
                catch (Exception ex)
                {
                    exception2 = ex;
                }
            }
        }
    }
}
