using DarkTonic.MasterAudio;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
// ReSharper disable once CheckNamespace
public class MasterAudioHierIcon : MonoBehaviour {
    static readonly Texture2D MAicon;
	static readonly Texture2D PCicon;

    static MasterAudioHierIcon() {
		MAicon = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/MasterAudio Icon.png", typeof(Texture2D)) as Texture2D;
		PCicon = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/PlaylistController Icon.png", typeof(Texture2D)) as Texture2D;

		if (MAicon == null) {
            return;
        } 

        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        EditorApplication.RepaintHierarchyWindow();
    }

    // ReSharper disable once InconsistentNaming
    static void HierarchyItemCB(int instanceId, Rect selectionRect) {
        var masterAudioGameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

		if (masterAudioGameObject == null) {
			return;
		}   

		if (MAicon != null && masterAudioGameObject.GetComponent<MasterAudio>() != null) {
            var r = new Rect(selectionRect);
            r.x = r.width - 5;

			GUI.Label(r, MAicon);
		} else if (PCicon != null && masterAudioGameObject.GetComponent<PlaylistController>() != null) {
			var r = new Rect(selectionRect);
			r.x = r.width - 5;
			
			GUI.Label(r, PCicon); 
		}
	}
}
