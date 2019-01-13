using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class anim_piece : MonoBehaviour {

	public anim_palettes_bundle anim_Palettes_Bundle;

	public int animation_index;
	public int palette_index;
	public int sprite_index;
	private SpriteRenderer sprite_renderer;


	protected virtual void Awake() {
		sprite_renderer = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		refresh_sprite();
	}

	private void OnEnable() {
		StartCoroutine(refresh_on_anim_change());
		StartCoroutine(refresh_on_palette_change());
		StartCoroutine(refresh_on_sprite_change());
	}

	public virtual void refresh_sprite() {
		if (0 <= animation_index && animation_index < anim_Palettes_Bundle.sets.Count) {
			anim_palettes_set set = anim_Palettes_Bundle.sets[animation_index];
			if (0 <= palette_index && palette_index < set.palettes.Count) {
				spritesheet sheet = set.palettes[palette_index];
				if (0 <= sprite_index && sprite_index < sheet.sprites.Count) {
					sprite_renderer.sprite = sheet.sprites[sprite_index];
				}
			}
		}
	}

	// Coroutines set up to refresh the sprite whenever one of the animation indices change
	private IEnumerator refresh_on_anim_change() {
		while (true) {
			int previous = animation_index;
			yield return new WaitUntil(() => previous != animation_index);
			refresh_sprite();
		}
	}
	private IEnumerator refresh_on_palette_change() {
		while (true) {
			int previous = palette_index;
			yield return new WaitUntil(() => previous != palette_index);
			refresh_sprite();
		}
	}
	private IEnumerator refresh_on_sprite_change() {
		while (true) {
			int previous = sprite_index;
			yield return new WaitUntil(() => previous != sprite_index);
			refresh_sprite();
		}
	}

}
