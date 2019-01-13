using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_parent : MonoBehaviour {

	public List<anim_piece> anim_pieces;
	public int palette_index { get; private set; }


	public void set_all_animation(int anim_index) {
		foreach (anim_piece piece in anim_pieces) {
			piece.animation_index = anim_index;
		}
	}

	public void set_all_palette(int inc_palette_index) {
		palette_index = inc_palette_index;
		foreach (anim_piece piece in anim_pieces) {
			piece.palette_index = inc_palette_index;
		}
	}

	public void set_all_sprite(int sprite_index) {
		foreach (anim_piece piece in anim_pieces) {
			piece.sprite_index = sprite_index;
		}
	}

	private void update_all_sprites() {
		foreach (anim_piece piece in anim_pieces) {
			piece.refresh_sprite();
		}
	}

}
