using System;


namespace IziHardGames.Libs.NonEngine.Async
{
	public class FactoryAsyncChain
	{
		public ProcessorForAsyncChain processorForAsync;

		public FactoryAsyncChain(ProcessorForAsyncChain processorForAsync)
		{
			this.processorForAsync = processorForAsync;
		}

		public AsyncToken CreateToken()
		{
			return processorForAsync.Acllocate();
		}
		public AsyncToken CreateToken(Action action)
		{
			return processorForAsync.Insert(action);
		}
	}
}