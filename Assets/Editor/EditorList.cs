using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public static class EditorList {

    public static void Show(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;
        if (list.isExpanded)
        {
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));

            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));

                SerializedProperty MyListRef = list.GetArrayElementAtIndex(i);
                EditorGUI.indentLevel += 1;

                if (list.GetArrayElementAtIndex(i).isExpanded)
                {
                    SerializedProperty Model = MyListRef.FindPropertyRelative("Model");
                    EditorGUILayout.PropertyField(Model);

                    if(Model.isExpanded)
                    {
                        EditorGUILayout.PropertyField(Model.FindPropertyRelative("mainObject"));
                        EditorGUILayout.PropertyField(Model.FindPropertyRelative("Body"));
                        EditorGUILayout.PropertyField(Model.FindPropertyRelative("Larm"));
                        EditorGUILayout.PropertyField(Model.FindPropertyRelative("Rarm"));
                        EditorGUILayout.PropertyField(Model.FindPropertyRelative("Lleg"));
                        EditorGUILayout.PropertyField(Model.FindPropertyRelative("Rleg"));
                    }

                    SerializedProperty Effects = MyListRef.FindPropertyRelative("Effects");
                    EditorGUILayout.PropertyField(Effects);

                    if (Effects.isExpanded)
                    {
                        EditorGUI.indentLevel += 1;

                        EditorGUILayout.PropertyField(Effects.FindPropertyRelative("Array.size"));

                        for (int j = 0; j < Effects.arraySize; j++)
                        {  

                            EditorGUILayout.PropertyField(Effects.GetArrayElementAtIndex(j));
                            SerializedProperty EffectsListRef = Effects.GetArrayElementAtIndex(j);

                            SerializedProperty effectName = EffectsListRef.FindPropertyRelative("name");
                            SerializedProperty effectDisc = EffectsListRef.FindPropertyRelative("duration");

                            if (EffectsListRef.isExpanded)
                            {
                                EditorGUILayout.PropertyField(effectName);
                                EditorGUILayout.PropertyField(effectDisc);
                            }
                        }

                     
                    }

                    EditorGUI.indentLevel -= 1;

                    EditorGUILayout.PropertyField(MyListRef.FindPropertyRelative("name"));
                    EditorGUILayout.PropertyField(MyListRef.FindPropertyRelative("Disc"));
                }
                EditorGUI.indentLevel -= 1;
            }
        }
        EditorGUI.indentLevel -= 1;
    }
}
