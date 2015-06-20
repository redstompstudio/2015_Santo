using UnityEngine;

#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
// Undo API 
#else
// ReSharper disable once RedundantUsingDirective
using UnityEditor;
#endif

// ReSharper disable once CheckNamespace
public static class AudioUndoHelper {
    public static void CreateObjectForUndo(GameObject go, string actionName) {
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
        // No Undo API 
#else
        // New Undo API
        Undo.RegisterCreatedObjectUndo(go, actionName);
#endif
    }

    public static void SetTransformParentForUndo(Transform child, Transform newParent, string name) {
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
        // No Undo API 
#else
        // New Undo API
        Undo.SetTransformParent(child, newParent, name);
#endif
    }

    // ReSharper disable once RedundantAssignment
    public static void RecordObjectPropertyForUndo(ref bool isDirty, Object objectProperty, string actionName) {
        isDirty = true;

#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
        // No Undo API 
#else
        // New Undo API
        Undo.RecordObject(objectProperty, actionName);
#endif
    }

    public static void RecordObjectsForUndo(Object[] objects, string actionName) {
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
        // No Undo API 
#else
        // New Undo API
        Undo.RecordObjects(objects, actionName);
#endif
    }

    public static void DestroyForUndo(GameObject go) {
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
        // ReSharper disable once AccessToStaticMemberViaDerivedType
        GameObject.DestroyImmediate(go);
#else
        // New Undo API
        Undo.DestroyObjectImmediate(go);
#endif
    }
}
