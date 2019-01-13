using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ========== Status dictionaries ==========
[CustomPropertyDrawer(typeof(Status_BoolVar_Dict))]
[CustomPropertyDrawer(typeof(Status_EventObject_Dict))]
[CustomPropertyDrawer(typeof(Status_FloatEventObject_Dict))]
[CustomPropertyDrawer(typeof(Status_Float_Dict))]
[CustomPropertyDrawer(typeof(Status_FloatVar_Dict))]
// ============= Event dicts ===========================
[CustomPropertyDrawer(typeof(mtc_object_event_dict))]
[CustomPropertyDrawer(typeof(Animation_Trigger_Dict))]
[CustomPropertyDrawer(typeof(Int_var_to_string))]
[CustomPropertyDrawer(typeof(Float_var_to_string))]
[CustomPropertyDrawer(typeof(Bool_var_to_string))]
public class Custom_AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

//[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class Custom_AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }