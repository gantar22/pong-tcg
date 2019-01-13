using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddle_controls : MonoBehaviour {


    [SerializeField]
    int paddle_id;

    [SerializeField]
    int_var local_id;

    Rigidbody2D rb;

    [SerializeField]
    float_var move_speed;

    [SerializeField]
    Vec2Var ball_pos;

    [SerializeField]
    Vec2Var ball_vel;

    [SerializeField]
    bool_var local_multiplayer;

	// Use this for initialization
	void OnEnable () {
        rb = GetComponent<Rigidbody2D>();
        if(local_id == paddle_id) StartCoroutine(controls());
        if (local_multiplayer && paddle_id == 1) StartCoroutine(alt_controls());
	}


    IEnumerator alt_controls()
    {
        while (true)
        {
            yield return null;
            while (Input.GetKey(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow))
            {
                rb.velocity = Vector2.up * move_speed;
                yield return null;
            }
            while (Input.GetKey(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = Vector2.down * move_speed;
                yield return null;
            }
            while (!(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)))
            {
                //tracking code here.
                rb.velocity = Vector2.zero;
                yield return null;
            }
        }
    }

    IEnumerator controls()
    {
        while(true)
        {
            yield return null;
            while(Input.GetKey(KeyCode.W) && !Input.GetKeyDown(KeyCode.S))
            {
                rb.velocity = Vector2.up * move_speed;
                yield return null;
            }
            while(Input.GetKey(KeyCode.S) && !Input.GetKeyDown(KeyCode.W))
            {
                rb.velocity = Vector2.down * move_speed;
                yield return null;
            }
            while(!(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)))
            {
                //tracking code here.
                rb.velocity = Vector2.zero;
                yield return null;
            }
        }
    }
}
