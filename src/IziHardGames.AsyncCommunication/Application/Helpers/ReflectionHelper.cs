using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IziHardGames.AsyncCommunication.Application.Models;
using IziHardGames.AsyncCommunication.Contracts.EventDrivenDesign;

namespace IziHardGames.AsyncCommunication.Application.Helpers
{
    public static class ReflectionHelper
    {
        public static Eble GetConsumerMetasEnumerable(Assembly assembly)
        {
            return new Eble(assembly.GetTypes());
        }

        public static Eble GetConsumerMetasEnumerable(IEnumerable<Type> types)
        {
            return new Eble(types);
        }

        public static void ForeachConsumerMetas(IEnumerable<Type> types, Action<ConsumerMeta> action)
        {
            foreach (var meta in GetConsumerMetasEnumerable(types))
            {
                action(meta);
            }
        }

        public struct Eble
        {
            private IEnumerable<Type> types;

            public Eble(IEnumerable<Type> types)
            {
                this.types = types;
            }

            public Etor GetEnumerator()
            {
                return new Etor(types);
            }
        }

        public struct Etor
        {
            public ConsumerMeta Current => current ?? throw new InvalidOperationException();
            private ConsumerMeta? current;
            private readonly IEnumerable<(Type type, Type iface)> q;
            private readonly IEnumerator<(Type type, Type iface)> etor;

            public Etor(IEnumerable<Type> types)
            {
                var q1 = types.Select(x => (type: x, iface: x.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConsumer<>))));
                var q2 = q1.Where(x => x.iface is not null);
                this.q = q2.Select(x => (type: x.type, iface: x.iface!));
                this.etor = q.GetEnumerator();
            }

            public bool MoveNext()
            {
                if (etor.MoveNext())
                {
                    var val = etor.Current;
                    var eType = val.iface.GetGenericArguments()[0];
                    current = new ConsumerMeta(val.type, eType, val.iface);
                    return true;
                }
                return false;
            }
        }
    }
}
