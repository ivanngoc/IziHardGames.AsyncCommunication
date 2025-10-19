using System.Threading.Tasks;

namespace IziHardGames.Async
{
    public struct AsyncOperation<T>
    {
        public int id;
        public T operation;
    }
}
