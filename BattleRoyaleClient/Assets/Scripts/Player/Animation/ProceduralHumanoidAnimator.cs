using UnityEngine;

namespace BattleRoyale.Player.Animation
{
    public class ProceduralHumanoidAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private Transform legL;
        [SerializeField] private Transform legR;
        [SerializeField] private Transform armL;
        [SerializeField] private Transform armR;
        [SerializeField] private Transform rifleModel;

        private float walkCycleTimer;
        private Vector3 initialRiflePos;
        private Quaternion initialRifleRot;

        private void Start()
        {
            if (movement == null) movement = GetComponentInParent<PlayerMovement>();
            FindLimbReferencesIfNull();

            if (rifleModel != null)
            {
                initialRiflePos = rifleModel.localPosition;
                initialRifleRot = rifleModel.localRotation;
            }
        }

        private void FindLimbReferencesIfNull()
        {
            if (legL == null) legL = transform.Find("Leg_L");
            if (legR == null) legR = transform.Find("Leg_R");
            if (armL == null) armL = transform.Find("Arm_L");
            if (armR == null) armR = transform.Find("Arm_R");
            if (rifleModel == null) rifleModel = transform.Find("AR47_TacticalRifle");
        }

        private void Update()
        {
            if (movement == null) return;

            PlayerRuntimeState state = movement.CurrentState;
            float speed = new Vector3(state.Velocity.x, 0, state.Velocity.z).magnitude;

            // 1. Leg Stride Locomotion Cycle
            if (speed > 0.1f && state.IsGrounded)
            {
                float cycleSpeed = state.IsSprinting ? 14.0f : 9.0f;
                walkCycleTimer += Time.deltaTime * cycleSpeed;
                float legAngle = Mathf.Sin(walkCycleTimer) * (state.IsSprinting ? 32f : 20f);

                if (legL != null) legL.localRotation = Quaternion.Euler(legAngle, 0, 0);
                if (legR != null) legR.localRotation = Quaternion.Euler(-legAngle, 0, 0);

                if (armL != null) armL.localRotation = Quaternion.Euler(-legAngle * 0.7f, 0, 0);
            }
            else
            {
                walkCycleTimer = 0f;
                if (legL != null) legL.localRotation = Quaternion.Lerp(legL.localRotation, Quaternion.identity, 10f * Time.deltaTime);
                if (legR != null) legR.localRotation = Quaternion.Lerp(legR.localRotation, Quaternion.identity, 10f * Time.deltaTime);
                if (armL != null) armL.localRotation = Quaternion.Lerp(armL.localRotation, Quaternion.identity, 10f * Time.deltaTime);
            }

            // 2. Weapon Holding & Aim Pose
            if (armR != null)
            {
                Quaternion targetArmRot = Quaternion.Euler(-45f, 0, 0); // Aiming forward
                armR.localRotation = Quaternion.Lerp(armR.localRotation, targetArmRot, 12f * Time.deltaTime);
            }

            // Smooth Rifle Recoil Recovery
            if (rifleModel != null)
            {
                rifleModel.localPosition = Vector3.Lerp(rifleModel.localPosition, initialRiflePos, 15f * Time.deltaTime);
                rifleModel.localRotation = Quaternion.Lerp(rifleModel.localRotation, initialRifleRot, 15f * Time.deltaTime);
            }
        }

        public void TriggerWeaponRecoilKick()
        {
            if (rifleModel != null)
            {
                rifleModel.localPosition = initialRiflePos + new Vector3(0, 0.05f, -0.12f);
                rifleModel.localRotation = initialRifleRot * Quaternion.Euler(-12f, 0, 0);
            }
        }
    }
}
