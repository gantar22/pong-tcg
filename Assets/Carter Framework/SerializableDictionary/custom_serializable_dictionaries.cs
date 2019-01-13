using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ========== Status dictionaries ==========
[System.Serializable]
public class Status_BoolVar_Dict : SerializableDictionary<status, player_bool> { }

[System.Serializable]
public class Status_EventObject_Dict : SerializableDictionary<status, int_event_object> { }

[System.Serializable]
public class Status_FloatEventObject_Dict : SerializableDictionary<status, int_float_event> { }

[System.Serializable]
public class Status_Float_Dict : SerializableDictionary<status, player_float> { }

[System.Serializable]
public class Animation_Trigger_Dict : SerializableDictionary<event_object,string> { }

[System.Serializable]
public class Int_var_to_string : SerializableDictionary<int_var,string> { }

[System.Serializable]
public class Bool_var_to_string : SerializableDictionary<bool_var, string> { }

[System.Serializable]
public class Float_var_to_string : SerializableDictionary<float_var, string> { }

[System.Serializable]
public class Status_FloatVar_Dict : SerializableDictionary<status, float_var> { }

