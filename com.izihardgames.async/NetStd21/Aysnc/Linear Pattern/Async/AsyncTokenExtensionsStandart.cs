using System;


namespace IziHardGames.Libs.NonEngine.Async
{
	public static class AsyncTokenExtensionsStandart
	{
		/// <summary>
		/// Version for passing not delegate but object with specific contract
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="asyncToken"></param>
		/// <returns></returns>
		public static AsyncToken AfterComplete<T>(this AsyncToken asyncToken, T item)
		{
			throw new NotImplementedException();
		}
	}
}