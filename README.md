# events
Serializable async reactive properties with UniRx syntax which made to replace С# events.

Supported expressions:
1) Where
2) Select
3) SelectMany

Differences from UniRx:
1) User can manually set subscribtion priority. All observers will be notified in adjusted priority (in case priority is similar - in subscribtion order)
2) Async/await support
3) No redundant functionality

Why just not use C# or UnityEvents?
1) More clever syntax
2) Smaller chance to face a memory leak

That's how it looks with c# events:
```
private class DataStorage
{
  public event Action<int> OnDataChanged = delegate { }
}

private class CCharpEventsExample : MonoBehaviour
{
  private DataStorage _storage; //let's say this dependency already injected

  private void Awake()
  {
    _storage.OnDataChanged += OnDataChangedCallback;
  }

  private void OnDestroy()
  {
      //in case user forgot to unsubscribe - memory leak
      //user should implement onDesotroy in every class with any subscribtion
      _storage.OnDataChanged -= OnDataChangedCallback;
  }

  //user must create methods such this instead of using lambda expressions
  //in other way user can't unsubscribe
  private void OnDataChangedCallback(int data)
  {
      if (data >= 100)
          Debug.Log(data);
  }
}
```

That's how it looks with this package:
```
    class DataStorage
    {
        private ReactiveProperty<float> _data; //can be serialized

        public IReadonlyReactiveProperty<float> ReadonlyData => _data; //supports encapsulation
        public ReactiveProperty<float> Data => _data; //in general case user shuldn't be able to modify data state in public. Used for example

        public DataStorage(float value) => _data = new ReactiveProperty<float>(value);
    }

    public class RXExample : MonoBehaviour
    {
        private void Awake()
        {
            var storage = new DataStorage(100);

            storage.Data.Value = 99;
            //storage.ReadonlyData.Value = 99; //error

            var sub1 = storage.ReadonlyData
                    .Where(x => x >= 100)
                    .Subscribe(onNext: async x => Debug.Log($"first subscribtion {x}"), 
                               onCompleted: async () => Debug.Log("1st disposed"), //optional param
                               onError: null,                                      //optional param
                               skipLatestValueOnSubscribe: false,                  //optional param
                               priority: 1);                                       //optional param
            var sub2 = storage.ReadonlyData
                    .Where(x => x >= 150)
                    .Subscribe(onNext: async x => Debug.Log($"second subscribtion {x}"), 
                               onCompleted: async () => Debug.Log("2nd disposed"), //optional param
                               onError: null,                                      //optional param
                               skipLatestValueOnSubscribe: true,                   //optional param
                               prioirty: 2);                                       //optional param

            //no output
            a.Data.Value = 98;
            //only first subscribtion will be triggered
            a.Data.Value = 101;
            //first then second
            a.Data.Value = 151;

            //auto dispose implemented by AddTo method
            sub1.AddTo(this);
            Destroy(gameObject);
            
            //manual dispose
            sub2.Dispose();
        }
    }
```
