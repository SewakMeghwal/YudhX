using UnityEngine;

namespace BattleRoyale.Utilities
{
    public interface IObjectPool
    {
        GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation);
        void ReturnToPool(GameObject instance);
        void Prewarm(GameObject prefab, int count);
    }
}
