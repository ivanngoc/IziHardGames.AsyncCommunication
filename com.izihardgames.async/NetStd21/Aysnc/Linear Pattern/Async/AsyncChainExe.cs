using System;


namespace IziHardGames.Libs.NonEngine.Async
{

	/// <summary>
	/// API для выполнения методов последовательно но в разных кадрах
	/// </summary>
	public static class AsyncChainExe
	{
		public static FactoryAsyncChain? factoryAsyncChain;
		public static ProcessorForAsyncChain? processorForAsyncChain;

		/// <summary>
		/// Run 
		/// </summary>
		/// <param name="actionRun"></param>
		/// <returns></returns>
		public static AsyncToken Begin(Action actionRun)
		{
			actionRun();
			return factoryAsyncChain!.CreateToken();
		}
		/// <summary>
		/// Delayed execution to next itteration of <see cref="ProcessorForAsyncChain"/>
		/// </summary>
		/// <param name="actionRun"></param>
		/// <returns></returns>
		public static AsyncToken BeginAsync(Action actionRun)
		{
			return factoryAsyncChain!.CreateToken(actionRun);
		}
		public static AsyncToken BeginAsync(Action<AsyncToken> actionWriteToken, Action action)
		{
			var token = Begin(action);
			actionWriteToken(token);
			return token;
		}
	}
}