using System;
using System.Threading.Tasks;

namespace RX
{
    internal abstract class ObservableExpression<TSource, TResult> : IObservable<TResult>, IObserver<TSource>
    {
        protected IObservable<TSource> _observable;
        protected IObserver<TResult> _observer;

        public bool SkipLatestOnSubscribe => _observer.SkipLatestOnSubscribe;
        public int Priority => _observer.Priority;

        public ObservableExpression(IObservable<TSource> observable)
        {
            _observable = observable;
        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            _observer = observer;
            var subscribtion = _observable.Subscribe(this);

            return new DisposeToken
            {
                DisposeAction = async () => subscribtion.Dispose()
            };
        }

        async Task IObserver<TSource>.OnCompleted() => await _observer.OnCompleted();
        async Task IObserver<TSource>.OnError(Exception exception) => await _observer.OnError(exception);
        public abstract Task OnNext(TSource value);
    }
}