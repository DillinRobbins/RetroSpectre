using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(InteractableBase))]
public class InteractableBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("WARNING: InteractableWarning.cs is an inheritable class all other interactables derive from." +
            "\n\nPlease create a copy of InteractableGeneric.cs with a different name and attach it to your interactable object instead.", MessageType.Warning);
    }
}

[CustomEditor (typeof(Blank_Interactable))]
public class InteractableGenericEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("WARNING: InteractableGeneric.cs is a generic version of this script." +
            "\n\nSince this script requires unique variables on a per-case basis, please create a copy of InteractableGeneric.cs with a different name and attach it to your interactable object instead.", MessageType.Warning);
    }
}

