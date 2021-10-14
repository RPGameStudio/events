
using System;
using System.Threading.Tasks;

namespace RX
{
    public interface IReadonlyReactiveProperty<T> : IObservable<T>
    {
        T Value { get; }
    }

    public interface IReactiveProperty<T> : IReadonlyReactiveProperty<T>
    {
        new T Value { get; set; }
    }

    public interface IObservable<out T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }

    public interface IObserver<in T>
    {
        bool SkipLatestOnSubscribe { get; }
        int Priority { get; }

        Task OnNext(T value);
        Task OnError(Exception e);
        Task OnCompleted();
    }
}