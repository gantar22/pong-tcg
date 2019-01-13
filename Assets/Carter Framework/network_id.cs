using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network_id : MonoBehaviour, IValue<int> {
    [SerializeField]
    private int id;

    public int val
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }
}
