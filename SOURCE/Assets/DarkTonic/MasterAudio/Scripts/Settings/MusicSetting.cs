/*! \cond PRIVATE */
using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    [Serializable]
    // ReSharper disable once CheckNamespace
    public class MusicSetting {
        // ReSharper disable InconsistentNaming
        public string alias = string.Empty;
        public MasterAudio.AudioLocation audLocation = MasterAudio.AudioLocation.Clip;
        public AudioClip clip;
        public string songName = string.Empty;
        public string resourceFileName = string.Empty;
        public float volume = 1f;
        public float pitch = 1f;
        public bool isExpanded = true;
        public bool isLoop = false;
        public float customStartTime = 0f;
        public int lastKnownTimePoint = 0;
        public int songIndex = 0;
        public bool songStartedEventExpanded = false;
        public string songStartedCustomEvent = string.Empty;
        public bool songChangedEventExpanded = false;
        public string songChangedCustomEvent = string.Empty;
        // ReSharper restore InconsistentNaming
    }
}
/*! \endcond */