using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleRoyale.UI.Mobile
{
    public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }

        public event Action OnPressed;
        public event Action OnReleased;

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            OnReleased?.Invoke();
        }

        private void OnDisable()
        {
            IsPressed = false;
        }
    }
}
