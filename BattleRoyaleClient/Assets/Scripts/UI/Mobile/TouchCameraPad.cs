using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleRoyale.UI.Mobile
{
    public class TouchCameraPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private float sensitivity = 0.2f;

        private Vector2 lookDelta;
        private Vector2 lastPointerPosition;
        private bool isDragging;

        public Vector2 LookDelta => lookDelta;

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            lastPointerPosition = eventData.position;
            lookDelta = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Vector2 currentPos = eventData.position;
            lookDelta = (currentPos - lastPointerPosition) * sensitivity;
            lastPointerPosition = currentPos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            lookDelta = Vector2.zero;
        }

        private void LateUpdate()
        {
            if (!isDragging)
            {
                lookDelta = Vector2.zero;
            }
        }
    }
}
