using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum mtc_Type : byte { player_state, dagger_throw, dagger_die, player_die, config_update, dash, fireball_throw, fireball_die, character_select, payload_event, torch_placed, trap_placed, trap_caught, reflect, hill_score, hill_move, team_select, WC_controller }




[System.Serializable]
public struct mtc_data
{
    public mtc_Type type;
    public float t;
    public object body;
    public int id;

    public mtc_data(mtc_Type type, float t, object body,int id)
    {
        this.type = type;
        this.t = t;
        this.body = body;
        this.id = id;
    }
}


public class splitter : MonoBehaviour {
    [SerializeField]
    obj_event message_in;

    [SerializeField]
    mtc_object_event_dict local_events;

    [SerializeField] 
    mtc_object_event_dict network_events;

    [SerializeField]
    obj_event out_unreliable;

    [SerializeField]
    obj_event out_large;

    [SerializeField]
    obj_event out_mtc;
    

	// Use this for initialization
	void Start () {
        message_in.e.AddListener(split);
        foreach(KeyValuePair<mtc_Type,sync_event> pair in local_events)
        {
            pair.Value.e.AddListener((t, o, id) =>
             out_mtc.Invoke((object)(new mtc_data(pair.Key, t, o, id))));
            pair.Value.r.AddListener((t, o, id) =>
            out_unreliable.Invoke((object)(new mtc_data(pair.Key, t, o, id))));
            pair.Value.l.AddListener((t, o, id) =>
            out_large.Invoke((object)(new mtc_data(pair.Key, t, o, id))));

        }
	}




	
    void split(object obj_in)
    {
        mtc_data md = (mtc_data)obj_in;
        network_events[md.type].Invoke(md.t,md.body,md.id);
        if(md.type == mtc_Type.dagger_die) print($"received message of type {md.type}");

    }

}
