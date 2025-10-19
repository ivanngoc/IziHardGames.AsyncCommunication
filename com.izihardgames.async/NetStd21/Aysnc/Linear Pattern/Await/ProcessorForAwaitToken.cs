using System;
using System.Collections.Generic;
//using Dictionary = IziHardGames.Libs.NonEngine.Collections.Dictionaries.WithOwnKey.DictionaryByIndexOnMemory<IziHardGames.Libs.NonEngine.Async.Await.DataForAwaitProcess>;

namespace IziHardGames.Libs.NonEngine.Async.Await
{
	public class ProcessorForAwaitToken
	{
		internal IDictionary<int, DataForAwaitProcess> items;
        internal ProcessorForAwaitToken(IDictionary<int, DataForAwaitProcess> items)
		{
			this.items = items;
		}
		public ProcessorForAwaitToken()
		{
			//int capacity = 1;
			//items = new Dictionary(capacity);
			throw new System.NotImplementedException();
		}
		public void Execute()
		{
            throw new System.NotImplementedException();

   //         var span = items.Span;

			//for (int i = 0; i < items.Count; i++)
			//{
			//	if (span[i].isAwaitCompleted)
			//	{
			//		span[i].action.Invoke();
			//		items.RemoveByInternalKeyWithSetDefault(i);
			//		i--;
			//	}
			//}
		}

		internal void AccomulateCallback(AwaitToken awaitToken)
		{
            throw new System.NotImplementedException();

   //         ref var data = ref items.GetRef(awaitToken.idAsIndex);
			//data.Increment();
		}

		public AwaitToken Insert(Action action, int callbackCount)
		{
            throw new System.NotImplementedException();

            //return new AwaitToken(items.Add(new DataForAwaitProcess(action, callbackCount)));
		}
	}

	internal struct DataForAwaitProcess
	{
		public bool isAwaitCompleted;
		public int countTotal;
		public int countCurrent;
		public Action action;

		public DataForAwaitProcess(Action action, int callbackCount) : this()
		{
			this.action = action;
			countTotal = callbackCount;
		}

		internal void Increment()
		{
			countCurrent++;
			isAwaitCompleted = countTotal == countCurrent;
#if UNITY_EDITOR
			if (countCurrent > countTotal) throw new OverflowException($"Количество колбэков превышает максимальное");
#endif
		}
	}
}