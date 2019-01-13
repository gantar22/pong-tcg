using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class circle_from_line : MonoBehaviour {

    LineRenderer lr;

    [SerializeField]
    float base_radius;

    [SerializeField]
    float twiddle_radius;

    [SerializeField]
    float angle;

    [SerializeField]
    int steps;
    

	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
        if(!lr) lr = GetComponent<LineRenderer>();
        List<Vector3> pos = new List<Vector3>();
        float effective_radius = base_radius + twiddle_radius;
		for(int i = 0; i < steps; i++)
        {
            
            float theta = angle + ((float)i / (float)steps) * 2 * Mathf.PI;
            pos.Add(new Vector3(Mathf.Cos(theta) * effective_radius, Mathf.Sin(theta) * effective_radius, 0));
        }
        lr.positionCount = steps;
        lr.SetPositions(pos.ToArray());
	}
}
