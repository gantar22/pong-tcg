using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_delayed : MonoBehaviour {

	public float delay;


	void Start () {
		Destroy(gameObject, delay);
	}
}
