using UnityEngine;
using UnityEngine.UI;

namespace BattleRoyale.UI
{
    public class CompassBarUI : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Text compassText;

        private void Start()
        {
            if (playerTransform == null)
            {
                var player = GameObject.Find("Player");
                if (player != null) playerTransform = player.transform;
            }

            BuildRuntimeCompassIfMissing();
        }

        private void Update()
        {
            if (playerTransform == null || compassText == null) return;

            float heading = playerTransform.eulerAngles.y;
            int headingInt = Mathf.RoundToInt(heading) % 360;

            string directionStr = GetCardinalDirection(headingInt);
            compassText.text = $"[ {directionStr}  |  {headingInt:000}° ]";
        }

        private string GetCardinalDirection(int heading)
        {
            if (heading >= 3375 || heading < 22.5f) return "N";
            if (heading >= 22.5f && heading < 67.5f) return "NE";
            if (heading >= 67.5f && heading < 112.5f) return "E";
            if (heading >= 112.5f && heading < 157.5f) return "SE";
            if (heading >= 157.5f && heading < 202.5f) return "S";
            if (heading >= 202.5f && heading < 247.5f) return "SW";
            if (heading >= 247.5f && heading < 292.5f) return "W";
            if (heading >= 292.5f && heading < 337.5f) return "NW";
            return "N";
        }

        private void BuildRuntimeCompassIfMissing()
        {
            if (compassText != null) return;

            GameObject compassBg = new GameObject("CompassBar_Bg");
            compassBg.transform.SetParent(transform, false);
            RectTransform bgRect = compassBg.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.5f, 1f);
            bgRect.anchorMax = new Vector2(0.5f, 1f);
            bgRect.anchoredPosition = new Vector2(0, -18);
            bgRect.sizeDelta = new Vector2(240, 24);

            Image img = compassBg.AddComponent<Image>();
            img.color = new Color(0.1f, 0.1f, 0.12f, 0.85f);

            GameObject textObj = new GameObject("CompassText");
            textObj.transform.SetParent(compassBg.transform, false);
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;

            compassText = textObj.AddComponent<Text>();
            compassText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            compassText.text = "[ N  |  000° ]";
            compassText.fontSize = 13;
            compassText.color = new Color(1.0f, 0.85f, 0.2f);
            compassText.alignment = TextAnchor.MiddleCenter;
        }
    }
}
