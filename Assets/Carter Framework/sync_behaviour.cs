using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(network_id))]
public class sync_behaviour<T> : MonoBehaviour {

    [SerializeField]
    public sync_event in_event;

    [SerializeField]
    public sync_event out_event;

    [SerializeField]
    public int_var local_id;

    [SerializeField]
    public float_var t0;

    [SerializeField]
    [Range(1,27)]
    float sync_rate;


    public T state;

    public bool is_local
    {
        get { return gameObject_id.val == local_id.val; }
    }
    public IValue<int> gameObject_id;

    // Use this for initialization
    public virtual void Start() {
        in_event.e.AddListener(receive_state);
        gameObject_id = GetComponent<network_id>();
    }

    public void sync_continously()
    {
        StartCoroutine(send_update());
    }

    IEnumerator send_update()
    {
        yield return null;
        while(gameObject_id.val == local_id.val)
        {
            yield return new WaitForSeconds(1 / sync_rate);
            send_state_unreliable(state);
        }
    }

    void receive_state(float t, object o, int id)
    {
        //print($"one way trip time {(Time.time - t0.val) - t}");
        //if (t > Time.time - t0.val) print($"you got a message from the future! from: {t}, now: {Time.time - t0.val} ");
        //if (id == local_id.val) print($"you got a message you shouldn't have {id}");
        if (id == gameObject_id.val)
            rectify(t + t0.val, (T)o);
    }

    public virtual void rectify(float t, T state)
    {
        //override this to rectify differences
    }

    public void send_state(T state)
    { //Call this to send changes
        out_event.Invoke(Time.time - t0.val, (object)state, gameObject_id.val);
    }

    void send_state_unreliable(T state)
    {
        //print($"Sending state at t_i = {Time.time - t0.val}");
        out_event.Invoke(Time.time - t0.val, (object)state, gameObject_id.val,reliable: false);
    }

}
