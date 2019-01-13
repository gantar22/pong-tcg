using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class init_fade_into_lobby : MonoBehaviour {

	public float fade_time;

	public Image black_panel;


	// Initialization
	private void Awake() {
		DontDestroyOnLoad(gameObject);
		black_panel.CrossFadeColor(new Color(1, 1, 1, 0), fade_time, false, true);
		StartCoroutine(destroy_after_fade());
	}

	IEnumerator destroy_after_fade() {
		yield return new WaitForSeconds(fade_time + 0.3f);
		Destroy(gameObject);
	}
}
