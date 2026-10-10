using UnityEngine;
using UnityEngine.UI;
using BattleRoyale.Zone;

namespace BattleRoyale.UI
{
    public class MinimapRadarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private SafeZoneController zoneController;

        [Header("Minimap Settings")]
        [SerializeField] private RectTransform minimapContainer;
        [SerializeField] private RectTransform playerMarker;
        [SerializeField] private RectTransform zoneCircleMarker;
        [SerializeField] private float mapScaleMeters = 250f; // Radar radius in world meters

        private void Start()
        {
            if (playerTransform == null)
            {
                var player = GameObject.Find("Player");
                if (player != null) playerTransform = player.transform;
            }
            if (zoneController == null) zoneController = FindObjectOfType<SafeZoneController>();

            BuildRuntimeMinimapIfMissing();
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // Rotate player marker to match heading
            if (playerMarker != null)
            {
                float yaw = playerTransform.eulerAngles.y;
                playerMarker.localRotation = Quaternion.Euler(0, 0, -yaw);
            }

            // Update Zone Circle on Minimap
            if (zoneController != null && zoneCircleMarker != null)
            {
                Vector3 zoneCenter = zoneController.CurrentCenter;
                Vector3 playerPos = playerTransform.position;
                Vector3 relativePos = zoneCenter - playerPos;

                float uiFactor = 90f / mapScaleMeters; // 90px radar radius
                zoneCircleMarker.anchoredPosition = new Vector2(relativePos.x * uiFactor, relativePos.z * uiFactor);

                float radiusPx = zoneController.CurrentRadius * uiFactor * 2f;
                zoneCircleMarker.sizeDelta = new Vector2(radiusPx, radiusPx);
            }
        }

        private void BuildRuntimeMinimapIfMissing()
        {
            if (minimapContainer != null) return;

            // Create Minimap Circle Container (Top-Left)
            GameObject mapBg = new GameObject("MinimapRadar_Container");
            mapBg.transform.SetParent(transform, false);
            minimapContainer = mapBg.AddComponent<RectTransform>();
            minimapContainer.anchorMin = new Vector2(0f, 1f);
            minimapContainer.anchorMax = new Vector2(0f, 1f);
            minimapContainer.anchoredPosition = new Vector2(110, -110);
            minimapContainer.sizeDelta = new Vector2(180, 180);

            Image bgImg = mapBg.AddComponent<Image>();
            bgImg.color = new Color(0.1f, 0.15f, 0.1f, 0.85f);

            // Safe Zone Circle Ring
            GameObject zoneObj = new GameObject("Minimap_ZoneCircle");
            zoneObj.transform.SetParent(mapBg.transform, false);
            zoneCircleMarker = zoneObj.AddComponent<RectTransform>();
            zoneCircleMarker.anchorMin = new Vector2(0.5f, 0.5f);
            zoneCircleMarker.anchorMax = new Vector2(0.5f, 0.5f);
            zoneCircleMarker.anchoredPosition = Vector2.zero;
            zoneCircleMarker.sizeDelta = new Vector2(180, 180);

            Image zoneImg = zoneObj.AddComponent<Image>();
            zoneImg.color = new Color(0.2f, 0.6f, 1.0f, 0.4f);

            // Player Icon (Center Arrow)
            GameObject playerIcon = new GameObject("Minimap_PlayerMarker");
            playerIcon.transform.SetParent(mapBg.transform, false);
            playerMarker = playerIcon.AddComponent<RectTransform>();
            playerMarker.anchorMin = new Vector2(0.5f, 0.5f);
            playerMarker.anchorMax = new Vector2(0.5f, 0.5f);
            playerMarker.anchoredPosition = Vector2.zero;
            playerMarker.sizeDelta = new Vector2(18, 22);

            Image iconImg = playerIcon.AddComponent<Image>();
            iconImg.color = new Color(1.0f, 0.85f, 0.1f);
        }
    }
}
