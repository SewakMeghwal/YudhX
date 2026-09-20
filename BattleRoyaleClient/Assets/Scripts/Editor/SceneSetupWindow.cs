using System.IO;
using UnityEngine;
using BattleRoyale.Player;
using BattleRoyale.CameraSystem;
using BattleRoyale.HealthSystem;
using BattleRoyale.Weapons;
using BattleRoyale.InventorySystem;
using BattleRoyale.Loot;
using BattleRoyale.Map;
using BattleRoyale.Zone;
using BattleRoyale.AI;
using BattleRoyale.Multiplayer;
using BattleRoyale.Multiplayer.Combat;
using BattleRoyale.Multiplayer.Inventory;
using BattleRoyale.Multiplayer.AntiCheat;
using BattleRoyale.Multiplayer.Server;
using BattleRoyale.Match;
using BattleRoyale.Backend;
using BattleRoyale.Backend.Stats;
using BattleRoyale.Utilities;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace BattleRoyale.Editor
{
    public static class SceneSetupWindow
    {
        [MenuItem("BattleRoyale/Build Battle Royale Scene (MainMap)")]
        public static void GenerateMainMapScene()
        {
            // 1. Create New Scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // 2. Ensure Folders Exist
            EnsureDirectory("Assets/Scenes");
            EnsureDirectory("Assets/ScriptableObjects");

            // 3. Generate ScriptableObjects
            var moveSettings = CreateAsset<MovementSettings>("Assets/ScriptableObjects/PlayerMovementSettings.asset");
            var camSettings = CreateAsset<CameraSettings>("Assets/ScriptableObjects/CameraSettings.asset");
            var zoneSettings = CreateAsset<ZoneSettings>("Assets/ScriptableObjects/ZoneSettings.asset");
            var netConfig = CreateAsset<NetworkConfig>("Assets/ScriptableObjects/NetworkConfig.asset");
            var backendConfig = CreateAsset<BackendConfig>("Assets/ScriptableObjects/BackendConfig.asset");
            var botConfig = CreateAsset<BotConfig>("Assets/ScriptableObjects/BotConfig.asset");
            var antiCheatSettings = CreateAsset<AntiCheatSettings>("Assets/ScriptableObjects/AntiCheatSettings.asset");
            var matchConfig = CreateAsset<MatchConfig>("Assets/ScriptableObjects/MatchConfig.asset");

            // 4. Create Ground Environment (500m x 500m)
            var groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundObj.name = "GroundTerrain_500m";
            groundObj.transform.position = Vector3.zero;
            groundObj.transform.localScale = new Vector3(50f, 1f, 50f); // 500m x 500m plane

            // 5. Create MapManager & Spawn Points
            var mapMgrObj = new GameObject("MapManager");
            var mapMgr = mapMgrObj.AddComponent<MapManager>();

            for (int i = 0; i < 8; i++)
            {
                float angle = i * (360f / 8f) * Mathf.Deg2Rad;
                Vector3 spawnPos = new Vector3(Mathf.Cos(angle) * 200f, 1f, Mathf.Sin(angle) * 200f);
                var spawnObj = new GameObject($"SpawnPoint_{i + 1}");
                spawnObj.transform.SetParent(mapMgrObj.transform);
                spawnObj.transform.position = spawnPos;
                spawnObj.transform.rotation = Quaternion.LookRotation(-spawnPos.normalized);
                spawnObj.AddComponent<SpawnPoint>();
            }

            // Create POIs
            CreatePOI(mapMgrObj.transform, "Central Outpost", Vector3.zero, 50f);
            CreatePOI(mapMgrObj.transform, "North Airfield", new Vector3(0, 0, 150f), 45f);
            CreatePOI(mapMgrObj.transform, "South Docks", new Vector3(0, 0, -150f), 45f);

            // 6. Create Player Capsule GameObject
            var playerObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObj.name = "Player";
            playerObj.transform.position = new Vector3(0, 1.5f, 0);

            var charController = playerObj.GetComponent<CharacterController>() ?? playerObj.AddComponent<CharacterController>();
            charController.height = 2.0f;
            charController.center = new Vector3(0, 1.0f, 0);

            var groundChecker = playerObj.AddComponent<GroundChecker>();
            var animHooks = playerObj.AddComponent<PlayerAnimationHooks>();
            var inputHandler = playerObj.AddComponent<PlayerInputHandler>();
            var movement = playerObj.AddComponent<PlayerMovement>();
            var playerCtrl = playerObj.AddComponent<PlayerController>();

            SetPrivateField(playerCtrl, "settings", moveSettings);

            var health = playerObj.AddComponent<PlayerHealth>();
            var weaponInv = playerObj.AddComponent<PlayerWeaponInventory>();
            var backpackInv = playerObj.AddComponent<PlayerBackpackInventory>();
            var interaction = playerObj.AddComponent<PlayerInteraction>();

            // 7. Configure Main Camera
            var mainCamObj = Camera.main != null ? Camera.main.gameObject : new GameObject("Main Camera");
            var tpCam = mainCamObj.GetComponent<ThirdPersonCamera>() ?? mainCamObj.AddComponent<ThirdPersonCamera>();
            var camCollision = mainCamObj.GetComponent<CameraCollisionHandler>() ?? mainCamObj.AddComponent<CameraCollisionHandler>();
            var camInput = mainCamObj.GetComponent<CameraInputHandler>() ?? mainCamObj.AddComponent<CameraInputHandler>();

            SetPrivateField(tpCam, "settings", camSettings);
            SetPrivateField(tpCam, "targetPlayer", playerObj.transform);

            // 8. Create SafeZone Controller
            var zoneObj = new GameObject("SafeZoneController");
            var zoneCtrl = zoneObj.AddComponent<SafeZoneController>();
            var wallVis = zoneObj.AddComponent<SafeZoneWallVisual>();

            // Create Electric Wall Cylinder
            var wallCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wallCylinder.name = "ElectricZoneWallMesh";
            wallCylinder.transform.SetParent(zoneObj.transform);
            wallCylinder.transform.localPosition = Vector3.zero;
            wallCylinder.transform.localScale = new Vector3(500f, 50f, 500f);

            SetPrivateField(wallVis, "wallCylinderTransform", wallCylinder.transform);
            SetPrivateField(zoneCtrl, "settings", zoneSettings);
            SetPrivateField(zoneCtrl, "wallVisual", wallVis);

            // 9. Create Network & Backend Manager
            var netObj = new GameObject("NetworkManager");
            var netManager = netObj.AddComponent<CustomNetworkManager>();
            var serverBootstrap = netObj.AddComponent<HeadlessServerBootstrap>();
            var antiCheatManager = netObj.AddComponent<ServerAntiCheatManager>();
            var matchCtrl = netObj.AddComponent<MatchController>();

            SetPrivateField(netManager, "config", netConfig);
            SetPrivateField(netManager, "mapManager", mapMgr);
            SetPrivateField(matchCtrl, "config", matchConfig);
            SetPrivateField(antiCheatManager, "settings", antiCheatSettings);

            var backendObj = new GameObject("BackendApiClient");
            var backendClient = backendObj.AddComponent<BackendApiClient>();
            var statsClient = backendObj.AddComponent<StatsLeaderboardApiClient>();

            SetPrivateField(backendClient, "config", backendConfig);
            SetPrivateField(statsClient, "config", backendConfig);
            SetPrivateField(statsClient, "authClient", backendClient);

            var profilerObj = new GameObject("PerformanceProfiler");
            profilerObj.AddComponent<PerformanceProfiler>();

            var poolObj = new GameObject("ObjectPoolManager");
            poolObj.AddComponent<ObjectPoolManager>();

            // 10. Save Scene
            string scenePath = "Assets/Scenes/MainMap.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            AssetDatabase.Refresh();

            Debug.Log($"✅ [Battle Royale Setup] MainMap scene successfully generated and saved at '{scenePath}'! Click PLAY to test.");
        }

        private static void CreatePOI(Transform parent, string name, Vector3 pos, float radius)
        {
            var poiObj = new GameObject($"POI_{name}");
            poiObj.transform.SetParent(parent);
            poiObj.transform.position = pos;
            var poi = poiObj.AddComponent<PoiLocation>();
            SetPrivateField(poi, "poiName", name);
            SetPrivateField(poi, "poiRadius", radius);
        }

        private static void EnsureDirectory(string relativePath)
        {
            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }
        }

        private static T CreateAsset<T>(string assetPath) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, assetPath);
            }
            return asset;
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            if (target == null) return;
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (field != null)
            {
                field.SetValue(target, value);
            }
        }
    }
}
#endif
