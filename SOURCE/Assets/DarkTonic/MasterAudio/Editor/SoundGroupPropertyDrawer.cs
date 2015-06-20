﻿#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#else
using DarkTonic.MasterAudio;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SoundGroupAttribute))]
// ReSharper disable once CheckNamespace
public class SoundGroupPropertyDrawer : PropertyDrawer {
    // ReSharper disable once InconsistentNaming
    public int index;
    // ReSharper disable once InconsistentNaming
    public bool typeIn;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!typeIn) {
            return base.GetPropertyHeight(property, label);
        }
        return base.GetPropertyHeight(property, label) + 16;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var ma = MasterAudio.SafeInstance;
        // ReSharper disable once RedundantAssignment
        var groupName = "[Type In]";

        if (ma == null) {
            //Debug.LogError("No Master Audio prefab in Scene. Cannot use Sound Group Property Drawer.");
            index = -1;
            typeIn = false;
            property.stringValue = EditorGUI.TextField(position, label.text, property.stringValue);
            return;
        }

        index = ma.GroupNames.IndexOf(property.stringValue);

        if (typeIn || index == -1) {
            index = 0;
            typeIn = true;
            position.height -= 16;
        }

        index = EditorGUI.Popup(position, label.text, index, MasterAudio.Instance.GroupNames.ToArray());
        groupName = MasterAudio.Instance.GroupNames[index];

        switch (groupName) {
            case "[Type In]":
                typeIn = true;
                position.yMin += 16;
                position.height += 16;
                EditorGUI.BeginChangeCheck();
                property.stringValue = EditorGUI.TextField(position, label.text, property.stringValue);
                EditorGUI.EndChangeCheck();
                break;
            default:
                typeIn = false;
                property.stringValue = groupName;
                break;
        }
    }
}

#endif