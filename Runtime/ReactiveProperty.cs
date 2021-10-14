using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RX
{
    [Serializable]
    public class ReactiveProperty<T> : IObservable<T>, IReactiveProperty<T>
    {
        private T _value = default;
        [NonSerialized] private SortedDictionary<int, LinkedList<IObserver<T>>> _observers;

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                    _ = NotifyValueChanged(_value = value);
            }
        }

        public ReactiveProperty() => _observers = new SortedDictionary<int, LinkedList<IObserver<T>>>();
        public ReactiveProperty(T value) : this() => _value = value;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.ContainsKey(observer.Priority))
            {
                _observers.Add(observer.Priority, new LinkedList<IObserver<T>>());
            }

            var observers = _observers;
            _observers[observer.Priority].AddLast(observer);

            if (!observer.SkipLatestOnSubscribe)
                observer.OnNext(_value);

            return new DisposeToken
            {
                DisposeAction = async () =>
                {
                    observers[observer.Priority].Remove(observer);
                    await observer.OnCompleted();
                },
            };
        }

        private async Task NotifyValueChanged(T value)
        {
            foreach (var key in _observers.Keys)
            {
                foreach (var observer in _observers[key])
                {
                    try
                    {
                        await observer.OnNext(value);
                    }
                    catch (Exception e)
                    {
                        await observer.OnError(e);
                    }
                }
            }
        }

        #region Equals and type casting
        public override bool Equals(object obj)
        {
            if (obj is IReadonlyReactiveProperty<T> property)
                return _value == null ? property.Value == null : _value.Equals(property.Value);
            else if (obj is T t)
                return _value == null ? t == null : _value.Equals(t);
            else
                return false;
        }

        public static bool operator ==(ReactiveProperty<T> rf, T v) => !Equals(rf, null) && rf.Equals(v);
        public static bool operator !=(ReactiveProperty<T> rf, T v) => !Equals(rf, null) && !rf.Equals(v);
        public static bool operator ==(T v, ReactiveProperty<T> rf) => !Equals(rf, null) && rf.Equals(v);
        public static bool operator !=(T v, ReactiveProperty<T> rf) => !Equals(rf, null) && !rf.Equals(v);
        public static bool operator ==(ReactiveProperty<T> rf1, ReactiveProperty<T> rf2) => !Equals(rf1, null) && rf1.Equals(rf2);
        public static bool operator !=(ReactiveProperty<T> rf1, ReactiveProperty<T> rf2) => !Equals(rf1, null) && !rf1.Equals(rf2);
        public static implicit operator T(ReactiveProperty<T> property) => property.Value;
        public override int GetHashCode() => -1939223833 + EqualityComparer<T>.Default.GetHashCode(_value);
        #endregion
    }
}
