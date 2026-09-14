using UnityEngine;

namespace BattleRoyale.Player.Animation
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepEventListener : MonoBehaviour
    {
        [SerializeField] private FootstepAudioConfig audioConfig;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Transform feetTransform;
        [SerializeField] private LayerMask groundMask = ~0;

        private void Awake()
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (feetTransform == null) feetTransform = transform;
        }

        // Animation Event Callback
        public void OnFootstep()
        {
            if (audioConfig == null || audioSource == null) return;

            string surfaceTag = DetectSurfaceTag();
            AudioClip clip = audioConfig.GetRandomFootstep(surfaceTag);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip, audioConfig.BaseVolume);
            }
        }

        // Animation Event Callback
        public void OnLand()
        {
            if (audioConfig == null || audioSource == null) return;

            string surfaceTag = DetectSurfaceTag();
            AudioClip clip = audioConfig.GetLandClip(surfaceTag);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip, audioConfig.BaseVolume * 1.2f);
            }
        }

        private string DetectSurfaceTag()
        {
            if (Physics.Raycast(feetTransform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, 0.6f, groundMask))
            {
                return hit.collider.tag;
            }
            return "Untagged";
        }
    }
}
