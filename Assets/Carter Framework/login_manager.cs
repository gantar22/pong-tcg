using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login_manager : MonoBehaviour {

    [SerializeField]
    event_object to_connect;

    [SerializeField]
    event_object to_login;

    [SerializeField]
    client_var client;

    [SerializeField]
    event_object on_connect;

    [SerializeField]
    event_object fail_connect;




	// Use this for initialization
	void Start () {
        to_connect.addListener(connect);
	}
	
	
    void connect()
    {
        client.val.Connect(0, on_connect.Invoke, fail_connect.Invoke);
    }


}
