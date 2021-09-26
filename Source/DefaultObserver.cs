using System;
using System.Threading.Tasks;

namespace RX
{
    internal class DefaultObserver<T> : IObserver<T>
    {
        public Func<T, Task> OnNext;
        public Func<Exception, Task> OnError;
        public Func<Task> OnCompleted;

        public bool SkipLatestOnSubscribe { get; set; }
        public int Priority { get; set; }

        async Task IObserver<T>.OnCompleted()
        {
            if (OnCompleted != null)
                await OnCompleted();
        }

        async Task IObserver<T>.OnError(Exception exception)
        {
            if (OnError != null)
                await OnError(exception);
        }

        async Task IObserver<T>.OnNext(T next)
        {
            if (OnNext != null)
                await OnNext(next);
        }
    }
}