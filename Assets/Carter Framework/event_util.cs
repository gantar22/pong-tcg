using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class gen_event<T> : ScriptableObject
{

    private UnityEvent<T> _e;

    public void addListener(UnityAction<T> action)
    {
        e.AddListener(action);
    }

    public UnityEvent<T> e
    {
        get { if (_e == null) _e = new adhoc_event<T>(); return _e; }
    }

    public void Invoke(T arg) { e.Invoke(arg); }

}



public class adhoc_event<T> : UnityEvent<T> { }

public class adhoc_event<T,T2> : UnityEvent<T,T2> { }

[Serializable]
public class gen_event<T,T2> : ScriptableObject
{

    private UnityEvent<T,T2> _e;

    public void addListener(UnityAction<T,T2> action)
    {
        e.AddListener(action);
    }

    public UnityEvent<T,T2> e
    {
        get { if (_e == null) _e = new adhoc_event<T,T2>(); return _e; }
    }

    public void Invoke(T arg,T2 arg2) { e.Invoke(arg,arg2); }

}

[Serializable]
public class gen_event<T, T2, T3> : ScriptableObject {

	private UnityEvent<T, T2, T3> _e;

    public void addListener(UnityAction<T,T2,T3> action)
    {
        e.AddListener(action);
    }

    public UnityEvent<T, T2, T3> e {
		get { if (_e == null) _e = new adhoc_event<T, T2, T3>(); return _e; }
	}

	public void Invoke(T arg, T2 arg2, T3 arg3) { e.Invoke(arg, arg2, arg3); }

}
