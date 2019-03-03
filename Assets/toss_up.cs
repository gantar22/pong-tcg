using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class toss_up : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField]
    float_var starting_speed;

    [SerializeField]
    event_object start;


	// Use this for initialization
	void OnEnable () {
        rb = GetComponent<Rigidbody2D>();
        start.e.AddListener(() => 
            {
            if (Random.value > .5f)
            {
                rb.velocity = Vector2.right * starting_speed;
            }
            else
            {
                rb.velocity = Vector2.left * starting_speed;
            }
            });
	}
	

}
