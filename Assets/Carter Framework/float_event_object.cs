using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "events/float_event")]
public class float_event_object : ScriptableObject {
	[SerializeField]
	float constant = 0;

	[System.Serializable]
	public class float_event : UnityEvent<float> {}

	[SerializeField]
	public UnityEvent<float> e = new float_event();

	public void Invoke(float d) {e.Invoke(d);}

	public void Invoke() {e.Invoke(constant);}

}

