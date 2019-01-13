using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouse_over_trigger : MonoBehaviour {




    public void OnMouseEnter()
    {
        GetComponent<Animator>().SetBool("mouse_over",true);
    }

    public void OnMouseExit()
    {
        GetComponent<Animator>().SetBool("mouse_over", false);
    }
}
