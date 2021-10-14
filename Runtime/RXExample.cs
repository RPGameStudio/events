using System.Collections;
using System.Collections.Generic;
using RX;
using UnityEngine;

namespace Example
{
    class DataStorage
    {
        private ReactiveProperty<float> _data;

        public IReadonlyReactiveProperty<float> ReadonlyData => _data;
        public ReactiveProperty<float> Data => _data;

        public DataStorage(float value) => _data = new ReactiveProperty<float>(value);
    }

    public class RXExample : MonoBehaviour
    {
        private void Awake()
        {
            var a = new DataStorage(100);

            a.Data.Value = 99;
            //a.ReadonlyData.Value = 99; //error

            var sub1 = a.ReadonlyData.Where(x => x >= 100).Select(x => x.ToString()).Subscribe(async x => Debug.Log($"first subscribtion {x}"), async () => Debug.Log("1st completed"), null, false, 1);
            var sub2 = a.ReadonlyData.Where(x => x >= 150).Select(x => x.ToString()).Subscribe(async x => Debug.Log($"second subscribtion {x}"), async () => Debug.Log("2nd completed"), null, true, 2);

            //no output
            a.Data.Value = 98;
            Debug.Log("\n");
            //only first
            a.Data.Value = 101;
            Debug.Log("\n");
            //first then second
            a.Data.Value = 151;

            //sub1 will be disposed after sub2, because gameobject will be deleted in other frame
            sub1.AddTo(this);
            Destroy(gameObject);
            sub2.Dispose();
        }
    }
}