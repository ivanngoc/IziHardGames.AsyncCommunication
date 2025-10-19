using IziHardGames.AsyncCommunication.Application.Helpers;
using IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign;
using IziHardGames.AsyncCommunication.Promises;

namespace IziHardGames.AsyncCommunication.UnitTests
{
    public class PromiseResolver_UnitTest
    {
        [Fact]
        public async Task Test1()
        {
            var resolver = new PromiseResolver<int>();
            var guid = Guid.Parse("8ff01131-15c2-4280-859d-56b97a38cb5c");
            var promise = Task.Run(async () => await resolver.Promise(guid, TimeSpan.FromSeconds(1), default));
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await promise);
        }

        [Fact]
        public async Task Linked_Cancelation_Not_Affect_Completed_Task()
        {
            var resolver = new PromiseResolver<int>();
            var guid = Guid.Parse("8ff01131-15c2-4280-859d-56b97a38cb5c");
            var cts = new CancellationTokenSource();
            var promise = resolver.Promise(guid, default, cts.Token);
            var resolved = resolver.Resolve(guid, 100);
            Assert.True(resolved);
            var result = await promise;
            Assert.Equal(100, result);
            await cts.CancelAsync();
        }

        [Fact]
        public void Should_FindTypes()
        {
            var etor = ReflectionHelper.GetConsumerMetasEnumerable(typeof(ConsumerStr).Assembly);
            var count = 0;
            foreach (var item in etor)
            {
                count++;
                if (item.ActulaType == typeof(ConsumerStr))
                {
                    Assert.Equal(typeof(string), item.EventType);
                }
                else if (item.ActulaType == typeof(ConsumerInt))
                {
                    Assert.Equal(typeof(int), item.EventType);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            Assert.Equal(2, count);
        }

        private class ConsumerStr : IConsumer<string>
        {
            public async Task ConsumeAsync(string e)
            {
                await Task.CompletedTask;
            }
        }

        private class ConsumerInt : IConsumer<int>
        {
            public async Task ConsumeAsync(int e)
            {
                await Task.CompletedTask;
            }
        }
    }
}
