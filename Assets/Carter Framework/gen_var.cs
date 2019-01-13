using UnityEngine;
using System;

[Serializable]
public class gen_var<T> : ScriptableObject, IValue<T>
{

    [SerializeField]
    bool use_constant = false;

    [SerializeField]
    T constant;

    [SerializeField]
    private T value;

    public T val
    {
        get { return use_constant ? constant : value; }
        set { this.value = value; }
    }

    public static implicit operator T(gen_var<T> so)
    {
        return so.val;
    }
}
