using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "anim/anim_palettes_bundle")]
public class anim_palettes_bundle : ScriptableObject {
	public List<anim_palettes_set> sets;

#if UNITY_EDITOR
	[MenuItem("Custom/Populate Palletes Bundles %#&g")]
	public static void populate_palettes_bundles() {
		Debug.Log("Starting to populate your palletes bundles");
		Object[] APSs = Selection.GetFiltered(typeof(anim_palettes_set), SelectionMode.DeepAssets);
		Object[] APBs = Selection.GetFiltered(typeof(anim_palettes_bundle), SelectionMode.DeepAssets);
		if (APSs.Length <= 0 || APBs.Length <= 0) {
			Debug.LogWarning("To populate an anim_palletes_bundle, you must first select any potential folders for the relevant anim_palletes_sets, as well as the target anim_palettes_bundle");
			return;
		}

		IOrderedEnumerable<Object> ordered_APSs = APSs.OrderBy(obj => obj.name);

		foreach (anim_palettes_bundle APB in APBs) {
			populate_one_set(APB, ordered_APSs);
		}
	}

	// Populate a particular anim_palettes_bundle
	protected static void populate_one_set(anim_palettes_bundle APB, IOrderedEnumerable<Object> APSs) {
		APB.sets = new List<anim_palettes_set>();
		Debug.Log("Populating your anim_palettes_bundle with all sets found containing substring: " + APB.name);

		foreach (anim_palettes_set set in APSs) {
			if (set.name.Contains(APB.name)) {
				APB.sets.Add(set);
			}
		}
		EditorUtility.SetDirty(APB);
	}
#endif
}
