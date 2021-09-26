
using System;
using System.Threading.Tasks;

namespace RX
{
    public interface IReadonlyReactiveProperty<T> : IObservable<T>
    {
        T Value { get; }
    }

    public interface IReactivePreoperty<T> : IReadonlyReactiveProperty<T>
    {
        new T Value { get; set; }
    }

    public interface IObservable<out T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }

    public interface IObserver<in T>
    {
        Task OnNext(T value);
        Task OnError(Exception e);
        Task OnCompleted();
    }

    public interface IRXObserver<in T> : IObserver<T>
    {
        bool SkipLatestOnSubscribe { get; }
        int Priority { get; }
    }
}