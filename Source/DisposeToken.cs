using System;

namespace RX
{
    internal class DisposeToken : IDisposable
    {
        public Action DisposeAction;

        public void Dispose() => DisposeAction?.Invoke();
    }
}