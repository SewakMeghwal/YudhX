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
using BattleRoyale.UI;
using BattleRoyale.Vehicles;

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

            // 2. Ensure Directories
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

            // 4. Create Realistic Materials
            Material groundMat = CreateColoredMaterial("Mat_Terrain", new Color(0.20f, 0.35f, 0.18f)); // Tactical Grass Green
            Material roadMat = CreateColoredMaterial("Mat_Road", new Color(0.18f, 0.18f, 0.20f)); // Asphalt Black/Gray
            Material mountainMat = CreateColoredMaterial("Mat_Mountain", new Color(0.35f, 0.32f, 0.28f)); // Rock/Soil Gray
            Material playerMat = CreateColoredMaterial("Mat_Player", new Color(0.12f, 0.25f, 0.50f)); // Tactical Navy Blue
            Material visorMat = CreateColoredMaterial("Mat_Visor", new Color(0.1f, 0.95f, 1.0f)); // Glowing Visor Cyan
            Material vestMat = CreateColoredMaterial("Mat_Vest", new Color(0.15f, 0.15f, 0.15f)); // Dark Vest Gray
            Material rifleMat = CreateColoredMaterial("Mat_Rifle", new Color(0.08f, 0.08f, 0.10f)); // Gun Metal
            Material scopeMat = CreateColoredMaterial("Mat_Scope", new Color(0.1f, 0.5f, 0.8f)); // Scope Glass
            Material bldgMat = CreateColoredMaterial("Mat_Building", new Color(0.55f, 0.55f, 0.58f)); // Concrete Gray
            Material bldgRoofMat = CreateColoredMaterial("Mat_Roof", new Color(0.45f, 0.18f, 0.15f)); // Terracotta Red Roof
            Material treeFoliageMat = CreateColoredMaterial("Mat_Foliage", new Color(0.12f, 0.32f, 0.12f)); // Pine Green
            Material treeTrunkMat = CreateColoredMaterial("Mat_Trunk", new Color(0.30f, 0.20f, 0.10f)); // Bark Brown
            Material rockMat = CreateColoredMaterial("Mat_Rock", new Color(0.42f, 0.42f, 0.45f)); // Boulder Gray
            Material zoneMat = CreateColoredMaterial("Mat_SafeZone", new Color(0.1f, 0.65f, 1.0f, 0.35f)); // Glowing Electric Blue
            Material crateGold = CreateColoredMaterial("Mat_Loot_AR47", new Color(1.0f, 0.75f, 0.1f)); // Gold Weapon Crate
            Material crateBlue = CreateColoredMaterial("Mat_Loot_Sub9", new Color(0.2f, 0.5f, 1.0f)); // Blue Weapon Crate
            Material crateRed = CreateColoredMaterial("Mat_Loot_Medkit", new Color(0.9f, 0.15f, 0.15f)); // Red Health Crate
            Material crateYellow = CreateColoredMaterial("Mat_Loot_Ammo", new Color(0.95f, 0.90f, 0.2f)); // Yellow Ammo Crate
            Material vehicleMat = CreateColoredMaterial("Mat_Vehicle", new Color(0.22f, 0.32f, 0.22f)); // Military Camo Green

            // 5. Setup Sunlight & Atmosphere (PUBG / Free Fire Style)
            var dirLightObj = GameObject.Find("Directional Light");
            if (dirLightObj == null) dirLightObj = new GameObject("Directional Light");
            var dirLight = dirLightObj.GetComponent<Light>();
            if (dirLight == null) dirLight = dirLightObj.AddComponent<Light>();
            dirLight.type = LightType.Directional;
            dirLight.color = new Color(1.0f, 0.95f, 0.85f); // Warm Sunlight
            dirLight.intensity = 1.35f;
            dirLight.shadows = LightShadows.Soft;
            dirLight.shadowStrength = 0.85f;
            dirLightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.65f, 0.80f, 1.0f);
            RenderSettings.ambientEquatorColor = new Color(0.70f, 0.78f, 0.70f);
            RenderSettings.ambientGroundColor = new Color(0.28f, 0.25f, 0.20f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.65f, 0.78f, 0.90f);
            RenderSettings.fogMode = FogMode.ExponentialWithDistance;
            RenderSettings.fogDensity = 0.0022f;

            // 6. Create Ground Environment & Perimeter Mountains (500m x 500m)
            var environmentRoot = new GameObject("Environment");

            var groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            groundObj.name = "GroundTerrain_500m";
            groundObj.transform.SetParent(environmentRoot.transform);
            groundObj.transform.position = Vector3.zero;
            groundObj.transform.localScale = new Vector3(50f, 1f, 50f); // 500m x 500m plane
            SetMaterial(groundObj, groundMat);

            // Perimeter Mountain Range around map borders
            CreatePerimeterMountains(environmentRoot.transform, mountainMat);

            // Asphalt Highways
            CreateRoad(environmentRoot.transform, new Vector3(0, 0.05f, 0), new Vector3(12f, 0.05f, 440f), roadMat); // North-South Highway
            CreateRoad(environmentRoot.transform, new Vector3(0, 0.05f, 0), new Vector3(440f, 0.05f, 12f), roadMat); // East-West Highway

            // Scatter Trees & Boulders
            CreateFoliageAndRocks(environmentRoot.transform, treeFoliageMat, treeTrunkMat, rockMat);

            // 7. Create MapManager & Spawns & POI Towns
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

            // Create 3D POI Town Structures & Loot Boxes
            Create3DPOITown(mapMgrObj.transform, "Pochinki Outpost", Vector3.zero, 50f, bldgMat, bldgRoofMat, crateGold, crateBlue, crateRed, crateYellow);
            Create3DPOITown(mapMgrObj.transform, "North Airfield", new Vector3(0, 0, 150f), 45f, bldgMat, bldgRoofMat, crateGold, crateBlue, crateRed, crateYellow);
            Create3DPOITown(mapMgrObj.transform, "South Docks", new Vector3(0, 0, -150f), 45f, bldgMat, bldgRoofMat, crateGold, crateBlue, crateRed, crateYellow);
            Create3DPOITown(mapMgrObj.transform, "Military Base", new Vector3(150f, 0, 0), 45f, bldgMat, bldgRoofMat, crateGold, crateBlue, crateRed, crateYellow);

            // 8. Create Tactical 3D Player Character
            var playerObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerObj.name = "Player";
            playerObj.transform.position = new Vector3(0, 1.5f, -10f);
            SetMaterial(playerObj, playerMat);

            // Visor Helmet, Tactical Vest & Backpack
            var visorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visorObj.name = "HelmetVisor";
            visorObj.transform.SetParent(playerObj.transform);
            visorObj.transform.localPosition = new Vector3(0, 0.5f, 0.35f);
            visorObj.transform.localScale = new Vector3(0.5f, 0.2f, 0.25f);
            SetMaterial(visorObj, visorMat);

            var vestObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vestObj.name = "TacticalVest";
            vestObj.transform.SetParent(playerObj.transform);
            vestObj.transform.localPosition = new Vector3(0, 0f, 0f);
            vestObj.transform.localScale = new Vector3(0.85f, 0.75f, 0.65f);
            SetMaterial(vestObj, vestMat);

            var backpackObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backpackObj.name = "TacticalBackpack";
            backpackObj.transform.SetParent(playerObj.transform);
            backpackObj.transform.localPosition = new Vector3(0, 0.1f, -0.45f);
            backpackObj.transform.localScale = new Vector3(0.65f, 0.75f, 0.4f);
            SetMaterial(backpackObj, vestMat);

            // Detailed 3D Assault Rifle (AR-47) in Hand
            var rifleBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rifleBody.name = "AR47_RifleBody";
            rifleBody.transform.SetParent(playerObj.transform);
            rifleBody.transform.localPosition = new Vector3(0.45f, 0.1f, 0.45f);
            rifleBody.transform.localScale = new Vector3(0.12f, 0.18f, 0.85f);
            SetMaterial(rifleBody, rifleMat);

            var rifleScope = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rifleScope.name = "RedDotScope";
            rifleScope.transform.SetParent(rifleBody.transform);
            rifleScope.transform.localPosition = new Vector3(0, 0.75f, 0.1f);
            rifleScope.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            rifleScope.transform.localScale = new Vector3(0.4f, 0.25f, 0.4f);
            SetMaterial(rifleScope, scopeMat);

            var charController = playerObj.GetComponent<CharacterController>();
            if (charController == null) charController = playerObj.AddComponent<CharacterController>();
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

            // 9. Create 4x4 Offroad Vehicle
            CreateOffroadVehicle(new Vector3(15f, 0.8f, -10f), vehicleMat, roadMat);

            // 10. Configure Main Camera (Third Person Over-the-Shoulder)
            var mainCamObj = Camera.main != null ? Camera.main.gameObject : new GameObject("Main Camera");
            var mainCam = mainCamObj.GetComponent<Camera>();
            if (mainCam == null) mainCam = mainCamObj.AddComponent<Camera>();
            mainCam.farClipPlane = 1000f;

            var tpCam = mainCamObj.GetComponent<ThirdPersonCamera>();
            if (tpCam == null) tpCam = mainCamObj.AddComponent<ThirdPersonCamera>();

            var camCollision = mainCamObj.GetComponent<CameraCollisionHandler>();
            if (camCollision == null) camCollision = mainCamObj.AddComponent<CameraCollisionHandler>();

            var camInput = mainCamObj.GetComponent<CameraInputHandler>();
            if (camInput == null) camInput = mainCamObj.AddComponent<CameraInputHandler>();

            SetPrivateField(tpCam, "settings", camSettings);
            SetPrivateField(tpCam, "targetPlayer", playerObj.transform);

            // 11. Create SafeZone Controller
            var zoneObj = new GameObject("SafeZoneController");
            var zoneCtrl = zoneObj.AddComponent<SafeZoneController>();
            var wallVis = zoneObj.AddComponent<SafeZoneWallVisual>();

            // Create Electric Wall Cylinder
            var wallCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wallCylinder.name = "ElectricZoneWallMesh";
            wallCylinder.transform.SetParent(zoneObj.transform);
            wallCylinder.transform.localPosition = Vector3.zero;
            wallCylinder.transform.localScale = new Vector3(500f, 50f, 500f);
            SetMaterial(wallCylinder, zoneMat);

            SetPrivateField(wallVis, "wallCylinderTransform", wallCylinder.transform);
            SetPrivateField(zoneCtrl, "settings", zoneSettings);
            SetPrivateField(zoneCtrl, "wallVisual", wallVis);

            // 12. Create In-Game HUD UI Canvas
            var hudObj = new GameObject("BattleRoyaleHUD");
            hudObj.AddComponent<BattleRoyaleHudUI>();

            // 13. Create Network & Backend Manager
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

            // 14. Save Scene
            string scenePath = "Assets/Scenes/MainMap.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            AssetDatabase.Refresh();

            Debug.Log($"✅ [Battle Royale Setup] MainMap scene successfully generated and saved at '{scenePath}'! Click PLAY to test.");
        }

        private static void CreateRoad(Transform parent, Vector3 pos, Vector3 scale, Material roadMat)
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "RoadSegment";
            road.transform.SetParent(parent);
            road.transform.position = pos;
            road.transform.localScale = scale;
            SetMaterial(road, roadMat);
        }

        private static void CreatePerimeterMountains(Transform parent, Material mountainMat)
        {
            var mountainGroup = new GameObject("PerimeterMountains");
            mountainGroup.transform.SetParent(parent);

            int count = 16;
            float radius = 240f;
            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count) * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 15f, Mathf.Sin(angle) * radius);
                var mountain = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                mountain.name = $"Mountain_{i + 1}";
                mountain.transform.SetParent(mountainGroup.transform);
                mountain.transform.position = pos;
                mountain.transform.localScale = new Vector3(60f, 30f, 60f);
                SetMaterial(mountain, mountainMat);
            }
        }

        private static void CreateFoliageAndRocks(Transform parent, Material foliageMat, Material trunkMat, Material rockMat)
        {
            var foliageGroup = new GameObject("FoliageAndRocks");
            foliageGroup.transform.SetParent(parent);

            System.Random rand = new System.Random(42); // Fixed seed for procedural consistency

            // 40 Pine Trees
            for (int i = 0; i < 40; i++)
            {
                float x = (float)(rand.NextDouble() * 360.0 - 180.0);
                float z = (float)(rand.NextDouble() * 360.0 - 180.0);
                if (Mathf.Abs(x) < 25f && Mathf.Abs(z) < 25f) continue; // Keep spawn clear

                var treeObj = new GameObject($"PineTree_{i + 1}");
                treeObj.transform.SetParent(foliageGroup.transform);
                treeObj.transform.position = new Vector3(x, 0, z);

                var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                trunk.name = "Trunk";
                trunk.transform.SetParent(treeObj.transform);
                trunk.transform.localPosition = new Vector3(0, 1.5f, 0);
                trunk.transform.localScale = new Vector3(0.5f, 1.5f, 0.5f);
                SetMaterial(trunk, trunkMat);

                var foliage = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                foliage.name = "FoliageCone";
                foliage.transform.SetParent(treeObj.transform);
                foliage.transform.localPosition = new Vector3(0, 4.5f, 0);
                foliage.transform.localScale = new Vector3(3.5f, 2.5f, 3.5f);
                SetMaterial(foliage, foliageMat);
            }

            // 25 Boulders
            for (int i = 0; i < 25; i++)
            {
                float x = (float)(rand.NextDouble() * 360.0 - 180.0);
                float z = (float)(rand.NextDouble() * 360.0 - 180.0);
                if (Mathf.Abs(x) < 25f && Mathf.Abs(z) < 25f) continue;

                var rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rock.name = $"Boulder_{i + 1}";
                rock.transform.SetParent(foliageGroup.transform);
                rock.transform.position = new Vector3(x, 0.8f, z);
                rock.transform.localScale = new Vector3(2.5f, 1.6f, 2.8f);
                SetMaterial(rock, rockMat);
            }
        }

        private static void Create3DPOITown(Transform parent, string name, Vector3 pos, float radius, Material bldgMat, Material roofMat, Material crateGold, Material crateBlue, Material crateRed, Material crateYellow)
        {
            var poiObj = new GameObject($"POI_{name}");
            poiObj.transform.SetParent(parent);
            poiObj.transform.position = pos;
            var poi = poiObj.AddComponent<PoiLocation>();
            SetPrivateField(poi, "poiName", name);
            SetPrivateField(poi, "poiRadius", radius);

            // Concrete Plaza Base
            var pad = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pad.name = "ConcretePlaza";
            pad.transform.SetParent(poiObj.transform);
            pad.transform.localPosition = new Vector3(0, 0.1f, 0);
            pad.transform.localScale = new Vector3(radius * 1.5f, 0.2f, radius * 1.5f);
            SetMaterial(pad, bldgMat);

            // Building 1: Main Hangar / Fortress
            var bldg1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bldg1.name = "MainBuilding";
            bldg1.transform.SetParent(poiObj.transform);
            bldg1.transform.localPosition = new Vector3(-12f, 4f, -12f);
            bldg1.transform.localScale = new Vector3(16f, 8f, 16f);
            SetMaterial(bldg1, bldgMat);

            var roof1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof1.name = "BuildingRoof";
            roof1.transform.SetParent(bldg1.transform);
            roof1.transform.localPosition = new Vector3(0, 0.55f, 0);
            roof1.transform.localScale = new Vector3(1.1f, 0.1f, 1.1f);
            SetMaterial(roof1, roofMat);

            // Building 2: Sniper Watchtower
            var tower = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tower.name = "SniperWatchtower";
            tower.transform.SetParent(poiObj.transform);
            tower.transform.localPosition = new Vector3(15f, 7.5f, 15f);
            tower.transform.localScale = new Vector3(6f, 15f, 6f);
            SetMaterial(tower, bldgMat);

            // Loot Crates around POI
            CreateLootCrate(poiObj.transform, "Loot_AR47", new Vector3(-5f, 0.6f, -5f), crateGold, LootRarity.Legendary);
            CreateLootCrate(poiObj.transform, "Loot_Sub9", new Vector3(8f, 0.6f, -8f), crateBlue, LootRarity.Rare);
            CreateLootCrate(poiObj.transform, "Loot_Medkit", new Vector3(-8f, 0.6f, 8f), crateRed, LootRarity.Epic);
            CreateLootCrate(poiObj.transform, "Loot_Ammo", new Vector3(5f, 0.6f, 5f), crateYellow, LootRarity.Uncommon);
        }

        private static void CreateLootCrate(Transform parent, string name, Vector3 localPos, Material mat, LootRarity rarity)
        {
            var crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crate.name = name;
            crate.transform.SetParent(parent);
            crate.transform.localPosition = localPos;
            crate.transform.localScale = new Vector3(1.2f, 0.8f, 0.8f);
            SetMaterial(crate, mat);

            var pickup = crate.AddComponent<LootPickup>();
            SetPrivateField(pickup, "rarity", rarity);
            SetPrivateField(pickup, "quantity", 1);

            // Glowing Light Shaft above Crate
            var lightObj = new GameObject("LootLight");
            lightObj.transform.SetParent(crate.transform);
            lightObj.transform.localPosition = Vector3.up * 1.5f;
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = mat.color;
            light.range = 5f;
            light.intensity = 2f;
        }

        private static void CreateOffroadVehicle(Vector3 pos, Material bodyMat, Material wheelMat)
        {
            var vehicleObj = new GameObject("Vehicle_4x4");
            vehicleObj.transform.position = pos;
            var vehicleCtrl = vehicleObj.AddComponent<VehicleController>();

            // Car Body
            var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "VehicleBody";
            body.transform.SetParent(vehicleObj.transform);
            body.transform.localPosition = new Vector3(0, 0.5f, 0);
            body.transform.localScale = new Vector3(2.2f, 1.2f, 4.2f);
            SetMaterial(body, bodyMat);

            // 4 Wheels
            Vector3[] wheelOffsets = new Vector3[]
            {
                new Vector3(-1.2f, -0.2f, 1.4f),
                new Vector3(1.2f, -0.2f, 1.4f),
                new Vector3(-1.2f, -0.2f, -1.4f),
                new Vector3(1.2f, -0.2f, -1.4f)
            };

            for (int i = 0; i < 4; i++)
            {
                var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wheel.name = $"Wheel_{i + 1}";
                wheel.transform.SetParent(vehicleObj.transform);
                wheel.transform.localPosition = wheelOffsets[i];
                wheel.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                wheel.transform.localScale = new Vector3(0.8f, 0.25f, 0.8f);
                SetMaterial(wheel, wheelMat);
            }
        }

        private static Material CreateColoredMaterial(string matName, Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.name = matName;
            mat.color = color;
            if (color.a < 1.0f)
            {
                mat.SetFloat("_Mode", 3); // Transparent
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
            return mat;
        }

        private static void SetMaterial(GameObject obj, Material mat)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null && mat != null)
            {
                renderer.sharedMaterial = mat;
            }
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
