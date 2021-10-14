using System;
using System.Threading.Tasks;

namespace RX
{
    internal class WhereObservable<T> : ObservableExpression<T, T>
    {
        private Predicate<T> _predicate;

        public WhereObservable(IObservable<T> observable, Predicate<T> predicate) : base(observable)
        {
            _predicate = predicate;
        }

        public override async Task OnNext(T value)
        {
            if (_predicate(value))
                await _observer.OnNext(value);
        }
    }
}