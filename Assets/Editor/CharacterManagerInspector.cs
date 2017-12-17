using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterManager))]
public class CharacterManagerInspector : Editor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorList.Show(serializedObject.FindProperty("characters"));
        serializedObject.ApplyModifiedProperties();
    }

}
