using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(event_object))]
public class event_object_property_Drawer : Editor
{
    public override void OnInspectorGUI()
    { 
        event_object my_event = (event_object)target;

        if (GUILayout.Button("Press Me!"))
        {
            my_event.Invoke();
        }
    }
}

