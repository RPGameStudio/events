using System;
using System.Threading.Tasks;

namespace RX
{
    internal class WhereObservable<T> : IObservable<T>, IObserver<T>
    {
        private Predicate<T> _predicate;
        private IObservable<T> _observable;
        private IObserver<T> _observer;

        public bool SkipLatestOnSubscribe => _observer.SkipLatestOnSubscribe;
        public int Priority => _observer.Priority;

        public WhereObservable(IObservable<T> observable, Predicate<T> predicate)
        {
            _observable = observable;
            _predicate = predicate;
            _observer = null;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observer = observer;
            var subscribtion = _observable.Subscribe(this);

            return new DisposeToken
            {
                DisposeAction = async () =>
                {
                    await observer.OnCompleted();
                    subscribtion.Dispose();
                }
            };
        }

        async Task IObserver<T>.OnCompleted() => await _observer.OnCompleted();
        async Task IObserver<T>.OnError(Exception exception) => await _observer.OnError(exception);
        async Task IObserver<T>.OnNext(T next)
        {
            if (_predicate(next))
                await _observer.OnNext(next);
        }
    }
}