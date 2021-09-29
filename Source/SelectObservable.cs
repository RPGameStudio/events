using System;
using System.Collections;
using System.Threading.Tasks;

namespace RX
{
    internal class SelectObservable<TSource, TResult> : ObservableExpression<TSource, TResult>
    {
        private Func<TSource, TResult> _selector;

        public SelectObservable(IObservable<TSource> observable, Func<TSource, TResult> selector) : base(observable)
        {
            _selector = selector;
        }

        public override async Task OnNext(TSource value) => await _observer.OnNext(_selector.Invoke(value));
    }
}