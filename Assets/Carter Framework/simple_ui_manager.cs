using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class simple_ui_manager : MonoBehaviour {

    [SerializeField]
    client_var client;

    
    [SerializeField]
    GameObject loading;

    [SerializeField]
    GameObject start_menu;


    [SerializeField]
    obj_event to_splitter;

    [SerializeField]
    party_var my_party;

    [SerializeField]
    GameObject sign_up_error;

    [SerializeField]
    GameObject connect_error;

    [SerializeField]
    GameObject login_error;

    [SerializeField]
    int_var local_id;

    [SerializeField]
    GameObject party_menu;

    [SerializeField]
    bool_var host_var;

    [SerializeField]
    event_object notify_parties;

    bool connected = false;

    [SerializeField]
    event_object login;

   

    string my_name;

    string pass = "";

    [SerializeField]
    event_object begin_hoc;


	// Use this for initialization
	public void Start () {

        StartCoroutine(starting());

        
	}

    IEnumerator starting()
    {
        yield return new WaitForSeconds(.2f);
        client.val.Connect(0, OnConnect, () => connect_error.SetActive(true));
        client.val.Register_friend_requests(str => { return; });
        client.val.Register_Receive_Request((str, a) => { return; });
        client.val.Register_Receive_Invite((s, a) => { return; });
        System.Random rand = new System.Random();
        string chars = "QWERTYUIPOASDFGHJKLMNBVXC";
        my_name = new string(Enumerable.Repeat(chars, 8).Select(s => s[rand.Next(s.Length)]).ToArray());
        my_party.val = new Party_Names(my_name,new List<string>());

        while (true)
        {
            yield return null;
            if (my_party.val.members.Any(_ => true)) { begin_hoc.Invoke(); break; }
        }
    }

    IEnumerator notify_party_change(event_object notify)
    {
        yield return new WaitUntil(() => my_party.val.members != null);
        while (true)
        {
            Party_Names pn = my_party.val;
            yield return new WaitUntil(
                () => !pn.members.SequenceEqual(my_party.val.members)
                      || pn.leader != my_party.val.leader
                );
            notify_parties.Invoke();
           
        }
    }


    void OnConnect()
    {
        connected = true;
        client.val.Register_Message_Receive(to_splitter.Invoke);
        client.val.Register_Party_List(pn => 
        {
            my_party.val = pn;
            if (pn.members.Contains(my_name))
            {
                local_id.val = pn.members.IndexOf(my_name) + 1;
                host_var.val = false;
            }
            else
            {
                local_id.val = 0;
                host_var.val = true;
            }

            if (pn.members.Any(_ => true)) { start_menu.SetActive(false); party_menu.SetActive(true); print("yooo"); } 

        });
        client.val.Register_friends(fl => { return; });
        client.val.Register_friend_callbacks(() => { return; }, () => { return; });

        StartCoroutine(notify_party_change(notify_parties   ));
        StartCoroutine(delay_sign_up());
    }

    public void sign_up()
    {
        StartCoroutine(delay_sign_up());
    }


    IEnumerator delay_sign_up()
    {
        yield return new WaitUntil(() => connected);
        client.val.Create_Player(my_name, pass, () => StartCoroutine(delay_sign_in()), 
            () => { sign_up_error.SetActive(true);  loading.SetActive(false); });
    }
    
    public void login_in()
    {
        StartCoroutine(delay_sign_in());
    }

    public void leave()
    {
        client.val.leave_party();
    }

    public void find()
    {
        client.val.find_match();
    }

    IEnumerator delay_sign_in()
    {
        yield return new WaitUntil(() => connected);
        client.val.Login(my_name, pass, () => { loading.SetActive(false); start_menu.SetActive(true); login.Invoke(); }
            ,() => { login_error.SetActive(true); loading.SetActive(false); });
    }

}
