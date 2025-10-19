using System.Threading.Tasks;
using System.Threading;
using System;
using IziHardGames.AsyncCommunication.Contracts;

namespace IziHardGames.AsyncCommunication.Promises
{
    public static class PromiseResolverGlobal<TSome, TError>
    {
        private static readonly PromiseResolver<TSome> resolverOfSomes = new PromiseResolver<TSome>();
        private static readonly PromiseResolver<TError> resolverOfErrors = new PromiseResolver<TError>();

        public static Task<object> Promise(Guid guid, TimeSpan timeout = default, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }

    public static class PromiseResolverGlobal<T>
    {
        private static readonly PromiseResolver<T> resolver = new PromiseResolver<T>();

        /// <inheritdoc cref="PromiseResolver{T}.Promise(Guid, TimeSpan, CancellationToken)"/>
        public static Task<T> Promise(Guid guid, TimeSpan timeout = default, CancellationToken ct = default)
        {
            return resolver.Promise(guid, timeout, ct);
        }

        public static Task<object> Promise<TError>(Guid guid, TimeSpan timeout = default, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="PromiseResolver{T}.Resolve(Guid, T)"/>
        public static bool Resolve(Guid guid, T result)
        {
            return resolver.Resolve(guid, result);
        }

        /// <inheritdoc cref="PromiseResolver{T}.Fail{TError}(TError)"/>
        public static void Fail<TError>(TError error) where TError : IFailedOperation
        {
            resolver.Fail(error);
        }
    }
}
