#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#else
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    /// <summary>
    /// This attribute can be used in public string fields in your custom scripts. It will show a dropdown of all Sound Groups in the Scene.
    /// </summary>
    public class SoundGroupAttribute : PropertyAttribute {
    }
}

#endif
