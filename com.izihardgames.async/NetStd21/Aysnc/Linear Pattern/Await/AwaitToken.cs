using System;

namespace IziHardGames.Libs.NonEngine.Async.Await
{
	/// <summary>
	/// Token for creating sync point.
	/// Например когда 10 объектов достигнут единой точки то...выполнить метод. Этот токен укажет после каких процессов какой метод выполнить
	/// </summary>
	public readonly struct AwaitToken
	{
		public static ProcessorForAwaitToken? processor;
		public readonly int idAsIndex;

		public AwaitToken(int idAsIndex)
		{
			this.idAsIndex = idAsIndex;
		}

		public AwaitToken Append(Func<bool> func)
		{
			throw new NotImplementedException();
		}
		public void Callback()
		{
			processor!.AccomulateCallback(this);
		}
	}


	public class FactoryAwaitToken
	{
		public ProcessorForAwaitToken processorForAwaitToken;

		public FactoryAwaitToken(ProcessorForAwaitToken processorForAwaitToken)
		{
			this.processorForAwaitToken = processorForAwaitToken;
		}
		public AwaitToken Create(Action action, int callbackCount)
		{
			return processorForAwaitToken.Insert(action, callbackCount);
		}
		internal AwaitToken Create(Action action)
		{
			throw new NotImplementedException();
		}
	}

	public static class AwaitExe
	{
		public static FactoryAwaitToken? factoryAwaitToken;
		public static ProcessorForAwaitToken? processorForAwaitToken;

		public static AwaitToken WhenAll(Action action)
		{
			return factoryAwaitToken!.Create(action);
		}
		public static AwaitToken WhenAll(Action action, int callbackCount)
		{
			return factoryAwaitToken!.Create(action, callbackCount);
		}
	}
}