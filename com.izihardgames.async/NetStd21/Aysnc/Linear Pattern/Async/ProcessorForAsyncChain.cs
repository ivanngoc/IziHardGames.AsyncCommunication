#pragma warning disable
using System;
using System.Collections.Generic;
//using ListChunk = IziHardGames.Libs.NonEngine.Collections.Chunks.ChunkedListT2<
//	IziHardGames.Libs.NonEngine.Async.AsyncToken,
//	IziHardGames.Libs.NonEngine.Async.DataAsyncExecution>;
//using Dictionary = IziHardGames.Libs.NonEngine.Collections.Dictionaries.WithOwnKey.DictionaryByIndexOnMemory<IziHardGames.Libs.NonEngine.Async.DataAsyncCallElement>;

namespace IziHardGames.Libs.NonEngine.Async
{
    /// <summary>
    /// Особенности:<br/>
    /// Текущая реализация <see cref="Execute"/> не позволяет выполнить повторный вызов одного и того же метода. После вызова элемент зануляется
    /// </summary>
    public class ProcessorForAsyncChain
    {
        internal IDictionary<int, DataAsyncCallElement> items;
        internal ProcessorForAsyncChain(IDictionary<int, DataAsyncCallElement> dic)
        {
            items = dic;
        }
        public ProcessorForAsyncChain()
        {
            //int capacity = 1;
            //items = new Dictionary(capacity);
            throw new NotImplementedException();
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
            //var span = items.Span;

            //for (int i = 0; i < items.Count; i++)
            //{
            //	if (span[i].isExecute)
            //	{
            //		span[i].action.Invoke();
            //		if (!span[i].TryToSetMoveNext())
            //		{
            //			items.RemoveByInternalKey(i);
            //			i--;
            //			continue;
            //		}
            //	}
            //	if (span[i].isMoveNext)
            //	{
            //		int externalKeyCurrent = items.GetExternalKey(i);
            //		int externalKeyNext = span[i].externalKeyNext;
            //		int externalKeyLast = span[i].externalKeyLast;

            //		items.MoveItemFromRightToLeftWithExternalsKey(externalKeyCurrent, externalKeyNext);

            //		ref DataAsyncCallElement next = ref items.GetRef(externalKeyCurrent);
            //		next.isExecute = true;
            //		next.externalKeyLast = externalKeyLast;
            //	}
            //}
        }

        public AsyncToken SetRun(AsyncToken asyncToken)
        {
            items[asyncToken.idAsIndex].SetExecute();
            return asyncToken;
        }
        public AsyncToken Acllocate()
        {
            throw new NotImplementedException();

            //return new AsyncToken(items.AddDefault());
        }
        public AsyncToken Insert(Action action)
        {
            throw new NotImplementedException();

            //ref DataAsyncCallElement data = ref items.AddDefault(out int key);
            //data.action = action;
            //data.isExecute = true;
            //data.externalKeyLast = key;
            //data.externalKeyNext = int.MinValue;
            //return new AsyncToken(key);
        }
        public void Append(AsyncToken asyncToken, Action action)
        {
            throw new NotImplementedException();

            //ref DataAsyncCallElement next = ref items.AddDefault(out int keyForNext);
            //next.action = action;
            //next.externalKeyNext = int.MinValue;

            //ref DataAsyncCallElement head = ref items.GetRef(asyncToken.idAsIndex);
            //head.externalKeyLast = keyForNext;

            //ref DataAsyncCallElement last = ref items.GetRef(head.externalKeyLast);
            //last.externalKeyNext = keyForNext;
        }
        /// <summary>
        /// Delete all elements after given element
        /// </summary>
        /// <param name="asyncToken"></param>
        public void Break(AsyncToken asyncToken)
        {
            throw new NotImplementedException();

            //ref DataAsyncCallElement head = ref items.GetRef(asyncToken.idAsIndex);
            //int keyCurrent = head.externalKeyNext;
            //head.externalKeyNext = int.MinValue;

            //while (keyCurrent >= 0)
            //{
            //    int keyNext = items.GetRef(keyCurrent).externalKeyNext;
            //    items.Remove(keyCurrent);
            //    keyCurrent = keyNext;
            //}
        }
    }

    /// <summary>
    /// Дата об элементе в цепочке вызовов.
    /// </summary>
    internal struct DataAsyncCallElement
    {
        public int externalKeyLast;
        public int externalKeyNext;

        public Action action;
        /// <summary>
        /// Is Proceed callchain forward? move to next <see cref="action"/> wich locate in <see cref="ProcessorForAsyncChain.items"/> with key=<see cref="externalKeyNext"/>
        /// </summary>
        public bool isMoveNext;
        /// <summary>
        /// Is run <see cref="action"/> in <see cref="ProcessorForAsyncChain.Execute"/>?
        /// </summary>
        public bool isExecute;

        public bool TryToSetMoveNext()
        {
            if (externalKeyNext < 0)
            {
                return false;
            }
            else
            {
                isMoveNext = true;
                return true;
            }
        }
        public void SetExecute()
        {
            isExecute = true;
        }
    }
}