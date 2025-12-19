using System;
using System.Runtime.CompilerServices;
using System.Threading;
using IziHardGames.Async.Attributes;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace IziHardGames
{
    public static class ExtensionsForAsyncOperation
	{
		public static AdapterForAsyncOperation Adapt(this AsyncOperation operation)
		{
			return new AdapterForAsyncOperation();
		}
	}
	[GetAwaiter]
	public static class UnityAwaiting
	{
		/// System.Threading.Tasks.Sources.ManualResetValueTaskSourceCore
		public static IziTaskForAsyncOperation AwaitIndependently(AsyncOperation operation, CancellationToken token = default)
		{
			return new IziTaskForAsyncOperation(operation, token);
		}
		public static IziAwaiterForAsyncOperation GetAwaiter(AsyncOperation operation)
		{
			return new IziAwaiterForAsyncOperation(operation);
		}
	}
	public readonly struct IziTaskForAsyncOperation
	{
		private readonly AsyncOperation asyncOperation;
		public IziTaskForAsyncOperation(AsyncOperation asyncOperation, CancellationToken token = default)
		{
			this.asyncOperation = asyncOperation;
		}
		public IziAwaiterForAsyncOperation GetAwaiter()
		{
			return new IziAwaiterForAsyncOperation(asyncOperation);
		}
	}
    /// <summary>
    /// создает много мусора при создании <see cref="action"/>
    /// </summary>
    public struct IziAwaiterForAsyncOperation : INotifyCompletion
	{
		private AsyncOperation operation;
		private Action<AsyncOperation>? action;
		public bool IsCompleted => operation.isDone;
		public IziAwaiterForAsyncOperation(AsyncOperation asyncOperation) : this()
		{
            operation = asyncOperation;
		}
		public void OnCompleted(Action continuation)
		{
            action = (x) => continuation();
			operation.completed += action;
		}
		public void GetResult()
		{
			if (action != null)
				operation.completed -= action;
		}
	}

	public struct AdapterForAsyncOperation 
	{

	}
}