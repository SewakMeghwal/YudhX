using UnityEngine;

namespace BattleRoyale.Loot
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        bool CanInteract(GameObject interactor);
        void Interact(GameObject interactor);
        void OnFocusEnter();
        void OnFocusExit();
    }
}
