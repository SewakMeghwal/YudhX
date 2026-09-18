using UnityEngine;

namespace BattleRoyale.Utilities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PooledParticle : MonoBehaviour
    {
        private ParticleSystem particleSys;

        private void Awake()
        {
            particleSys = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            if (particleSys != null)
            {
                particleSys.Play();
            }
        }

        private void Update()
        {
            if (particleSys != null && !particleSys.IsAlive(true))
            {
                if (ObjectPoolManager.Instance != null)
                {
                    ObjectPoolManager.Instance.ReturnToPool(gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
