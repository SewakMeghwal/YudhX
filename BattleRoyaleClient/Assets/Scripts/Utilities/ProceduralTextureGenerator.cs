using UnityEngine;

namespace BattleRoyale.Utilities
{
    public static class ProceduralTextureGenerator
    {
        public static Texture2D CreateCamoTexture(Color baseColor, Color spotColorA, Color spotColorB, int width = 256, int height = 256)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, true);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = (float)x / width;
                    float v = (float)y / height;

                    float n1 = Mathf.PerlinNoise(u * 6.0f, v * 6.0f);
                    float n2 = Mathf.PerlinNoise(u * 12.0f + 10f, v * 12.0f + 10f);

                    Color pixel = baseColor;
                    if (n1 > 0.55f) pixel = Color.Lerp(baseColor, spotColorA, (n1 - 0.55f) * 2.2f);
                    if (n2 > 0.60f) pixel = Color.Lerp(pixel, spotColorB, (n2 - 0.60f) * 2.5f);

                    tex.SetPixel(x, y, pixel);
                }
            }
            tex.Apply();
            return tex;
        }

        public static Texture2D CreateRoadTexture(int width = 256, int height = 256)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, true);
            tex.wrapMode = TextureWrapMode.Repeat;

            Color asphaltBase = new Color(0.18f, 0.18f, 0.20f);
            Color stripeYellow = new Color(0.95f, 0.85f, 0.2f);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float noise = (Mathf.PerlinNoise(x * 0.1f, y * 0.1f) - 0.5f) * 0.08f;
                    Color col = asphaltBase + new Color(noise, noise, noise);

                    // Center Line Yellow Stripe
                    if (x >= width / 2 - 4 && x <= width / 2 + 4)
                    {
                        if ((y % 32) < 20) col = stripeYellow;
                    }

                    tex.SetPixel(x, y, col);
                }
            }
            tex.Apply();
            return tex;
        }
    }
}
