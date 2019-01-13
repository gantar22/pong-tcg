using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "events/unit")]
public class event_object : ScriptableObject
{


    public void addListener(UnityAction action)
    {
        e.AddListener(action);
    }

    private UnityEvent _e;

    public UnityEvent e
    {
        get { if (_e == null) _e = new UnityEvent(); return _e; }
    }

    public void Invoke() { e.Invoke(); }

}
