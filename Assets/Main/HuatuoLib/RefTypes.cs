using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Foundation;

[assembly: Preserve]
enum IntEnum : int
{
    A,
    B,
}

public class RefTypes : MonoBehaviour
{

    void RefUnityEngine()
    {
        GameObject.Instantiate<GameObject>(null);
        Instantiate<GameObject>(null, null);
        Instantiate<GameObject>(null, null, false);
        Instantiate<GameObject>(null, new Vector3(), new Quaternion());
        Instantiate<GameObject>(null, new Vector3(), new Quaternion(), null);
        this.gameObject.AddComponent<RefTypes>();
        gameObject.AddComponent(typeof(RefTypes));
    }

    void RefNullable()
    {
        // nullable
        int? a = 5;
        object b = a;
    }

    void RefContainer()
    {
        new List<object>()
        {
            new Dictionary<int, int>(),
            new Dictionary<int, long>(),
            new Dictionary<int, object>(),
            new Dictionary<long, int>(),
            new Dictionary<long, long>(),
            new Dictionary<long, object>(),
            new Dictionary<object, long>(),
            new Dictionary<object, object>(),
            new SortedDictionary<int, long>(),
            new SortedDictionary<int, object>(),
            new SortedDictionary<long, int>(),
            new SortedDictionary<long, object>(),
            new HashSet<int>(),
            new HashSet<long>(),
            new HashSet<object>(),
            new List<int>(),
            new List<long>(),
            new List<float>(),
            new List<double>(),
            new List<object>(),
            new ValueTuple<int, int>(1, 1),
            new ValueTuple<long, long>(1, 1),
            new ValueTuple<object, object>(1, 1),
#region moha
            new Dictionary<int, double>(),
            new Dictionary<int, float>(),
            new Dictionary<int, Long2>(),
            new Dictionary<double, int>(),
            new Dictionary<double, long>(),
            new Dictionary<double, double>(),
            new Dictionary<double, float>(),
            new Dictionary<double, object>(),
            new Dictionary<double, Long2>(),
            new Dictionary<float, int>(),
            new Dictionary<float, long>(),
            new Dictionary<float, double>(),
            new Dictionary<float, float>(),
            new Dictionary<float, object>(),
            new Dictionary<float, Long2>(),
            new Dictionary<Long2, int>(),
            new Dictionary<Long2, long>(),
            new Dictionary<Long2, double>(),
            new Dictionary<Long2, float>(),
            new Dictionary<Long2, object>(),
            new Dictionary<Long2, Long2>(),

            new BaseEventArgs(EnumEventType.None),
            new EventArgsOne<int>(EnumEventType.None,1),
            new EventArgsOne<long>(EnumEventType.None,1),
            new EventArgsOne<double>(EnumEventType.None,1),
            new EventArgsOne<float>(EnumEventType.None,1),
            new EventArgsOne<Long2>(EnumEventType.None,Long2.zero),
            new EventArgsOne<object>(EnumEventType.None,1),
            new EventArgsTwo<int,int>(EnumEventType.None,1,1),
            new EventArgsTwo<int,double>(EnumEventType.None,1,1),
            new EventArgsTwo<int,float>(EnumEventType.None,1,1),
            new EventArgsTwo<int,long>(EnumEventType.None,1,1),
            new EventArgsTwo<int,Long2>(EnumEventType.None,1,Long2.zero),
            new EventArgsTwo<int,object>(EnumEventType.None,1,1),
            new EventArgsTwo<double,int>(EnumEventType.None,1,1),
            new EventArgsTwo<double,double>(EnumEventType.None,1,1),
            new EventArgsTwo<double,float>(EnumEventType.None,1,1),
            new EventArgsTwo<double,long>(EnumEventType.None,1,1),
            new EventArgsTwo<double,Long2>(EnumEventType.None,1,Long2.zero),
            new EventArgsTwo<double,object>(EnumEventType.None,1,1),
            new EventArgsTwo<float,int>(EnumEventType.None,1,1),
            new EventArgsTwo<float,double>(EnumEventType.None,1,1),
            new EventArgsTwo<float,float>(EnumEventType.None,1,1),
            new EventArgsTwo<float,long>(EnumEventType.None,1,1),
            new EventArgsTwo<float,Long2>(EnumEventType.None,1,Long2.zero),
            new EventArgsTwo<float,object>(EnumEventType.None,1,1),
            new EventArgsTwo<long,int>(EnumEventType.None,1,1),
            new EventArgsTwo<long,double>(EnumEventType.None,1,1),
            new EventArgsTwo<long,float>(EnumEventType.None,1,1),
            new EventArgsTwo<long,long>(EnumEventType.None,1,1),
            new EventArgsTwo<long,Long2>(EnumEventType.None,1,Long2.zero),
            new EventArgsTwo<long,object>(EnumEventType.None,1,1),
            new EventArgsTwo<Long2,int>(EnumEventType.None,Long2.zero,1),
            new EventArgsTwo<Long2,double>(EnumEventType.None,Long2.zero,1),
            new EventArgsTwo<Long2,float>(EnumEventType.None,Long2.zero,1),
            new EventArgsTwo<Long2,long>(EnumEventType.None,Long2.zero,1),
            new EventArgsTwo<Long2,Long2>(EnumEventType.None,Long2.zero,Long2.zero),
            new EventArgsTwo<Long2,object>(EnumEventType.None,Long2.zero,1),
            new EventArgsTwo<object,int>(EnumEventType.None,1,1),
            new EventArgsTwo<object,double>(EnumEventType.None,1,1),
            new EventArgsTwo<object,float>(EnumEventType.None,1,1),
            new EventArgsTwo<object,long>(EnumEventType.None,1,1),
            new EventArgsTwo<object,Long2>(EnumEventType.None,1,Long2.zero),
            new EventArgsTwo<object,object>(EnumEventType.None,1,1),
            new EventArgsThree<int,int,int>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,int,double>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,int,float>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,int,long>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,int,Long2>(EnumEventType.None,1,1,Long2.zero),
            new EventArgsThree<int,int,object>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,double,int>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,double,double>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,double,float>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,double,long>(EnumEventType.None,1,1,1),
            new EventArgsThree<int,double,Long2>(EnumEventType.None,1,1,Long2.zero),
            new EventArgsThree<int,double,object>(EnumEventType.None,1,1,1),
            new EventArgsThree<object,object,object>(EnumEventType.None,1,1,1),
            new EventArgsThree<object,float,object>(EnumEventType.None,1,1,1),
            new EventArgsThree<Vector3,float,object>(EnumEventType.None,Vector3.zero,1,1),
            
#endregion
        };
    }

    class RefStateMachine : IAsyncStateMachine
    {
        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new NotImplementedException();
        }
    }

    void RefAsyncMethod()
    {
        var stateMachine = new RefStateMachine();

        TaskAwaiter aw = default;
        var c0 = new AsyncTaskMethodBuilder();
        c0.Start(ref stateMachine);
        c0.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c0.SetException(null);
        c0.SetResult();

        var c1 = new AsyncTaskMethodBuilder();
        c1.Start(ref stateMachine);
        c1.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c1.SetException(null);
        c1.SetResult();

        var c2 = new AsyncTaskMethodBuilder<bool>();
        c2.Start(ref stateMachine);
        c2.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c2.SetException(null);
        c2.SetResult(default);

        var c3 = new AsyncTaskMethodBuilder<int>();
        c3.Start(ref stateMachine);
        c3.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c3.SetException(null);
        c3.SetResult(default);

        var c4 = new AsyncTaskMethodBuilder<long>();
        c4.Start(ref stateMachine);
        c4.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c4.SetException(null);

        var c5 = new AsyncTaskMethodBuilder<float>();
        c5.Start(ref stateMachine);
        c5.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c5.SetException(null);
        c5.SetResult(default);

        var c6 = new AsyncTaskMethodBuilder<double>();
        c6.Start(ref stateMachine);
        c6.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c6.SetException(null);
        c6.SetResult(default);

        var c7 = new AsyncTaskMethodBuilder<object>();
        c7.Start(ref stateMachine);
        c7.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c7.SetException(null);
        c7.SetResult(default);

        var c8 = new AsyncTaskMethodBuilder<IntEnum>();
        c8.Start(ref stateMachine);
        c8.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c8.SetException(null);
        c8.SetResult(default);

        var c9 = new AsyncVoidMethodBuilder();
        var b = AsyncVoidMethodBuilder.Create();
        c9.Start(ref stateMachine);
        c9.AwaitUnsafeOnCompleted(ref aw, ref stateMachine);
        c9.SetException(null);
        c9.SetResult();
        Debug.Log(b);
    }
}
