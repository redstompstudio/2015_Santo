/*! \cond PRIVATE */
using System.Collections.Generic;
using UnityEngine;

#if UNITY_5
// ReSharper disable once RedundantUsingDirective
	using UnityEngine.Audio;
#endif

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    // ReSharper disable once CheckNamespace
    public class MasterAudioGroup : MonoBehaviour {
        public const float UseCurveSpatialBlend = -99f;
        public const string NoBus = "[NO BUS]";

        // ReSharper disable InconsistentNaming
        public int busIndex = -1;

#if UNITY_5
        public MasterAudio.ItemSpatialBlendType spatialBlendType = MasterAudio.ItemSpatialBlendType.ForceTo3D;
		public float spatialBlend = 1f;
#endif

        public bool isSelected = false;
        public bool isExpanded = true;
        public float groupMasterVolume = 1f;
        public int retriggerPercentage = 50;
        public VariationMode curVariationMode = VariationMode.Normal;
        public bool alwaysHighestPriority = false;

        public float chainLoopDelayMin;
        public float chainLoopDelayMax;
        public ChainedLoopLoopMode chainLoopMode = ChainedLoopLoopMode.Endless;
        public int chainLoopNumLoops = 0;
        public bool useDialogFadeOut = false;
        public float dialogFadeOutTime = .5f;

        public VariationSequence curVariationSequence = VariationSequence.Randomized;
        public bool useInactivePeriodPoolRefill = false;
        public float inactivePeriodSeconds = 5f;
        public List<SoundGroupVariation> groupVariations = new List<SoundGroupVariation>();
        public MasterAudio.AudioLocation bulkVariationMode = MasterAudio.AudioLocation.Clip;
        public bool resourceClipsAllLoadAsync = true;
        public bool logSound = false;

        public bool copySettingsExpanded = false;
        public int selectedVariationIndex = 0;

        public ChildGroupMode childGroupMode = ChildGroupMode.None;
        public List<string> childSoundGroups = new List<string>();

        public LimitMode limitMode = LimitMode.None;
        public int limitPerXFrames = 1;
        public float minimumTimeBetween = 0.1f;
        public bool useClipAgePriority = false;

        public bool limitPolyphony = false;
        public int voiceLimitCount = 1;

        public TargetDespawnedBehavior targetDespawnedBehavior = TargetDespawnedBehavior.None;
        public float despawnFadeTime = 1f;

        public bool isSoloed = false;
        public bool isMuted = false;

        public bool soundPlayedEventActive = false;
        public string soundPlayedCustomEvent = string.Empty;
        // ReSharper restore InconsistentNaming

        private List<int> _activeAudioSourcesIds;
        private string _objectName = string.Empty;
        private Transform _trans;
        private int _childCount;

        public enum ChildGroupMode {
            None,
            TriggerLinkedGroupsWhenRequested,
            TriggerLinkedGroupsWhenPlayed
        }

        public enum TargetDespawnedBehavior {
            None,
            Stop,
            FadeOut
        }

        public enum VariationSequence {
            Randomized,
            TopToBottom
        }

        public enum VariationMode {
            Normal,
            LoopedChain,
            Dialog
        }

        public enum ChainedLoopLoopMode {
            Endless,
            NumberOfLoops
        }

        public enum LimitMode {
            None,
            FrameBased,
            TimeBased
        }

        public int ActiveVoices {
            get { return ActiveAudioSourceIds.Count; }
        }

        public int TotalVoices {
            get { return transform.childCount; }
        }

        public MasterAudio.InternetFileLoadStatus GroupLoadStatus {
            get {
                var groupStatus = MasterAudio.InternetFileLoadStatus.Loaded;

                for (var i = 0; i < Trans.childCount; i++) {
                    var aVar = Trans.GetChild(i).GetComponent<SoundGroupVariation>();
                    if (aVar.audLocation != MasterAudio.AudioLocation.FileOnInternet) {
                        continue;
                    }

                    if (aVar.internetFileLoadStatus == MasterAudio.InternetFileLoadStatus.Failed) {
                        groupStatus = MasterAudio.InternetFileLoadStatus.Failed;
                        break;
                    }

                    if (aVar.internetFileLoadStatus == MasterAudio.InternetFileLoadStatus.Loading) {
                        groupStatus = MasterAudio.InternetFileLoadStatus.Loading;
                    }
                }

                return groupStatus;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void Start() {
            // time to rename!
            _objectName = name;
            var childCount = ActiveAudioSourceIds.Count; // time to create clones
            if (childCount > 0) {
            } // to get rid of warning

            var needsUpgrade = false;

            for (var i = 0; i < Trans.childCount; i++) {
                var variation = Trans.GetChild(i).GetComponent<SoundGroupVariation>();
                if (variation == null) {
                    continue;
                }

                var updater = variation.GetComponent<SoundGroupVariationUpdater>();
                if (updater != null) {
                    continue;
                }
                needsUpgrade = true;
                break;
            }

            if (!needsUpgrade) {
                return;
            }

            Debug.LogError("One or more Variations of Sound Group '" + GameObjectName +
                           "' do not have the SoundGroupVariationUpdater component and will not function properly. Please stop and fix this by opening the Master Audio Manager window and clicking the Upgrade MA Prefab button before continuing.");
        }

        // ReSharper disable once UnusedMember.Local
        void OnDisable() {
            for (var i = 0; i < Trans.childCount; i++) {
                var aVar = Trans.GetChild(i).GetComponent<SoundGroupVariation>();
                if (aVar == null) {
                    continue;
                }

                if (aVar.audLocation != MasterAudio.AudioLocation.FileOnInternet) {
                    continue;
                }

                AudioResourceOptimizer.RemoveLoadedInternetClip(aVar.internetFileUrl);
            }
        }

        public void AddActiveAudioSourceId(int varInstanceId) {
            if (ActiveAudioSourceIds.Contains(varInstanceId)) {
                return;
            }

            ActiveAudioSourceIds.Add(varInstanceId);

            var bus = BusForGroup;
            if (bus != null) {
                bus.AddActiveAudioSourceId(varInstanceId);
            }
        }

        public void RemoveActiveAudioSourceId(int varInstanceId) {
            ActiveAudioSourceIds.Remove(varInstanceId);

            var bus = BusForGroup;
            if (bus != null) {
                bus.RemoveActiveAudioSourceId(varInstanceId);
            }
        }

#if UNITY_5
        public float SpatialBlendForGroup {
			get {
				switch (MasterAudio.Instance.mixerSpatialBlendType) {
					case MasterAudio.AllMixerSpatialBlendType.ForceAllTo2D:
						return MasterAudio.SpatialBlend_2DValue;	
					case MasterAudio.AllMixerSpatialBlendType.ForceAllTo3D:
				        return MasterAudio.SpatialBlend_3DValue;	
					case MasterAudio.AllMixerSpatialBlendType.ForceAllToCustom:
						return MasterAudio.Instance.mixerSpatialBlend;
				    // ReSharper disable once RedundantCaseLabel
					case MasterAudio.AllMixerSpatialBlendType.AllowDifferentPerGroup:
					default:
						switch (spatialBlendType) {
							case MasterAudio.ItemSpatialBlendType.ForceTo2D:
								return MasterAudio.SpatialBlend_2DValue;
							case MasterAudio.ItemSpatialBlendType.ForceTo3D:
								return MasterAudio.SpatialBlend_3DValue;
							case MasterAudio.ItemSpatialBlendType.ForceToCustom:
                                return spatialBlend;
						    // ReSharper disable once RedundantCaseLabel
							case MasterAudio.ItemSpatialBlendType.UseCurveFromAudioSource:
                            default:
                                return UseCurveSpatialBlend;
						}
				}
			}
		}
#endif

        public GroupBus BusForGroup {
            get {
                if (busIndex < MasterAudio.HardCodedBusOptions || !Application.isPlaying) {
                    return null; // no bus, so no voice limit
                }

                var index = busIndex - MasterAudio.HardCodedBusOptions;

                if (index >= MasterAudio.GroupBuses.Count) {
                    // this happens only with Dynamic SGC item removal
                    return null;
                }

                return MasterAudio.GroupBuses[index];
            }
        }

        public int ChainLoopCount { get; set; }

        public string GameObjectName {
            get {
                if (string.IsNullOrEmpty(_objectName)) {
                    _objectName = name;
                }

                return _objectName;
            }
        }

        private Transform Trans {
            get {
                if (_trans != null) {
                    return _trans;
                }
                _trans = transform;

                return _trans;
            }
        }

        private List<int> ActiveAudioSourceIds {
            get {
                if (_activeAudioSourcesIds != null) {
                    return _activeAudioSourcesIds;
                }
                _activeAudioSourcesIds = new List<int>(Trans.childCount);

                return _activeAudioSourcesIds;
            }
        }
    }
}
/*! \endcond */