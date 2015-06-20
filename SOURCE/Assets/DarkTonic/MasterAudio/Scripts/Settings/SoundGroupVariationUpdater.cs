/*! \cond PRIVATE */
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    /// <summary>
    /// This class is only activated when you need code to execute in an Update method, such as "follow" code.
    /// </summary>
    // ReSharper disable once CheckNamespace
    public class SoundGroupVariationUpdater : MonoBehaviour {
        private Transform _objectToFollow;
        private GameObject _objectToFollowGo;
        private bool _isFollowing;
        private SoundGroupVariation _variation;
        private float _priorityLastUpdated = -5f;
        private bool _useClipAgePriority;
        private WaitForSoundFinishMode _waitMode = WaitForSoundFinishMode.None;
        private float _soundPlayTime;

        // fade in out vars
        private float _fadeOutStartTime = -5;
        private bool _fadeInOutWillFadeOut;
        private bool _hasFadeInOutSetMaxVolume;
        private float _fadeInOutInFactor;
        private float _fadeInOutOutFactor;

        // fade out early vars
        private int _fadeOutEarlyTotalFrames;
        private float _fadeOutEarlyFrameVolChange;
        private int _fadeOutEarlyFrameNumber;
        private float _fadeOutEarlyOrigVol;

        // gradual fade vars
        private float _fadeToTargetFrameVolChange;
        private int _fadeToTargetFrameNumber;
        private float _fadeToTargetOrigVol;
        private int _fadeToTargetTotalFrames;
        private float _fadeToTargetVolume;
        private bool _fadeOutStarted;
        private float _lastFrameClipTime = -1f;
        private float _fxTailEndTime = -1f;
        private bool _isPlayingBackward;

        private bool _hasStartedNextInChain;

        private enum WaitForSoundFinishMode {
            None,
            Delay,
            Play,
            WaitForEnd,
            StopOrRepeat,
            FxTailWait
        }

        #region Public methods

        public void FadeOverTimeToVolume(float targetVolume, float fadeTime) {
            GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.GradualFade;

            var volDiff = targetVolume - VarAudio.volume;

            if (!VarAudio.loop && VarAudio.clip != null && fadeTime + VarAudio.time > VarAudio.clip.length) {
                // if too long, fade out faster
                fadeTime = VarAudio.clip.length - VarAudio.time;
            }

            _fadeToTargetTotalFrames = (int)(fadeTime / Time.deltaTime);
            _fadeToTargetFrameVolChange = volDiff / _fadeToTargetTotalFrames;
            _fadeToTargetFrameNumber = 0;
            _fadeToTargetOrigVol = VarAudio.volume;
            _fadeToTargetVolume = targetVolume;
        }

        public void FadeOutEarly(float fadeTime) {
            GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.FadeOutEarly;
            // cancel the FadeInOut loop, if it's going.

            if (!VarAudio.loop && VarAudio.clip != null && VarAudio.time + fadeTime > VarAudio.clip.length) {
                // if too long, fade out faster
                fadeTime = VarAudio.clip.length - VarAudio.time;
            }

            var frameTime = Time.deltaTime;
            if (frameTime == 0) {
                frameTime = Time.fixedDeltaTime;
            }

            _fadeOutEarlyTotalFrames = (int)(fadeTime / frameTime);
            _fadeOutEarlyFrameVolChange = -VarAudio.volume / _fadeOutEarlyTotalFrames;
            _fadeOutEarlyFrameNumber = 0;
            _fadeOutEarlyOrigVol = VarAudio.volume;

        }

        public void FadeInOut() {
            GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.FadeInOut;
            // wait to set this so it stops the previous one if it's still going.
            _fadeOutStartTime = VarAudio.clip.length - (GrpVariation.fadeOutTime * VarAudio.pitch);

            if (GrpVariation.fadeInTime > 0f) {
                VarAudio.volume = 0f; // start at zero volume
                _fadeInOutInFactor = GrpVariation.fadeMaxVolume / GrpVariation.fadeInTime;
            } else {
                _fadeInOutInFactor = 0f;
            }

            _fadeInOutWillFadeOut = GrpVariation.fadeOutTime > 0f && !VarAudio.loop;

            if (_fadeInOutWillFadeOut) {
                _fadeInOutOutFactor = GrpVariation.fadeMaxVolume / (VarAudio.clip.length - _fadeOutStartTime);
            } else {
                _fadeInOutOutFactor = 0f;
            }
        }

        public void FollowObject(bool follow, Transform objToFollow, bool clipAgePriority) {
            _isFollowing = follow;

            if (objToFollow != null) {
                _objectToFollow = objToFollow;
                _objectToFollowGo = objToFollow.gameObject;
            }
            _useClipAgePriority = clipAgePriority;

            UpdateAudioLocationAndPriority(false); // in case we're not following, it should get one update.
        }

        public void WaitForSoundFinish(float delaySound) {
            if (MasterAudio.IsWarming) {
                PlaySoundAndWait();
                return;
            }

            _waitMode = WaitForSoundFinishMode.Delay;

            var waitTime = 0f;

            if (GrpVariation.useIntroSilence && GrpVariation.introSilenceMax > 0f) {
                var rndSilence = Random.Range(GrpVariation.introSilenceMin, GrpVariation.introSilenceMax);
                waitTime += rndSilence;
            }

            if (delaySound > 0f) {
                waitTime += delaySound;
            }

            if (waitTime == 0f) {
                _waitMode = WaitForSoundFinishMode.Play; // skip delay mode
            } else {
                _soundPlayTime = Time.realtimeSinceStartup + waitTime;
                GrpVariation.IsWaitingForDelay = true;
            }
        }

        public void StopFading() {
            GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.None;

            DisableIfFinished();
        }

        public void StopWaitingForFinish() {
            _waitMode = WaitForSoundFinishMode.None;
            GrpVariation.curDetectEndMode = SoundGroupVariation.DetectEndMode.None;

            DisableIfFinished();
        }

        public void StopFollowing() {
            _isFollowing = false;
            _useClipAgePriority = false;
            _objectToFollow = null;
            _objectToFollowGo = null;

            DisableIfFinished();
        }

        #endregion

        #region Helper methods

        private void DisableIfFinished() {
            if (_isFollowing || GrpVariation.curDetectEndMode == SoundGroupVariation.DetectEndMode.DetectEnd ||
                GrpVariation.curFadeMode != SoundGroupVariation.FadeMode.None) {
                return;
            }

            enabled = false;
        }

        private void UpdateAudioLocationAndPriority(bool rePrioritize) {
            // update location, only if following.
            if (_isFollowing && _objectToFollow != null) {
                Trans.position = _objectToFollow.position;
            }

            // re-set priority, still used by non-following (audio clip age priority)
            if (!MasterAudio.Instance.prioritizeOnDistance || !rePrioritize || ParentGroup.alwaysHighestPriority) {
                return;
            }

            if (!(Time.realtimeSinceStartup - _priorityLastUpdated > MasterAudio.ReprioritizeTime)) {
                return;
            }
            AudioPrioritizer.Set3DPriority(VarAudio, _useClipAgePriority);
            _priorityLastUpdated = Time.realtimeSinceStartup;
        }

        private void PlaySoundAndWait() {
            GrpVariation.IsWaitingForDelay = false;
            if (VarAudio.clip == null) { // in case the warming sound is an "internet file"
                return;
            }
            VarAudio.Play();

            if (GrpVariation.useRandomStartTime) {
                var offset = Random.Range(GrpVariation.randomStartMinPercent, GrpVariation.randomStartMaxPercent) * 0.01f *
                             VarAudio.clip.length;
                VarAudio.time = offset;
            }

            GrpVariation.LastTimePlayed = Time.time;

            // sound play worked! Duck music if a ducking sound.
            MasterAudio.DuckSoundGroup(ParentGroup.GameObjectName, VarAudio);

            _isPlayingBackward = GrpVariation.OriginalPitch < 0;
            _lastFrameClipTime = _isPlayingBackward ? VarAudio.clip.length + 1 : -1f;

            _waitMode = WaitForSoundFinishMode.WaitForEnd;
        }

        private void StopOrChain() {
            var playSnd = GrpVariation.PlaySoundParm;

            var wasPlaying = playSnd.IsPlaying;
            var usingChainLoop = wasPlaying && playSnd.IsChainLoop;

            if (!VarAudio.loop || usingChainLoop) {
                GrpVariation.Stop();
            }

            if (!usingChainLoop) {
                return;
            }
            StopWaitingForFinish();

            MaybeChain();
        }

        private void MaybeChain() {
            if (_hasStartedNextInChain) {
                return;
            }

            _hasStartedNextInChain = true;

            var playSnd = GrpVariation.PlaySoundParm;

            // check if loop count is over.
            if (ParentGroup.chainLoopMode == MasterAudioGroup.ChainedLoopLoopMode.NumberOfLoops &&
                ParentGroup.ChainLoopCount >= ParentGroup.chainLoopNumLoops) {
                // done looping
                return;
            }

            var rndDelay = playSnd.DelaySoundTime;
            if (ParentGroup.chainLoopDelayMin > 0f || ParentGroup.chainLoopDelayMax > 0f) {
                rndDelay = Random.Range(ParentGroup.chainLoopDelayMin, ParentGroup.chainLoopDelayMax);
            }

            // cannot use "AndForget" methods! Chain loop needs to check the status.
            if (playSnd.AttachToSource || playSnd.SourceTrans != null) {
                if (playSnd.AttachToSource) {
                    MasterAudio.PlaySound3DFollowTransform(playSnd.SoundType, playSnd.SourceTrans,
                        playSnd.VolumePercentage, playSnd.Pitch, rndDelay, null, true);
                } else {
                    MasterAudio.PlaySound3DAtTransform(playSnd.SoundType, playSnd.SourceTrans, playSnd.VolumePercentage,
                        playSnd.Pitch, rndDelay, null, true);
                }
            } else {
                MasterAudio.PlaySound(playSnd.SoundType, playSnd.VolumePercentage, playSnd.Pitch, rndDelay, null, true);
            }
        }

        private void PerformFading() {
            switch (GrpVariation.curFadeMode) {
                case SoundGroupVariation.FadeMode.None:
                    break;
                case SoundGroupVariation.FadeMode.FadeInOut:
                    if (!VarAudio.isPlaying) {
                        break;
                    }

                    var clipTime = VarAudio.time;
                    if (GrpVariation.fadeInTime > 0f && clipTime < GrpVariation.fadeInTime) {
                        // fade in!
                        VarAudio.volume = clipTime * _fadeInOutInFactor;
                    } else if (clipTime >= GrpVariation.fadeInTime && !_hasFadeInOutSetMaxVolume) {
                        VarAudio.volume = GrpVariation.fadeMaxVolume;
                        _hasFadeInOutSetMaxVolume = true;
                        if (!_fadeInOutWillFadeOut) {
                            StopFading();
                        }
                    } else if (_fadeInOutWillFadeOut && clipTime >= _fadeOutStartTime) {
                        // fade out!
                        if (GrpVariation.PlaySoundParm.IsChainLoop && !_fadeOutStarted) {
                            MaybeChain();
                            _fadeOutStarted = true;
                        }
                        VarAudio.volume = (VarAudio.clip.length - clipTime) * _fadeInOutOutFactor;
                    }
                    break;
                case SoundGroupVariation.FadeMode.FadeOutEarly:
                    if (!VarAudio.isPlaying) {
                        break;
                    }

                    _fadeOutEarlyFrameNumber++;

                    VarAudio.volume = (_fadeOutEarlyFrameNumber * _fadeOutEarlyFrameVolChange) + _fadeOutEarlyOrigVol;

                    if (_fadeOutEarlyFrameNumber >= _fadeOutEarlyTotalFrames) {
                        GrpVariation.curFadeMode = SoundGroupVariation.FadeMode.None;
                        if (MasterAudio.Instance.stopZeroVolumeVariations) {
                            GrpVariation.Stop();
                        }
                    }

                    break;
                case SoundGroupVariation.FadeMode.GradualFade:
                    if (!VarAudio.isPlaying) {
                        break;
                    }

                    _fadeToTargetFrameNumber++;
                    if (_fadeToTargetFrameNumber >= _fadeToTargetTotalFrames) {
                        VarAudio.volume = _fadeToTargetVolume;
                        StopFading();
                    } else {
                        VarAudio.volume = (_fadeToTargetFrameNumber * _fadeToTargetFrameVolChange) + _fadeToTargetOrigVol;
                    }
                    break;
            }
        }

        #endregion

        #region MonoBehavior events

        // ReSharper disable once UnusedMember.Local
        private void OnEnable() {
            // values to be reset every time a sound plays.
            _fadeInOutWillFadeOut = false;
            _hasFadeInOutSetMaxVolume = false;
            _fadeOutStarted = false;
            _hasStartedNextInChain = false;
        }

        // ReSharper disable once UnusedMember.Local
        private void LateUpdate() {
            if (_isFollowing) {
                if (ParentGroup.targetDespawnedBehavior != MasterAudioGroup.TargetDespawnedBehavior.None) {
                    if (_objectToFollowGo == null || !DTMonoHelper.IsActive(_objectToFollowGo)) {
                        switch (ParentGroup.targetDespawnedBehavior) {
                            case MasterAudioGroup.TargetDespawnedBehavior.Stop:
                                GrpVariation.Stop();
                                break;
                            case MasterAudioGroup.TargetDespawnedBehavior.FadeOut:
                                GrpVariation.FadeOutNow(ParentGroup.despawnFadeTime);
                                break;
                        }

                        StopFollowing();
                    }
                }
            }

            // fade in out / out early etc.
            PerformFading();

            // priority
            UpdateAudioLocationAndPriority(true);

            switch (_waitMode) {
                case WaitForSoundFinishMode.None:
                    break;
                case WaitForSoundFinishMode.Delay:
                    if (Time.realtimeSinceStartup >= _soundPlayTime) {
                        _waitMode = WaitForSoundFinishMode.Play;
                    }
                    break;
                case WaitForSoundFinishMode.Play:
                    PlaySoundAndWait();
                    break;
                case WaitForSoundFinishMode.WaitForEnd:
                    var willChangeModes = false;

                    if (_isPlayingBackward) {
                        if (VarAudio.time > _lastFrameClipTime) {
                            willChangeModes = true;
                        }
                    } else {
                        if (VarAudio.time < _lastFrameClipTime) {
                            willChangeModes = true;
                        }
                    }

                    _lastFrameClipTime = VarAudio.time;

                    if (willChangeModes) {
                        if (GrpVariation.fxTailTime > 0f) {
                            _waitMode = WaitForSoundFinishMode.FxTailWait;
                            _fxTailEndTime = Time.realtimeSinceStartup + GrpVariation.fxTailTime;
                        } else {
                            _waitMode = WaitForSoundFinishMode.StopOrRepeat;
                        }
                    }
                    break;
                case WaitForSoundFinishMode.FxTailWait:
                    if (Time.realtimeSinceStartup >= _fxTailEndTime) {
                        _waitMode = WaitForSoundFinishMode.StopOrRepeat;
                    }
                    break;
                case WaitForSoundFinishMode.StopOrRepeat:
                    StopOrChain();
                    break;
            }
        }

        #endregion

        #region Properties

        private Transform Trans {
            get { return GrpVariation.Trans; }
        }

        private AudioSource VarAudio {
            get { return GrpVariation.VarAudio; }
        }

        private MasterAudioGroup ParentGroup {
            get { return GrpVariation.ParentGroup; }
        }

        private SoundGroupVariation GrpVariation {
            get {
                if (_variation != null) {
                    return _variation;
                }
                _variation = GetComponent<SoundGroupVariation>();

                return _variation;
            }
        }

        #endregion
    }
}
/*! \endcond */