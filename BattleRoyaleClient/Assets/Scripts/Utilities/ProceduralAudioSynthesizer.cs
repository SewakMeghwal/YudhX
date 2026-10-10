using UnityEngine;

namespace BattleRoyale.Utilities
{
    public static class ProceduralAudioSynthesizer
    {
        public static AudioClip CreateGunshotSound(string name, float duration = 0.25f, float frequency = 220f, float noiseMix = 0.7f)
        {
            int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] samples = new float[sampleCount];

            System.Random rand = new System.Random(name.GetHashCode());

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float decay = Mathf.Exp(-t * 22.0f); // Fast explosive decay

                float tone = Mathf.Sin(2.0f * Mathf.PI * frequency * t * (1.0f - t * 2f));
                float noise = (float)(rand.NextDouble() * 2.0 - 1.0);

                samples[i] = (tone * (1.0f - noiseMix) + noise * noiseMix) * decay;
            }

            AudioClip clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip CreateHitDingSound(string name = "HeadshotDing")
        {
            int sampleRate = 44100;
            float duration = 0.35f;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float decay = Mathf.Exp(-t * 12.0f);
                float tone = Mathf.Sin(2.0f * Mathf.PI * 1400.0f * t); // High metallic chime
                samples[i] = tone * decay * 0.7f;
            }

            AudioClip clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip CreateFootstepSound(string name = "Footstep")
        {
            int sampleRate = 44100;
            float duration = 0.08f;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] samples = new float[sampleCount];
            System.Random rand = new System.Random();

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float decay = Mathf.Exp(-t * 40.0f);
                float noise = (float)(rand.NextDouble() * 2.0 - 1.0);
                samples[i] = noise * decay * 0.4f;
            }

            AudioClip clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
