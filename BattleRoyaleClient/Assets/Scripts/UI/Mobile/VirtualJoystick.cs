using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleRoyale.UI.Mobile
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform containerRect;
        [SerializeField] private RectTransform handleRect;
        [SerializeField] private float handleRange = 75.0f;

        private Vector2 inputVector = Vector2.zero;

        public Vector2 InputVector => inputVector;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (containerRect == null || handleRect == null) return;

            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                containerRect,
                eventData.position,
                eventData.pressEventCamera,
                out position))
            {
                position.x = (position.x / containerRect.sizeDelta.x);
                position.y = (position.y / containerRect.sizeDelta.y);

                inputVector = new Vector2(position.x * 2f, position.y * 2f);
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

                handleRect.anchoredPosition = new Vector2(
                    inputVector.x * (containerRect.sizeDelta.x / 2f) * (handleRange / 100f),
                    inputVector.y * (containerRect.sizeDelta.y / 2f) * (handleRange / 100f)
                );
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputVector = Vector2.zero;
            if (handleRect != null)
            {
                handleRect.anchoredPosition = Vector2.zero;
            }
        }
    }
}
