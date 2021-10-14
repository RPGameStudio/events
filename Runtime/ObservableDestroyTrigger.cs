using System;
using System.Collections.Generic;
using UnityEngine;

namespace RX
{
    public class ObservableDestroyTrigger : MonoBehaviour
    {
        private List<IDisposable> _targets = new List<IDisposable>();

        public IDisposable Append(IDisposable disposable)
        {
            _targets.Add(disposable);
            return disposable;
        }

        private void OnDestroy()
        {
            foreach (var item in _targets)
            {
                item?.Dispose();
            }
        }
    }
}
