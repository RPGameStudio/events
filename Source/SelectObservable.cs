using System;
using System.Threading.Tasks;

namespace RX
{
    internal class SelectObservable<T, TR> : IObservable<TR>, IObserver<T>
    {
        private Func<T, TR> _selector;
        private IObservable<T> _observable;
        private IObserver<TR> _observer;

        public SelectObservable(IObservable<T> observable, Func<T, TR> selector)
        {
            _selector = selector;
            _observable = observable;
            _observer = null;
        }

        public IDisposable Subscribe(IObserver<TR> observer)
        {
            _observer = observer;
            _observable.Subscribe(this);

            return new DisposeToken
            {
                DisposeAction = async () =>
                {
                    await observer.OnCompleted();
                }
            };
        }

        async Task IObserver<T>.OnCompleted() => await _observer.OnCompleted();
        async Task IObserver<T>.OnError(Exception exception) => await _observer.OnError(exception);
        async Task IObserver<T>.OnNext(T next) => await _observer.OnNext(_selector.Invoke(next));
    }
}