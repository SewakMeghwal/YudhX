using System;
using UnityEngine;

namespace BattleRoyale.Player.Animation
{
    [Serializable]
    public struct SurfaceAudioEntry
    {
        public string SurfaceTag;
        public AudioClip[] FootstepClips;
        public AudioClip LandClip;
    }

    [CreateAssetMenu(fileName = "FootstepAudioConfig", menuName = "BattleRoyale/Audio/Footstep Audio Config")]
    public class FootstepAudioConfig : ScriptableObject
    {
        [SerializeField] private SurfaceAudioEntry[] surfaceEntries;
        [SerializeField] private AudioClip[] defaultFootstepClips;
        [SerializeField] private AudioClip defaultLandClip;
        [Range(0f, 1f)] [SerializeField] private float baseVolume = 0.7f;

        public float BaseVolume => baseVolume;

        public AudioClip GetRandomFootstep(string tag)
        {
            if (surfaceEntries != null)
            {
                foreach (var entry in surfaceEntries)
                {
                    if (entry.SurfaceTag.Equals(tag, StringComparison.OrdinalIgnoreCase) && entry.FootstepClips != null && entry.FootstepClips.Length > 0)
                    {
                        return entry.FootstepClips[UnityEngine.Random.Range(0, entry.FootstepClips.Length)];
                    }
                }
            }

            if (defaultFootstepClips != null && defaultFootstepClips.Length > 0)
            {
                return defaultFootstepClips[UnityEngine.Random.Range(0, defaultFootstepClips.Length)];
            }

            return null;
        }

        public AudioClip GetLandClip(string tag)
        {
            if (surfaceEntries != null)
            {
                foreach (var entry in surfaceEntries)
                {
                    if (entry.SurfaceTag.Equals(tag, StringComparison.OrdinalIgnoreCase) && entry.LandClip != null)
                    {
                        return entry.LandClip;
                    }
                }
            }

            return defaultLandClip;
        }
    }
}
