using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;




enum Custom_msg_type 
{ CREATE_PLAYER, LOGIN, SEND_PLAYER_LIST, SEND_PARTY_LIST, LEAVE_PARTY, REQ_JOIN_PARTY, INVITE_PLAYER, START_GAME, LOGOUT, MTC, RPC, CMD, END_GAME, FIND_MATCH, ADD_FRIEND, GET_PLAYER_INFO, SET_PLAYER_INFO, GET_FRIEND }

[System.Serializable]
struct Message_package
{
    public object message;
    public Custom_msg_type type;
}

[System.Serializable]
struct Message_obj
{
    public string arg1;
    public string arg2;
    public int target_connection;
}

class handle_message_event : gen_event<Tuple<Custom_msg_type, Message_obj>, object>{}

public class thirds_client : MonoBehaviour, IProtagoras_Client<object>
{
    public bool debug = true;
    int host;
    int conn_id;
    int reliable_channel;
    int state_update_channel;
    int large_data_channel;
    Action<Party_Names> party_event;
    Action<List<connection_struct>> friend_event;
    Action<string> invite_event;
    Action<string> request_event;
    Action<object> message_event;
    Action<string> friend_request;
    Action<player_info> pi_event;
    bool connected = false;
    HostTopology topology;
    handle_message_event handle_data_event;

    [SerializeField]
    obj_event in_multicast;

    [SerializeField]
    obj_event in_unreliable;

    [SerializeField]
    obj_event in_large;

    [SerializeField]
    float_event_object start_event;

    [SerializeField]
    event_object start_in;

    [SerializeField]
    bool local;

    [SerializeField]
    client_var client;

    bool in_game;


    void Start()
    {
        DontDestroyOnLoad(gameObject);

        client.val = this;
        in_multicast.e.AddListener(mtc);
        in_unreliable.e.AddListener(mtc_unrel);
        start_in.e.AddListener(() => Start_Game());
        in_large.e.AddListener(mtc_large);
    }
    void mtc(object o) { Multicast(o); } //because Mulitcast returns a bool and you can't cast lambdas to unity events
    void mtc_unrel(object o) { Multicast(o, reliable: true); }
    void mtc_large(object o) { Multicast(o, large: true); }

    IEnumerator Receive(Action connect, Action failure)
    {
        int _conn;
        int _channel;
        byte[] _buffer = new byte[2048];
        int data_size;
        byte error;
        Message_obj msg = new Message_obj();
        Message_package msg_p = new Message_package();
        handle_data_event = ScriptableObject.CreateInstance<handle_message_event>();
        
        while (true)
        {
            NetworkEventType _data = (byte)0;
            while (_data != NetworkEventType.Nothing)
            {
                _data = NetworkTransport.ReceiveFromHost(host, out _conn, out _channel, _buffer, 2048, out data_size, out error);
                switch (_data)
                {
                    case NetworkEventType.DataEvent:
                        msg_p = unformat_bytes<Message_package>(_buffer);
                        if (msg_p.message is Message_obj)
                        {
                            msg = (Message_obj)msg_p.message;
                        }
                        print($"received {msg_p.type}");
                        handle_message(msg_p.type, msg, msg_p.message);
                        handle_data_event.Invoke(new Tuple<Custom_msg_type, Message_obj>(msg_p.type, msg), msg_p.message);
                        break;
                    case NetworkEventType.ConnectEvent:
                        if (debug) print($"conected on channel: {_channel}, with error: {(NetworkError)error} on conn: {_conn}");
                        if (_conn == conn_id) connected = true;
                        connect();
                        break;
                    case NetworkEventType.DisconnectEvent:
                        if (debug) print($"didn't connect: {(NetworkError)error}");
                        failure();
                        break;
                }
            }

            yield return null;
        }
    }
    void handle_message(Custom_msg_type _type, Message_obj msg, object message)
    {
        if (_type == Custom_msg_type.START_GAME) in_game = true;
        if (_type == Custom_msg_type.END_GAME) in_game = false;
        switch (_type)
        {
            case Custom_msg_type.CREATE_PLAYER:
                //make the server sent this back if you do create a player
                //then make our create player function call an ienumerator that 
                //yields until we recieve this message. That way we can make 
                //create_player only return true iff we create a player.
                break;
            case Custom_msg_type.LOGIN:
                break;
            case Custom_msg_type.SEND_PLAYER_LIST:
                friend_event?.Invoke((List<connection_struct>)message);
                //ignore this for now
                break;
            case Custom_msg_type.SEND_PARTY_LIST:
                List<String> party_list = (List<string>)(message);
                if (party_list == null)
                {
                    party_list = new List<string>();
                }
                print(party_list.Aggregate(String.Concat));
                try { if(!in_game) party_event(new Party_Names(party_list.First(), party_list.Skip(1).ToList())); } catch(MissingReferenceException e) { print(e); }
                if (debug) { print($"recieved party_list"); }
                break;
            case Custom_msg_type.LEAVE_PARTY:
                break;
            case Custom_msg_type.REQ_JOIN_PARTY:
                if (debug) print($"Got a req event with {msg.arg1}");
                request_event(msg.arg1);
                break;
            case Custom_msg_type.INVITE_PLAYER:
                invite_event(msg.arg1);
                break;
            case Custom_msg_type.START_GAME:
                //message_event("your game started");
                DateTime serverTime = (DateTime)message;
                TimeSpan oneWayTrip = DateTime.Now - (serverTime.AddHours(-1));
                print(oneWayTrip.TotalSeconds);
                start_event.Invoke(Time.time);
                break;
            case Custom_msg_type.LOGOUT:
                break;
            case Custom_msg_type.MTC:
                message = unformat_bytes<object>((byte[])message);
                message_event(message);
                break;
            case Custom_msg_type.RPC:
                break;
            case Custom_msg_type.CMD:
                break;
            case Custom_msg_type.END_GAME:
                break;
            case Custom_msg_type.FIND_MATCH:
                break;
            case Custom_msg_type.ADD_FRIEND:
                break;
            case Custom_msg_type.SET_PLAYER_INFO:
                pi_event?.Invoke((player_info)message);
                break;
            case Custom_msg_type.GET_PLAYER_INFO:
                break;
            case Custom_msg_type.GET_FRIEND:
                print($"{msg.arg1} wants to be your friend");
                friend_request(msg.arg1);
                break;
        }
    }

    bool send_message(Custom_msg_type type, string arg1, string arg2, int targetConnection, object body = null,bool reliable = true,bool large = false)
    {

        byte error = 0;
        Message_package msg_p = new Message_package();
        Message_obj msg = new Message_obj();
        msg_p.type = type;
        msg.arg1 = arg1;
        msg.arg2 = arg2;
        msg.target_connection = targetConnection;
        if (body == null)
        {
            msg_p.message = msg;
        }
        else
        {
            msg_p.message = body;
        }

        byte[] data = format_data(msg_p);
        int channel = reliable_channel;
        if(!reliable)
        {
            channel = state_update_channel;
        }
        if(large)
        {
            channel = large_data_channel;
        }
        NetworkTransport.Send(host, conn_id, channel, data, data.Length, out error);
        //if(debug) print($"trying to send {type}: {(NetworkError)error} at channel: {channel}" +
        //    $" on host {host} on conn {conn_id}");
        return (NetworkError)error == NetworkError.Ok;
    }

    byte[] format_data<T>(T obj)
    {
        using (MemoryStream mem = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(mem, obj);
            return mem.ToArray();
        }
    }

    T unformat_bytes<T>(byte[] bytes)
    {
        using (MemoryStream mem = new MemoryStream(bytes))
        {
            mem.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(mem);
            return (T)obj;
        }
    }


    public bool Connect(int port, Action success, Action failure)
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliable_channel = config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);
        config.AddChannel(QosType.AllCostDelivery);
        state_update_channel = config.AddChannel(QosType.StateUpdate);
        large_data_channel = config.AddChannel(QosType.ReliableFragmentedSequenced);
        topology = new HostTopology(config, 100);

        host = NetworkTransport.AddHost(topology, 0);
        byte error;
        //progatoras is running on 15150. my ip: "71.61.58.16" localhost: "127.0.0.1"
        conn_id = NetworkTransport.Connect(host, local ? "127.0.0.1" : "71.61.58.16", 15150, 0, out error);
        if (debug) print($"connecting {(NetworkError)error}");
        StartCoroutine(Receive(success,failure));
        return (NetworkError)error == NetworkError.Ok;

    }

    public bool Create_Player(string name, string password, Action success,Action failure)
    {
        handle_data_event.e.AddListener((t, o) =>
            {
                if (t.Item1 == Custom_msg_type.CREATE_PLAYER) { if (t.Item2.target_connection == 1) success(); else failure(); }
            });
        if (debug) print($"create player with name: {name} and pass: {password}");
        return send_message(Custom_msg_type.CREATE_PLAYER, name, password, -1);
    }

    public bool End_Game()
    {
        return send_message(Custom_msg_type.END_GAME, "", null, -1);
    }


    public void Register_Party_List(Action<Party_Names> invoke)
    {
        party_event = invoke;
    }

    public bool Invite_Player(string name)
    {
        return send_message(Custom_msg_type.INVITE_PLAYER, name, null, -1);
    }

    public bool Join_Party(string name)
    {
        return send_message(Custom_msg_type.REQ_JOIN_PARTY, name, null, -1);
    }

    enum GAME { Cloak_and_Dagger };


    public bool Login(string name, string password, Action success, Action failure)
    {
        handle_data_event.e.AddListener((t, o) => 
        {
            if (t.Item1 == Custom_msg_type.LOGIN) { if (t.Item2.target_connection == 1) success(); else failure(); }
        });
        return send_message(Custom_msg_type.LOGIN, name, password, (int)GAME.Cloak_and_Dagger);
    }

    public bool Logout()
    {
        return send_message(Custom_msg_type.LOGOUT, "", null, -1);
    }

    public void Register_Receive_Invite(Action<string,Action> when_you_are_invited)
    {
        invite_event = str => when_you_are_invited(str,() => Join_Party(str));
    }

    public void Register_Receive_Request(Action<string,Action> when_someone_wants_to_join)
    {
        request_event = str => when_someone_wants_to_join(str, () => Invite_Player(str));
    }

    public bool Leave_Party()
    {
        return send_message(Custom_msg_type.LEAVE_PARTY, "", null, -1);
    }

    public void leave_party()
    {
        Leave_Party();
    }

    public bool Start_Game()
    {
        return send_message(Custom_msg_type.START_GAME, "", "", -1);
    }


    public thirds_client(string name, string password, Action<string,Action> invite_trigger, Action<string,Action> request_trigger, Action<string> message_trigger, int port)
    {
        //Setup_for_player(name, password, invite_trigger, request_trigger, message_trigger, port);
    }

    public bool Multicast(object msg,bool reliable = true, bool large = false)
    {
        msg = format_data(msg);
        return send_message(Custom_msg_type.MTC, "", "", -1,body: msg,reliable: reliable,large: large);
    }

    public void Register_Message_Receive(Action<object> when_you_receive_message)
    {
        message_event = when_you_receive_message;
    }

    public void send_info(player_info pi)
    {
        send_message(Custom_msg_type.SET_PLAYER_INFO, "", "", -1, format_data(pi), false, large: true);
    }

    public void get_player_info(Action<player_info> callback)
    {
        send_message(Custom_msg_type.GET_PLAYER_INFO, "", "", -1);
        pi_event = callback;
    }

    public void find_match()
    {
        send_message(Custom_msg_type.FIND_MATCH, "", "", -1);
    }
    


    public void Register_friend_callbacks(Action success, Action failure)
    {
        handle_data_event.e.AddListener((t, o) =>
        {
            if (t.Item1 == Custom_msg_type.ADD_FRIEND) { if (t.Item2.target_connection == 1) success(); else failure(); }
        });
    }

    public void add_friend(string name)
    {
        send_message(Custom_msg_type.ADD_FRIEND, name, "", -1);
    }
    
    public void Register_friends(Action<List<connection_struct>> callback)
    {
        friend_event = callback;
    }

    public void Register_friend_requests(Action<string> callback)
    {
        friend_request = callback;
    }
}