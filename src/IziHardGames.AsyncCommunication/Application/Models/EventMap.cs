using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign;
using static IziHardGames.AsyncCommunication.Application.Models.EventsMap;
using F = System.Func<object, System.Threading.Tasks.Task>;

namespace IziHardGames.AsyncCommunication.Application.Models
{
    using System;

    public class EventsMap : Dictionary<Type, ConsumptionGroup>
    {
        public EventsMap(IEnumerable<ConsumerMetaT1> enumerable, IEnumerable<KeyValuePair<Type, ConsumptionGroup>> pairs) : base(pairs.ToDictionary(x => x.Key, x => x.Value))
        {

        }

        public class ConsumptionGroup
        {
            public Type EventType { get; }
            public ConsumerMetaT1[] Metas { get; }
            internal Item[] Items { get; set; }

            public ConsumptionGroup(Type eventType, ConsumerMetaT1[] metas, Item[] items)
            {
                EventType = eventType;
                Metas = metas;
                Items = items;
            }

            public async Task ConsumeAsync(object e)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    await Items[i].Func(e).ConfigureAwait(false);
                }
            }

            public static Item CreateFunc(ConsumerMetaT1 consumer, Func<object> getTargetFunc)
            {
                var paramE = Expression.Parameter(typeof(object), "e");
                var paramFunc = Expression.Parameter(typeof(F), "arg");
                var method = typeof(ConsumptionGroup).GetMethod(nameof(ConsumeGenAsync), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var genericMethod = method!.MakeGenericMethod(consumer.meta.ActulaType, consumer.meta.EventType);
                var call = Expression.Call(genericMethod, paramE, Expression.Constant(getTargetFunc));
                var lambda = Expression.Lambda<F>(call, paramE);
                var f = lambda.Compile();
                return new Item(f, getTargetFunc);
            }

            internal static Task ConsumeGenAsync<TTarget, TEvent>(object e, Func<object> getTargetFunc) where TTarget : IConsumer<TEvent>
            {
                if (getTargetFunc() is TTarget target)
                {
                    return target.ConsumeAsync((TEvent)e);
                }
                return Task.CompletedTask;
            }

            public class Item
            {
                public F Func { get; }
                public Lazy<object> Target { get; }
                public Item(F func, Func<object> getTarget)
                {
                    Func = func;
                    Target = new Lazy<object>(getTarget);
                }
            }
        }
    }
}