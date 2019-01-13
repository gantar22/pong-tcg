using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "events/string_event")]
public class string_event_object : ScriptableObject {
	[SerializeField]
	string constant = "";

	public class string_event : UnityEvent<string> {}

	public UnityEvent<string> e = new string_event();

	public void Invoke(string d) {e.Invoke(d);}

	public void Invoke() {e.Invoke(constant);}

}

