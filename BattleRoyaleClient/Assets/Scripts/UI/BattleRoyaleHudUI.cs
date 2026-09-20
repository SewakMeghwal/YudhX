using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleRoyale.HealthSystem;
using BattleRoyale.InventorySystem;
using BattleRoyale.Weapons;
using BattleRoyale.Zone;
using BattleRoyale.Match;

namespace BattleRoyale.UI
{
    public class BattleRoyaleHudUI : MonoBehaviour
    {
        [Header("Player References")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerWeaponInventory weaponInventory;

        [Header("Zone & Match References")]
        [SerializeField] private SafeZoneController zoneController;
        [SerializeField] private MatchController matchController;

        [Header("UI Canvas Elements")]
        [SerializeField] private Text healthText;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Text armorText;
        [SerializeField] private Image armorBarFill;

        [Header("Weapon UI")]
        [SerializeField] private Text weaponNameText;
        [SerializeField] private Text ammoText;
        [SerializeField] private Text weaponSlotText;

        [Header("Zone & Match UI")]
        [SerializeField] private Text zoneTimerText;
        [SerializeField] private Text matchStatsText;
        [SerializeField] private Text killFeedText;

        private List<string> killFeedMessages = new List<string>();

        private void Start()
        {
            FindReferencesIfNull();
            BuildRuntimeHUDIfMissing();
        }

        private void FindReferencesIfNull()
        {
            if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();
            if (weaponInventory == null) weaponInventory = FindObjectOfType<PlayerWeaponInventory>();
            if (zoneController == null) zoneController = FindObjectOfType<SafeZoneController>();
            if (matchController == null) matchController = FindObjectOfType<MatchController>();
        }

        private void Update()
        {
            UpdateHealthUI();
            UpdateWeaponUI();
            UpdateZoneUI();
            UpdateMatchUI();
        }

        private void UpdateHealthUI()
        {
            if (playerHealth == null) return;

            float currentHp = playerHealth.CurrentHealth;
            float maxHp = playerHealth.MaxHealth;
            float currentArmor = playerHealth.CurrentArmor;
            float maxArmor = playerHealth.MaxArmor;

            if (healthText != null) healthText.text = $"HP: {Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
            if (healthBarFill != null) healthBarFill.fillAmount = Mathf.Clamp01(currentHp / Mathf.Max(1f, maxHp));

            if (armorText != null) armorText.text = $"ARMOR: {Mathf.CeilToInt(currentArmor)} / {Mathf.CeilToInt(maxArmor)}";
            if (armorBarFill != null) armorBarFill.fillAmount = Mathf.Clamp01(currentArmor / Mathf.Max(1f, maxArmor));
        }

        private void UpdateWeaponUI()
        {
            if (weaponInventory == null) return;

            IWeapon activeWeapon = weaponInventory.ActiveWeapon;
            if (activeWeapon != null && activeWeapon.Data != null)
            {
                if (weaponNameText != null) weaponNameText.text = activeWeapon.Data.WeaponName.ToUpper();
                if (ammoText != null) ammoText.text = $"AMMO: {activeWeapon.CurrentAmmo} / 120";
            }
            else
            {
                if (weaponNameText != null) weaponNameText.text = "AR-47 (TACTICAL)";
                if (ammoText != null) ammoText.text = "AMMO: 30 / 120";
            }

            if (weaponSlotText != null) weaponSlotText.text = "[1] AR-47  |  [2] SUB-9  |  [3] SCATTER-12  |  [4] PISTOL-9";
        }

        private void UpdateZoneUI()
        {
            if (zoneTimerText == null) return;

            if (zoneController != null)
            {
                float remainingTime = zoneController.TimeRemainingSeconds;
                int minutes = Mathf.FloorToInt(remainingTime / 60f);
                int seconds = Mathf.FloorToInt(remainingTime % 60f);
                zoneTimerText.text = $"SAFE ZONE SHRINKING IN: {minutes:00}:{seconds:00}";
            }
            else
            {
                zoneTimerText.text = "SAFE ZONE SHRINKING IN: 01:45";
            }
        }

        private void UpdateMatchUI()
        {
            if (matchStatsText == null) return;

            int aliveCount = matchController != null ? matchController.AlivePlayerCount : 50;
            int killCount = 0;
            matchStatsText.text = $"ALIVE: {aliveCount}  |  KILLS: {killCount}";
        }

        public void AddKillFeedEntry(string killerName, string victimName, string weaponUsed)
        {
            string msg = $"<color=#FF5555>{killerName}</color> eliminated <color=#55FFFF>{victimName}</color> [{weaponUsed}]";
            killFeedMessages.Add(msg);
            if (killFeedMessages.Count > 5) killFeedMessages.RemoveAt(0);

            if (killFeedText != null)
            {
                killFeedText.text = string.Join("\n", killFeedMessages);
            }
        }

        private void BuildRuntimeHUDIfMissing()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("BattleRoyaleHUD_Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                transform.SetParent(canvasObj.transform, false);
            }

            // Crosshair Center Dot
            CreateUIPanel("CrosshairDot", transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(6, 6), new Color(1f, 1f, 1f, 0.9f));
            CreateUIPanel("CrosshairLeft", transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-12, 0), new Vector2(10, 2), new Color(1f, 1f, 1f, 0.7f));
            CreateUIPanel("CrosshairRight", transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(12, 0), new Vector2(10, 2), new Color(1f, 1f, 1f, 0.7f));
            CreateUIPanel("CrosshairTop", transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 12), new Vector2(2, 10), new Color(1f, 1f, 1f, 0.7f));
            CreateUIPanel("CrosshairBottom", transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -12), new Vector2(2, 10), new Color(1f, 1f, 1f, 0.7f));

            // Health Bar Background & Fill (Bottom Left)
            GameObject hpBg = CreateUIPanel("HealthBg", transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(20, 50), new Vector2(250, 22), new Color(0.1f, 0.1f, 0.1f, 0.8f));
            GameObject hpFillObj = CreateUIPanel("HealthFill", hpBg.transform, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0, 0), new Vector2(0, 0), new Color(0.2f, 0.8f, 0.2f, 0.9f));
            healthBarFill = hpFillObj.GetComponent<Image>();
            healthText = CreateUIText("HealthText", hpBg.transform, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0, 0), "HP: 100 / 100", 14, Color.white, TextAnchor.MiddleCenter);

            // Armor Bar Background & Fill (Bottom Left above HP)
            GameObject armorBg = CreateUIPanel("ArmorBg", transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(20, 78), new Vector2(250, 16), new Color(0.1f, 0.1f, 0.1f, 0.8f));
            GameObject armorFillObj = CreateUIPanel("ArmorFill", armorBg.transform, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0, 0), new Vector2(0, 0), new Color(0.2f, 0.6f, 1.0f, 0.9f));
            armorBarFill = armorFillObj.GetComponent<Image>();
            armorText = CreateUIText("ArmorText", armorBg.transform, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0, 0), "ARMOR: 75 / 100", 12, Color.white, TextAnchor.MiddleCenter);

            // Weapon & Ammo HUD (Bottom Right)
            GameObject wpnBg = CreateUIPanel("WeaponBg", transform, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-20, 50), new Vector2(280, 60), new Color(0.1f, 0.1f, 0.1f, 0.85f));
            weaponNameText = CreateUIText("WeaponNameText", wpnBg.transform, new Vector2(0f, 0.5f), new Vector2(1f, 1f), new Vector2(0, 0), "AR-47 (TACTICAL)", 16, new Color(1f, 0.85f, 0.2f), TextAnchor.MiddleCenter);
            ammoText = CreateUIText("AmmoText", wpnBg.transform, new Vector2(0f, 0f), new Vector2(1f, 0.5f), new Vector2(0, 0), "AMMO: 30 / 120", 14, Color.white, TextAnchor.MiddleCenter);

            // Weapon Slots Bar
            weaponSlotText = CreateUIText("WeaponSlotsText", transform, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-20, 15), "[1] AR-47  |  [2] SUB-9  |  [3] SCATTER-12  |  [4] PISTOL-9", 12, new Color(0.8f, 0.8f, 0.8f), TextAnchor.MiddleRight);

            // Safe Zone Timer Banner (Top Center)
            GameObject zoneBg = CreateUIPanel("ZoneTimerBg", transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -35), new Vector2(350, 35), new Color(0.1f, 0.15f, 0.25f, 0.9f));
            zoneTimerText = CreateUIText("ZoneTimerText", zoneBg.transform, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0, 0), "SAFE ZONE SHRINKING IN: 01:45", 14, new Color(0.4f, 0.9f, 1.0f), TextAnchor.MiddleCenter);

            // Match Alive & Kills (Top Right)
            GameObject statsBg = CreateUIPanel("MatchStatsBg", transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-20, -35), new Vector2(220, 35), new Color(0.1f, 0.1f, 0.1f, 0.85f));
            matchStatsText = CreateUIText("MatchStatsText", statsBg.transform, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0, 0), "ALIVE: 50  |  KILLS: 0", 14, Color.white, TextAnchor.MiddleCenter);

            // Kill Feed (Top Right below match stats)
            killFeedText = CreateUIText("KillFeedText", transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-20, -120), "Survivor1 eliminated Bot_Alpha [AR-47]\nBot_Bravo eliminated Bot_Charlie [Sub-9]", 12, Color.yellow, TextAnchor.UpperRight);

            // Initial Kill Feed seed
            AddKillFeedEntry("Survivor1", "Bot_Alpha", "AR-47");
        }

        private GameObject CreateUIPanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 sizeDelta, Color color)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = sizeDelta;

            Image img = obj.AddComponent<Image>();
            img.color = color;
            return obj;
        }

        private Text CreateUIText(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, string textContent, int fontSize, Color color, TextAnchor alignment)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = Vector2.zero;

            Text text = obj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = textContent;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;
            return text;
        }
    }
}
