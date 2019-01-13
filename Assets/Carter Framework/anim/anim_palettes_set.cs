using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "anim/anim_palettes_set")]
public class anim_palettes_set : ScriptableObject {
	public List<spritesheet> palettes;

#if UNITY_EDITOR
	// Fill in each selecteed anim_palettes_set object with all respectively named spritesheet objects.
	// You must have all target anim_palettes_set objects selected, in addition to any potential directories for the spritesheet objects.
	// Additionally, each target anim_palettes_set must be named according to the common string which each such spritesheet object's name will contain.
	[MenuItem("Custom/Populate Palettes Sets %#g")]
	public static void populate_palettes_sets() {
		Object[] spritesheets = Selection.GetFiltered(typeof(spritesheet), SelectionMode.DeepAssets);
		Object[] APSs = Selection.GetFiltered(typeof(anim_palettes_set), SelectionMode.DeepAssets);
		if (spritesheets.Length <= 0 || APSs.Length <= 0) {
			Debug.LogWarning("To populate an anim_palettes_set, you must first select any potential folders for the relevant spritesheets, as well as the target anim_palettes_set");
			return;
		}

		IOrderedEnumerable<Object> ordered_spritesheets = spritesheets.OrderBy(obj => obj.name);

		foreach (anim_palettes_set APS in APSs) {
			populate_one_set(APS, ordered_spritesheets);
		}
	}

	// Populate a particular anim_palettes_set
	protected static void populate_one_set(anim_palettes_set APS, IOrderedEnumerable<Object> spritesheets) {
		APS.palettes = new List<spritesheet>();
		Debug.Log("Populating your anim_palettes_set with all spritesheets found containing substring: " + APS.name);

		foreach (spritesheet sheet in spritesheets) {
			if (sheet.name.Contains(APS.name)) {
				APS.palettes.Add(sheet);
			}
		}
		EditorUtility.SetDirty(APS);
	}
#endif
}
