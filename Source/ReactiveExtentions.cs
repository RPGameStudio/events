using System;
using System.Threading.Tasks;

namespace RX
{
    public static class ReactiveExtentions
    {
        public static IObservable<T> Where<T>(this IObservable<T> observable, Predicate<T> predicate) => new WhereObservable<T>(observable, predicate);
        public static IObservable<TR> Select<T, TR>(this IObservable<T> observable, Func<T, TR> selector) => new SelectObservable<T, TR>(observable, selector);
        public static IObservable<T> Filter<T>(this IObservable<T> observable) => observable.Where(x => x is T).Select(x => x);

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
}
