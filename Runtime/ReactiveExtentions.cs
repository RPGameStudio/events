using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RX
{
    public static class ReactiveExtentions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Func<T, Task> onNext, bool skipLatest = false, int priority = 1000) => Subscribe(observable, onNext, null, skipLatest, priority);
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Func<T, Task> onNext, Func<Task> onCompleted, bool skipLatest = false, int priority = 0) => Subscribe(observable, onNext, onCompleted, null, skipLatest, priority);
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Func<T, Task> onNext, Func<Task> onCompleted, Func<Exception, Task> onError, bool executeOnNextOnSubscribe = false, int priority = 0)
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

    public static class DisposableExtentions
    {
        public static IDisposable AddTo(this IDisposable disposable, MonoBehaviour @object) => disposable.AddTo(@object.gameObject);
        public static IDisposable AddTo(this IDisposable disposable, GameObject @object)
        {
            var trigger = @object.GetComponent<ObservableDestroyTrigger>();

            if (trigger == null)
                trigger = @object.AddComponent<ObservableDestroyTrigger>();

            return trigger.Append(disposable);
        }
    }
}
