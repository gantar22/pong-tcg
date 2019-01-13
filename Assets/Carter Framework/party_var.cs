using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "variables/party_info")]
public class party_var : gen_var<Party_Names> {
	public string get_name(int player_id) {
		if (player_id == 0) {
			return val.leader;
		} else {
			return val.members[player_id - 1];
		}
	}
}
