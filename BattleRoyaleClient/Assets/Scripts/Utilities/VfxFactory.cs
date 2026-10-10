using UnityEngine;

namespace BattleRoyale.Utilities
{
    public static class VfxFactory
    {
        public static GameObject SpawnMuzzleFlash(Vector3 position, Vector3 direction)
        {
            GameObject flashObj = new GameObject("VFX_MuzzleFlash");
            flashObj.transform.position = position;
            flashObj.transform.rotation = Quaternion.LookRotation(direction);

            ParticleSystem ps = flashObj.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = 0.05f;
            main.loop = false;
            main.startLifetime = 0.08f;
            main.startSpeed = 8.0f;
            main.startSize = 0.35f;
            main.startColor = new Color(1.0f, 0.85f, 0.3f, 0.9f);

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 15) });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;

            GameObject.Destroy(flashObj, 0.2f);
            return flashObj;
        }

        public static GameObject SpawnBloodImpact(Vector3 position, Vector3 normal)
        {
            GameObject bloodObj = new GameObject("VFX_BloodImpact");
            bloodObj.transform.position = position;
            bloodObj.transform.rotation = Quaternion.LookRotation(normal);

            ParticleSystem ps = bloodObj.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = 0.1f;
            main.loop = false;
            main.startLifetime = 0.25f;
            main.startSpeed = 4.0f;
            main.startSize = 0.25f;
            main.startColor = new Color(0.85f, 0.05f, 0.05f, 0.95f);

            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 25) });

            GameObject.Destroy(bloodObj, 0.5f);
            return bloodObj;
        }

        public static GameObject SpawnRedSmokeFlare(Vector3 position)
        {
            GameObject flareObj = new GameObject("VFX_RedSmokeFlare");
            flareObj.transform.position = position;

            ParticleSystem ps = flareObj.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = 10.0f;
            main.loop = true;
            main.startLifetime = 4.0f;
            main.startSpeed = 6.0f;
            main.startSize = 1.8f;
            main.startColor = new Color(0.95f, 0.15f, 0.15f, 0.6f);

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 10f;

            return flareObj;
        }
    }
}
