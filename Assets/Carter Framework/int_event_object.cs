using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "events/int_event")]
public class int_event_object : ScriptableObject {
	[SerializeField]
	int constant = 0;

	public class int_event : UnityEvent<int> {}

	public UnityEvent<int> e = new int_event();

	public void Invoke(int d) {e.Invoke(d);}

	public void Invoke() {e.Invoke(constant);}

}

