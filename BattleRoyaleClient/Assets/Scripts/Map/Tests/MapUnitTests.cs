using NUnit.Framework;
using UnityEngine;
using BattleRoyale.Map;

namespace BattleRoyale.Tests
{
    public class MapUnitTests
    {
        [Test]
        public void MapConfig_DefaultBounds_ContainCenter()
        {
            var config = ScriptableObject.CreateInstance<MapConfig>();

            Assert.IsTrue(config.WorldBounds.Contains(Vector3.zero));
            Assert.AreEqual(500f, config.MapSizeMeters);
        }

        [Test]
        public void PoiLocation_IsInsidePOI_CalculatesRadius()
        {
            var gameObject = new GameObject("TestPOI");
            var poi = gameObject.AddComponent<PoiLocation>();

            // Set POI at origin with 50m radius
            bool inside = poi.IsInsidePOI(new Vector3(10f, 0f, 10f));
            bool outside = poi.IsInsidePOI(new Vector3(100f, 0f, 0f));

            Assert.IsTrue(inside);
            Assert.IsFalse(outside);

            Object.DestroyImmediate(gameObject);
        }
    }
}
