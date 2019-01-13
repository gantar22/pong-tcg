using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_scene_by_index : MonoBehaviour {
	public int index;
	void Start () {
		SceneManager.LoadScene(index);
	}
}
