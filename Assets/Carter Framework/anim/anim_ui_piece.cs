using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class anim_ui_piece : anim_piece {

	private Image image;


	protected override void Awake() {
		image = GetComponent<Image>();
		base.Awake();
	}

	public override void refresh_sprite() {
		if (0 <= animation_index && animation_index < anim_Palettes_Bundle.sets.Count) {
			anim_palettes_set set = anim_Palettes_Bundle.sets[animation_index];
			if (0 <= palette_index && palette_index < set.palettes.Count) {
				spritesheet sheet = set.palettes[palette_index];
				if (0 <= sprite_index && sprite_index < sheet.sprites.Count) {
					image.sprite = sheet.sprites[sprite_index];
				}
			}
		}
	}

}
