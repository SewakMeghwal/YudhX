using UnityEngine;

namespace BattleRoyale.Weapons
{
    public class WeaponRecoil
    {
        private Vector3 currentRecoil;
        private Vector3 targetRecoil;

        public Vector3 CurrentRecoil => currentRecoil;

        public void ApplyRecoil(float verticalAmount, float horizontalAmount)
        {
            float randX = UnityEngine.Random.Range(-horizontalAmount, horizontalAmount);
            targetRecoil += new Vector3(-verticalAmount, randX, 0f);
        }

        public Vector3 UpdateRecoil(float deltaTime, float returnSpeed = 10f, float snappiness = 20f)
        {
            targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, returnSpeed * deltaTime);
            currentRecoil = Vector3.Slerp(currentRecoil, targetRecoil, snappiness * deltaTime);
            return currentRecoil;
        }
    }
}
