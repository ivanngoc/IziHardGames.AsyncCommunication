using System;
using System.Threading.Tasks;


namespace IziHardGames.Libs.NonEngine.Async
{

	/// <summary>
	/// Promise-object.
	/// Token который предоставляет методы для выстраивания последовательности из методов.
	/// Node-like code autoring provider.
	/// По сути является контейнером для ключа-индекса <see cref="idAsIndex"/> в <see cref="processor"/>. 
	/// Все найстройки и служубную информацию содержит <see cref="processor"/> а этот объект лишь указывает в каком элементе массива хранится информация о нем.
	/// </summary>
	public readonly struct AsyncToken
	{
		public static ProcessorForAsyncChain? processor;
		public readonly int idAsIndex;

		public AsyncToken(int idAsIndex)
		{
			this.idAsIndex = idAsIndex;
		}

		public AsyncToken RunSync(Action action)
		{
			action();
			throw new NotImplementedException();
		}

		public AsyncToken Next(Action action)
		{
			processor!.Append(this, action);
			return this;
		}
		public AsyncToken AfterComplete(Task task)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Repeat previous method while
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public AsyncToken While(Func<bool> func)
		{
			throw new NotImplementedException();
		}

		public static AsyncToken Error()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Operation was aborted. All changes is reverted
		/// </summary>
		/// <returns></returns>
		public static AsyncToken Cancel()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// In case like [bool TryGet(out TResult)] if condition not satisfied than return token with allocated in <see cref="processor"/> element 
		/// for further execution with certain branch (complete/error/finish/cancel and etc)
		/// </summary>
		/// <returns></returns>
		public static AsyncToken ConditionNotSatisfied()
		{
			return default;
		}

		/// <summary>
		/// Repeat previous method until
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public AsyncToken Until(Func<bool> func)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Remove all callings that left Or until condition sepcified by <see cref="CaseBreak"/>
		/// </summary>
		/// <returns></returns>
		public void Break()
		{
			processor!.Break(this);
		}

		public AsyncToken CaseBreak(Action action)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Run action when condition satisfied (becomes true)
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public AsyncToken When(Func<bool> condition, Action action)
		{
			throw new NotImplementedException();
		}

		public AsyncToken Branch(Func<bool> func, Action actionCaseTrue)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// If case
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public AsyncToken Branch(Func<bool> func, Action actionCaseTrue, Action actionCaseFalse)
		{
			throw new NotImplementedException();
		}

		public AsyncToken AfterSeconds(float seconds, Action action)
		{
			throw new NotImplementedException();
		}
		public AsyncToken AfterFrames(int frameCount, Action action)
		{
			throw new NotImplementedException();
		}

		public AsyncToken Wait()
		{
			throw new NotImplementedException();
		}
#if DEBUG
		public static void Test()
		{
			//Task? task = default;
			//task.IsCompleted
		}
#endif
	}
}