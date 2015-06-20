/*! \cond PRIVATE */
using System;
using System.Collections.Generic;

#if UNITY_5
    using UnityEngine.Audio;
#endif

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    [Serializable]
    // ReSharper disable once CheckNamespace
    public class GroupBus {
        // ReSharper disable InconsistentNaming
        public string busName;
        public float volume = 1.0f;
        public bool isSoloed = false;
        public bool isMuted = false;
        public int voiceLimit = -1;
        public bool stopOldest = false;
        public bool isExisting = false; // for Dynamic Sound Group - referenced Buses

#if UNITY_5
        public AudioMixerGroup mixerChannel = null;
#endif
        // ReSharper restore InconsistentNaming

        private readonly List<int> _activeAudioSourcesIds = new List<int>(50);

        public void AddActiveAudioSourceId(int id) {
            if (_activeAudioSourcesIds.Contains(id)) {
                return;
            }

            _activeAudioSourcesIds.Add(id);
        }

        public void RemoveActiveAudioSourceId(int id) {
            _activeAudioSourcesIds.Remove(id);
        }

        public int ActiveVoices {
            get { return _activeAudioSourcesIds.Count; }
        }

        public bool BusVoiceLimitReached {
            get {
                if (voiceLimit <= 0) {
                    return false; // no limit set
                }

                return _activeAudioSourcesIds.Count >= voiceLimit;
            }
        }
    }
}
/*! \endcond */