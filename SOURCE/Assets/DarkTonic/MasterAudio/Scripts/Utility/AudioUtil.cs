using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    /// <summary>
    /// This class contains frequently used methods for audio in general.
    /// </summary>
    // ReSharper disable once CheckNamespace
    public static class AudioUtil {
        private const float SemitonePitchChangeAmt = 1.0594635f;

        /*! \cond PRIVATE */
        public static float FrameTime() {
            var frame = Time.deltaTime;

            if (frame == 0) {
                return Time.fixedDeltaTime;
            }

            return frame;
        }

        public static float GetSemitonesFromPitch(float pitch) {
            float pitchSemitones;

            if (pitch < 1f && pitch > 0) {
                var pitchBelow = 1 / pitch;
                pitchSemitones = Mathf.Log(pitchBelow, SemitonePitchChangeAmt) * -1;
            } else {
                pitchSemitones = Mathf.Log(pitch, SemitonePitchChangeAmt);
            }

            return pitchSemitones;
        }

        public static float GetPitchFromSemitones(float semitones) {
            if (semitones >= 0) {
                return Mathf.Pow(SemitonePitchChangeAmt, semitones);
            }

            var newPitch = 1 / Mathf.Pow(SemitonePitchChangeAmt, Mathf.Abs(semitones));
            return newPitch;
        }

        public static float GetDbFromFloatVolume(float vol) {
            return Mathf.Log(vol) * 20;
        }

        public static float GetFloatVolumeFromDb(float db) {
            return Mathf.Exp(db / 20);
        }
        /*! \endcond */

        /// <summary>
        /// This method will tell you the percentage of the clip that is done Playing (0-100).
        /// </summary>
        /// <param name="source">The Audio Source to calculate for.</param>
        /// <returns>(0-100 float)</returns>
        public static float GetAudioPlayedPercentage(AudioSource source) {
            if (source.clip == null || source.time == 0f) {
                return 0f;
            }

            var playedPercentage = (source.time / source.clip.length) * 100;
            return playedPercentage;
        }

        /// <summary>
        /// This method returns whether an AudioSource is paused or not.
        /// </summary>
        /// <param name="source">The Audio Source in question.</param>
        /// <returns>True or false</returns>
        public static bool IsAudioPaused(AudioSource source) {
            return !source.isPlaying && GetAudioPlayedPercentage(source) > 0f;
        }

        /*! \cond PRIVATE */
        public static bool IsClipReadyToPlay(AudioClip clip) {
#if UNITY_5
            return clip.loadType != AudioClipLoadType.Streaming;
#else
            return clip.isReadyToPlay;
#endif
        }
        /*! \endcond */
    }
}