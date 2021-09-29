using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RX
{
    internal class SelectManyObservable<TSource, TResult> : ObservableExpression<TSource, TResult>
    {
        private Func<TSource, IEnumerable<TResult>> _selector;

        public SelectManyObservable(IObservable<TSource> observable, Func<TSource, IEnumerable<TResult>> selector) : base(observable)
        {
            _selector = selector;
        }

        public override async Task OnNext(TSource value)
        {
            foreach (var item in _selector.Invoke(value))
            {
                await _observer.OnNext(item);
            }
        }
    }
}