using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IValue<T>
{
    T val { get; set; }
}

[CreateAssetMenu(menuName = "variables/float")]
public class float_var : gen_var<float> {}
