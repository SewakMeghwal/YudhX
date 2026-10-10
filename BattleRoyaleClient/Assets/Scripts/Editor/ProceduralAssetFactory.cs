using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace BattleRoyale.Editor
{
    public static class ProceduralAssetFactory
    {
        // -------------------------------------------------------------
        // 1. HUMANOID SOLDIER CHARACTER MESH GENERATOR
        // -------------------------------------------------------------
        public static GameObject CreateHumanoidSoldier(Material bodyMat, Material visorMat, Material vestMat, Material rifleMat, Material scopeMat)
        {
            GameObject root = new GameObject("HumanoidSoldier_Player");

            // Pelvis & Main Body Capsule
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "BodyCapsule";
            body.transform.SetParent(root.transform);
            body.transform.localPosition = new Vector3(0, 1.0f, 0);
            body.transform.localScale = new Vector3(0.7f, 0.9f, 0.5f);
            SetMat(body, bodyMat);

            // Head & Helmet
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "HelmetHead";
            head.transform.SetParent(root.transform);
            head.transform.localPosition = new Vector3(0, 1.75f, 0);
            head.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
            SetMat(head, bodyMat);

            // Cyan Visor Goggles
            GameObject visor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visor.name = "VisorGoggles";
            visor.transform.SetParent(head.transform);
            visor.transform.localPosition = new Vector3(0, 0.1f, 0.4f);
            visor.transform.localScale = new Vector3(0.7f, 0.3f, 0.3f);
            SetMat(visor, visorMat);

            // Tactical Armor Vest (Plate Carrier)
            GameObject vest = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vest.name = "ArmorPlateCarrier";
            vest.transform.SetParent(root.transform);
            vest.transform.localPosition = new Vector3(0, 1.15f, 0);
            vest.transform.localScale = new Vector3(0.85f, 0.65f, 0.65f);
            SetMat(vest, vestMat);

            // Ammo Pouches on Chest
            for (int i = -1; i <= 1; i++)
            {
                GameObject pouch = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pouch.name = $"AmmoPouch_{i+2}";
                pouch.transform.SetParent(vest.transform);
                pouch.transform.localPosition = new Vector3(i * 0.28f, -0.15f, 0.55f);
                pouch.transform.localScale = new Vector3(0.22f, 0.35f, 0.25f);
                SetMat(pouch, vestMat);
            }

            // Backpack
            GameObject backpack = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backpack.name = "TacticalBackpack";
            backpack.transform.SetParent(root.transform);
            backpack.transform.localPosition = new Vector3(0, 1.2f, -0.42f);
            backpack.transform.localScale = new Vector3(0.65f, 0.75f, 0.42f);
            SetMat(backpack, vestMat);

            // Left & Right Arms
            CreateLimb(root.transform, "Arm_L", new Vector3(-0.52f, 1.15f, 0.1f), new Vector3(0.22f, 0.7f, 0.22f), bodyMat);
            CreateLimb(root.transform, "Arm_R", new Vector3(0.52f, 1.15f, 0.1f), new Vector3(0.22f, 0.7f, 0.22f), bodyMat);

            // Left & Right Legs & Combat Boots
            CreateLimb(root.transform, "Leg_L", new Vector3(-0.24f, 0.45f, 0f), new Vector3(0.26f, 0.85f, 0.26f), bodyMat);
            CreateLimb(root.transform, "Leg_R", new Vector3(0.24f, 0.45f, 0f), new Vector3(0.26f, 0.85f, 0.26f), bodyMat);

            CreateLimb(root.transform, "Boot_L", new Vector3(-0.24f, 0.08f, 0.08f), new Vector3(0.28f, 0.16f, 0.42f), vestMat);
            CreateLimb(root.transform, "Boot_R", new Vector3(0.24f, 0.08f, 0.08f), new Vector3(0.28f, 0.16f, 0.42f), vestMat);

            // 3D Rifle Attached to Right Hand
            GameObject rifle = CreateAR47Rifle(rifleMat, scopeMat);
            rifle.transform.SetParent(root.transform);
            rifle.transform.localPosition = new Vector3(0.42f, 1.15f, 0.55f);
            rifle.transform.localRotation = Quaternion.Euler(0, 0, 0);

            return root;
        }

        private static void CreateLimb(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.transform.SetParent(parent);
            obj.transform.localPosition = pos;
            obj.transform.localScale = scale;
            SetMat(obj, mat);
        }

        // -------------------------------------------------------------
        // 2. WEAPON 3D MESH GENERATORS (AR-47, Sub-9, Scatter-12, Pistol-9)
        // -------------------------------------------------------------
        public static GameObject CreateAR47Rifle(Material gunMat, Material scopeMat)
        {
            GameObject rifle = new GameObject("AR47_TacticalRifle");

            // Main Receiver
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "ReceiverBody";
            body.transform.SetParent(rifle.transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localScale = new Vector3(0.1f, 0.16f, 0.6f);
            SetMat(body, gunMat);

            // Barrel & Flash Hider
            GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            barrel.name = "RifledBarrel";
            barrel.transform.SetParent(rifle.transform);
            barrel.transform.localPosition = new Vector3(0, 0.02f, 0.55f);
            barrel.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            barrel.transform.localScale = new Vector3(0.06f, 0.3f, 0.06f);
            SetMat(barrel, gunMat);

            // Magazine Clip
            GameObject mag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mag.name = "ExtendedMagazine";
            mag.transform.SetParent(rifle.transform);
            mag.transform.localPosition = new Vector3(0, -0.22f, 0.08f);
            mag.transform.localRotation = Quaternion.Euler(15f, 0, 0);
            mag.transform.localScale = new Vector3(0.08f, 0.28f, 0.12f);
            SetMat(mag, gunMat);

            // Tactical Stock
            GameObject stock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stock.name = "TacticalStock";
            stock.transform.SetParent(rifle.transform);
            stock.transform.localPosition = new Vector3(0, 0.02f, -0.45f);
            stock.transform.localScale = new Vector3(0.09f, 0.15f, 0.35f);
            SetMat(stock, gunMat);

            // Holographic Scope Sight
            GameObject scopeHousing = GameObject.CreatePrimitive(PrimitiveType.Cube);
            scopeHousing.name = "ScopeHousing";
            scopeHousing.transform.SetParent(rifle.transform);
            scopeHousing.transform.localPosition = new Vector3(0, 0.14f, 0.05f);
            scopeHousing.transform.localScale = new Vector3(0.11f, 0.11f, 0.18f);
            SetMat(scopeHousing, gunMat);

            GameObject scopeLens = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            scopeLens.name = "ScopeLens";
            scopeLens.transform.SetParent(scopeHousing.transform);
            scopeLens.transform.localPosition = new Vector3(0, 0, 0);
            scopeLens.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            scopeLens.transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
            SetMat(scopeLens, scopeMat);

            return rifle;
        }

        public static GameObject CreateSub9SMG(Material gunMat, Material scopeMat)
        {
            GameObject smg = new GameObject("Sub9_SMG");

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "SMGBody";
            body.transform.SetParent(smg.transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localScale = new Vector3(0.09f, 0.14f, 0.45f);
            SetMat(body, gunMat);

            // Silencer / Suppressor
            GameObject silencer = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            silencer.name = "Suppressor";
            silencer.transform.SetParent(smg.transform);
            silencer.transform.localPosition = new Vector3(0, 0.01f, 0.4f);
            silencer.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            silencer.transform.localScale = new Vector3(0.08f, 0.22f, 0.08f);
            SetMat(silencer, gunMat);

            // Curved SMG Mag
            GameObject mag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mag.name = "CurvedMag";
            mag.transform.SetParent(smg.transform);
            mag.transform.localPosition = new Vector3(0, -0.2f, 0.05f);
            mag.transform.localRotation = Quaternion.Euler(20f, 0, 0);
            mag.transform.localScale = new Vector3(0.07f, 0.26f, 0.09f);
            SetMat(mag, gunMat);

            return smg;
        }

        // -------------------------------------------------------------
        // 3. 4x4 OFFROAD VEHICLE MESH GENERATOR
        // -------------------------------------------------------------
        public static GameObject Create4x4OffroadVehicle(Material bodyMat, Material tireMat, Material metalMat, Material headlightMat)
        {
            GameObject vehicle = new GameObject("Vehicle_4x4_Offroad");

            // Main Buggy Chassis Body
            GameObject chassis = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chassis.name = "MainChassis";
            chassis.transform.SetParent(vehicle.transform);
            chassis.transform.localPosition = new Vector3(0, 0.6f, 0);
            chassis.transform.localScale = new Vector3(2.2f, 0.8f, 4.4f);
            SetMat(chassis, bodyMat);

            // Rollcage Tubing (Top Roof Frame)
            GameObject rollcage = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rollcage.name = "RollcageTopFrame";
            rollcage.transform.SetParent(vehicle.transform);
            rollcage.transform.localPosition = new Vector3(0, 1.6f, -0.2f);
            rollcage.transform.localScale = new Vector3(2.0f, 1.1f, 2.2f);
            SetMat(rollcage, metalMat);

            // Engine Block (Front Hood)
            GameObject engine = GameObject.CreatePrimitive(PrimitiveType.Cube);
            engine.name = "EngineHood";
            engine.transform.SetParent(vehicle.transform);
            engine.transform.localPosition = new Vector3(0, 0.85f, 1.4f);
            engine.transform.localScale = new Vector3(1.8f, 0.65f, 1.4f);
            SetMat(engine, bodyMat);

            // Dual Glowing Headlights
            for (int side = -1; side <= 1; side += 2)
            {
                GameObject lightMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                lightMesh.name = $"Headlight_{(side < 0 ? "L" : "R")}";
                lightMesh.transform.SetParent(vehicle.transform);
                lightMesh.transform.localPosition = new Vector3(side * 0.75f, 0.85f, 2.15f);
                lightMesh.transform.localRotation = Quaternion.Euler(90f, 0, 0);
                lightMesh.transform.localScale = new Vector3(0.35f, 0.1f, 0.35f);
                SetMat(lightMesh, headlightMat);

                GameObject spotObj = new GameObject($"HeadlightSpot_{(side < 0 ? "L" : "R")}");
                spotObj.transform.SetParent(lightMesh.transform);
                spotObj.transform.localPosition = Vector3.zero;
                spotObj.transform.localRotation = Quaternion.identity;
                Light lightComp = spotObj.AddComponent<Light>();
                lightComp.type = LightType.Spot;
                lightComp.color = Color.white;
                lightComp.range = 35f;
                lightComp.spotAngle = 60f;
                lightComp.intensity = 3.5f;
            }

            // 4 Offroad Treaded Tires with Rims
            Vector3[] wheelPos = new Vector3[]
            {
                new Vector3(-1.25f, 0.45f, 1.45f),
                new Vector3(1.25f, 0.45f, 1.45f),
                new Vector3(-1.25f, 0.45f, -1.45f),
                new Vector3(1.25f, 0.45f, -1.45f)
            };

            for (int i = 0; i < 4; i++)
            {
                GameObject tire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                tire.name = $"Tire_{i+1}";
                tire.transform.SetParent(vehicle.transform);
                tire.transform.localPosition = wheelPos[i];
                tire.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                tire.transform.localScale = new Vector3(0.9f, 0.35f, 0.9f);
                SetMat(tire, tireMat);

                GameObject rim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                rim.name = "MetalRim";
                rim.transform.SetParent(tire.transform);
                rim.transform.localPosition = Vector3.zero;
                rim.transform.localScale = new Vector3(0.55f, 1.05f, 0.55f);
                SetMat(rim, metalMat);
            }

            return vehicle;
        }

        // -------------------------------------------------------------
        // 4. MILITARY WATCHTOWER & HANGAR BUILDINGS
        // -------------------------------------------------------------
        public static GameObject CreateMilitaryWatchtower(Material bldgMat, Material metalMat, Material roofMat)
        {
            GameObject tower = new GameObject("MilitaryWatchtower_Structure");

            // 4 Support Legs
            float legX = 2.5f, legZ = 2.5f;
            for (int i = 0; i < 4; i++)
            {
                float x = (i % 2 == 0) ? -legX : legX;
                float z = (i < 2) ? -legZ : legZ;

                GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                leg.name = $"SupportLeg_{i+1}";
                leg.transform.SetParent(tower.transform);
                leg.transform.localPosition = new Vector3(x, 6f, z);
                leg.transform.localScale = new Vector3(0.45f, 6.0f, 0.45f);
                SetMat(leg, metalMat);
            }

            // Top Platform Base
            GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platform.name = "PlatformFloor";
            platform.transform.SetParent(tower.transform);
            platform.transform.localPosition = new Vector3(0, 12.1f, 0);
            platform.transform.localScale = new Vector3(6.5f, 0.3f, 6.5f);
            SetMat(platform, bldgMat);

            // Guard House Cabin
            GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabin.name = "GuardCabin";
            cabin.transform.SetParent(tower.transform);
            cabin.transform.localPosition = new Vector3(0, 13.6f, 0);
            cabin.transform.localScale = new Vector3(4.5f, 2.8f, 4.5f);
            SetMat(cabin, bldgMat);

            // Roof
            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof.name = "RoofCap";
            roof.transform.SetParent(tower.transform);
            roof.transform.localPosition = new Vector3(0, 15.1f, 0);
            roof.transform.localScale = new Vector3(5.2f, 0.25f, 5.2f);
            SetMat(roof, roofMat);

            return tower;
        }

        public static GameObject CreateSandbagBarrier(Material sandbagMat)
        {
            GameObject barrier = new GameObject("SandbagBarrier");

            for (int layer = 0; layer < 3; layer++)
            {
                int count = 4 - (layer % 2);
                float startX = -(count - 1) * 0.45f;
                for (int i = 0; i < count; i++)
                {
                    GameObject bag = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    bag.name = $"Sandbag_{layer}_{i}";
                    bag.transform.SetParent(barrier.transform);
                    bag.transform.localPosition = new Vector3(startX + i * 0.9f, 0.2f + layer * 0.35f, 0);
                    bag.transform.localRotation = Quaternion.Euler(0, 90f, 0);
                    bag.transform.localScale = new Vector3(0.4f, 0.45f, 0.4f);
                    SetMat(bag, sandbagMat);
                }
            }

            return barrier;
        }

        private static void SetMat(GameObject obj, Material mat)
        {
            var r = obj.GetComponent<Renderer>();
            if (r != null && mat != null) r.sharedMaterial = mat;
        }
    }
}
#endif
