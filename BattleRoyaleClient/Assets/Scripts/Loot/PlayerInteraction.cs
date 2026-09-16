using System;
using UnityEngine;

namespace BattleRoyale.Loot
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Radius & Range")]
        [SerializeField] private float interactionDistance = 2.5f;
        [SerializeField] private LayerMask interactableLayers = ~0;
        [SerializeField] private Transform cameraTransform;

        private IInteractable currentTarget;

        public event Action<string> OnPromptUpdate;
        public IInteractable CurrentTarget => currentTarget;

        private void Awake()
        {
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            DetectInteractables();

            if (currentTarget != null && Input.GetKeyDown(KeyCode.F))
            {
                currentTarget.Interact(gameObject);
            }
        }

        private void DetectInteractables()
        {
            Vector3 origin = cameraTransform != null ? cameraTransform.position : transform.position + Vector3.up * 1.5f;
            Vector3 direction = cameraTransform != null ? cameraTransform.forward : transform.forward;

            IInteractable foundTarget = null;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, interactionDistance, interactableLayers))
            {
                foundTarget = hit.collider.GetComponentInParent<IInteractable>();
            }

            // Fallback sphere overlap if raycast missed ground pickup
            if (foundTarget == null)
            {
                Collider[] sphereHits = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f, 1.5f, interactableLayers);
                float closestDist = float.MaxValue;

                foreach (var col in sphereHits)
                {
                    IInteractable interactable = col.GetComponentInParent<IInteractable>();
                    if (interactable != null && interactable.CanInteract(gameObject))
                    {
                        float dist = Vector3.Distance(transform.position, col.transform.position);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            foundTarget = interactable;
                        }
                    }
                }
            }

            // Handle Target Change Focus events
            if (foundTarget != currentTarget)
            {
                currentTarget?.OnFocusExit();
                currentTarget = foundTarget;
                currentTarget?.OnFocusEnter();

                string prompt = currentTarget != null ? currentTarget.InteractionPrompt : string.Empty;
                OnPromptUpdate?.Invoke(prompt);
            }
        }
    }
}
