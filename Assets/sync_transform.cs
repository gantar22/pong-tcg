using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class serializable_vec2
{
    public float x;
    public float y;

    public serializable_vec2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector2(serializable_vec2 sv2)
    {
        return new Vector2(sv2.x, sv2.y);
    }

    public static implicit operator serializable_vec2(Vector2 v2)
    {
        return new serializable_vec2(v2.x, v2.y);
    }

    public static implicit operator Vector3(serializable_vec2 sv2)
    {
        return new Vector3(sv2.x, sv2.y);
    }


}

[System.Serializable]
public struct transform_state
{
    public serializable_vec2 pos;
    public serializable_vec2 vel;
    public float theta;

    public transform_state(Vector2 pos, Vector2 vel, float theta)
    {
        this.pos = pos;
        this.vel = vel;
        this.theta = theta;
    }
}

public class sync_transform : sync_behaviour<transform_state>
{
    
    Vector2 target_pos;

    float target_theta;

    float smooth_rot_vel = 0;

    Vector3 smooth_vel = Vector3.zero;

    Rigidbody2D rb;
    
    // Update is called once per frame
    void Update()
    {
        if (!is_local) //you are not the local go
        {
            if (Vector3.Distance(transform.position, target_pos) > 5) transform.position = target_pos;
            if (Mathf.Abs(((Vector2)transform.position - target_pos).magnitude) > 1) return;
            transform.position = Vector3.SmoothDamp(transform.position, target_pos, ref smooth_vel, .005f);
            
        }
        else
        {
            state = new transform_state(transform.position, rb ? rb.velocity : Vector2.zero, transform.eulerAngles.z);
        }

    }



    public override void rectify(float t, transform_state ts)
    {

        target_pos = ts.pos;
        if(rb) rb.velocity = ts.vel;
        transform.eulerAngles = Vector3.forward * ts.theta;

    }
    // Use this for initialization
    public override void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        base.Start();
        state = new transform_state(transform.position, rb ? rb.velocity : Vector2.zero, transform.eulerAngles.z);
        
    }
    
    private void OnEnable()
    {
        sync_continously();
    }
}
