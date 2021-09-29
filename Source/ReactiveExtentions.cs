using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RX
{
    public static class ReactiveExtentions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Func<T, Task> onNext, bool skipLatest = false, int priority = 1000) => Subscribe(observable, onNext, null, skipLatest, priority);
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Func<T, Task> onNext, Func<Exception, Task> onError, bool skipLatest = false, int priority = 0) => Subscribe(observable, onNext, onError, null, skipLatest, priority);
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Func<T, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted, bool executeOnNextOnSubscribe = false, int priority = 0)
        {
            var observer = new DefaultObserver<T>
            {
                OnNext = onNext,
                OnCompleted = onCompleted,
                OnError = onError,
                SkipLatestOnSubscribe = executeOnNextOnSubscribe,
                Priority = priority,
            };

            return observable.Subscribe(observer);
        }
    }

    public static class LinqReactiveExtentions
    {
        public static IObservable<T> Where<T>(this IObservable<T> observable, Predicate<T> predicate) => new WhereObservable<T>(observable, predicate);
        public static IObservable<TResult> Select<TSource, TResult>(this IObservable<TSource> observable, Func<TSource, TResult> selector) => new SelectObservable<TSource, TResult>(observable, selector);
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> observable, Func<TSource, IEnumerable<TResult>> selector) => new SelectManyObservable<TSource, TResult>(observable, selector);
    }
}
