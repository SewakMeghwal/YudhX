using System;
using UnityEngine;
using UnityEngine.AI;
using BattleRoyale.HealthSystem;
using BattleRoyale.InventorySystem;
using BattleRoyale.Weapons;
using BattleRoyale.Zone;

namespace BattleRoyale.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(PlayerWeaponInventory))]
    [RequireComponent(typeof(BotSensor))]
    public class BotController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private BotConfig config;

        private NavMeshAgent navAgent;
        private PlayerHealth health;
        private PlayerWeaponInventory weaponInventory;
        private BotSensor sensor;
        private SafeZoneController zoneController;

        private BotStateType currentState = BotStateType.Patrol;
        private Transform currentTarget;
        private float stateTimer;
        private float fireTimer;

        public BotStateType CurrentState => currentState;

        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<PlayerHealth>();
            weaponInventory = GetComponent<PlayerWeaponInventory>();
            sensor = GetComponent<BotSensor>();
            zoneController = FindObjectOfType<SafeZoneController>();
        }

        private void Start()
        {
            if (config != null)
            {
                sensor.Initialize(config);
            }
        }

        private void Update()
        {
            if (health != null && (health.IsDead || health.IsDowned))
            {
                currentState = BotStateType.Dead;
                if (navAgent.enabled) navAgent.isStopped = true;
                return;
            }

            // 1. Check Safe Zone Flee Condition
            if (zoneController != null && !zoneController.IsInsideSafeZone(transform.position))
            {
                currentState = BotStateType.FleeingZone;
                FleeToSafeZone();
                return;
            }

            // 2. Scan for Enemies
            currentTarget = sensor.ScanForEnemies();

            if (currentTarget != null)
            {
                currentState = BotStateType.Engaging;
                EngageTarget(currentTarget);
            }
            else
            {
                currentState = BotStateType.Patrol;
                PatrolWander();
            }
        }

        private void PatrolWander()
        {
            if (navAgent == null || !navAgent.enabled) return;

            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0f || navAgent.remainingDistance <= 0.5f)
            {
                stateTimer = UnityEngine.Random.Range(4.0f, 8.0f);
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * config.WanderRadius;
                randomDirection += transform.position;

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, config.WanderRadius, NavMesh.AllAreas))
                {
                    navAgent.SetDestination(hit.position);
                }
            }
        }

        private void EngageTarget(Transform target)
        {
            if (navAgent == null || target == null) return;

            float distance = Vector3.Distance(transform.position, target.position);
            navAgent.SetDestination(target.position);

            // Turn towards target smoothly
            Vector3 lookDirection = (target.position - transform.position).normalized;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 10f);
            }

            // Firing logic
            Weapon activeWeapon = weaponInventory.ActiveWeapon;
            if (activeWeapon != null && distance <= config.DetectionRadius)
            {
                fireTimer += Time.deltaTime;
                if (fireTimer >= activeWeapon.Data.FireInterval)
                {
                    fireTimer = 0f;

                    Vector3 shootDirection = (target.position + Vector3.up * 1.2f) - transform.position;
                    // Apply bot inaccuracy spread
                    shootDirection += UnityEngine.Random.insideUnitSphere * config.AimInaccuracySpread * 0.1f;

                    activeWeapon.TryFire(transform.position + Vector3.up * 1.5f, shootDirection.normalized, false, ~0, out _);
                }
            }
        }

        private void FleeToSafeZone()
        {
            if (navAgent == null || zoneController == null) return;

            navAgent.SetDestination(zoneController.CurrentCenter);
        }
    }
}
