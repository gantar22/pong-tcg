using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_between : MonoBehaviour {

	
    public void run(Vector3 source, Vector3 dest, int steps, float dur)
    {
        StartCoroutine(move(source, dest, steps, dur));
    }

    IEnumerator move(Vector3 source, Vector3 dest, int steps, float dur)
    {
        float dist = Vector3.Distance(source, dest);
        transform.position = source;
        float step_length = dist / steps;
        Vector3 dir = (dest - source).normalized;
        while(Vector3.Distance(transform.position,dest) > step_length)
        {
            transform.position += dir * step_length;
            yield return new WaitForSeconds(dur / steps);
        }
        transform.position = dest;
    }
}
