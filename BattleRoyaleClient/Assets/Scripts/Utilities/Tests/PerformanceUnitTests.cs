using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Utilities;

namespace BattleRoyale.Tests
{
    public class PerformanceUnitTests
    {
        [Test]
        public void ObjectPoolManager_PrewarmAndGet_ReusesInstance()
        {
            var poolObj = new GameObject("TestPoolManager");
            var pool = poolObj.AddComponent<ObjectPoolManager>();

            var prefab = new GameObject("TestPrefab");

            // Prewarm pool with 3 objects
            pool.Prewarm(prefab, 3);

            // Acquire object from pool
            GameObject instance = pool.Get(prefab, Vector3.zero, Quaternion.identity);

            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.activeSelf);

            // Return to pool
            pool.ReturnToPool(instance);
            Assert.IsFalse(instance.activeSelf);

            Object.DestroyImmediate(prefab);
            Object.DestroyImmediate(poolObj);
        }

        [Test]
        public void PerformanceProfiler_MemoryCalculation_IsPositive()
        {
            var profilerObj = new GameObject("TestProfiler");
            var profiler = profilerObj.AddComponent<PerformanceProfiler>();

            float mem = System.GC.GetTotalMemory(false) / (1024.0f * 1024.0f);
            Assert.Greater(mem, 0f, "Allocated memory should be greater than zero");

            Object.DestroyImmediate(profilerObj);
        }
    }
}
