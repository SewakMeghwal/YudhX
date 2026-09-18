using System;
using UnityEngine;

namespace BattleRoyale.Utilities
{
    public class PerformanceProfiler : MonoBehaviour
    {
        [SerializeField] private bool showOverlay = true;
        [SerializeField] private int targetFrameRate = 60;

        private float deltaTimeAccumulator;
        private int frameCount;
        private float currentFPS;
        private float frameTimeMs;
        private float gcMemoryMb;

        public float CurrentFPS => currentFPS;
        public float FrameTimeMs => frameTimeMs;
        public float GcMemoryMb => gcMemoryMb;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
        }

        private void Update()
        {
            deltaTimeAccumulator += Time.unscaledDeltaTime;
            frameCount++;

            if (deltaTimeAccumulator >= 0.5f) // Update stats twice per second
            {
                currentFPS = frameCount / deltaTimeAccumulator;
                frameTimeMs = (deltaTimeAccumulator / frameCount) * 1000.0f;
                gcMemoryMb = GC.GetTotalMemory(false) / (1024.0f * 1024.0f);

                frameCount = 0;
                deltaTimeAccumulator = 0f;
            }
        }

        private void OnGUI()
        {
            if (!showOverlay) return;

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = 14;
            style.normal.textColor = (currentFPS >= 55f) ? Color.green : (currentFPS >= 30f ? Color.yellow : Color.red);

            string statsText = $"FPS: {currentFPS:F0} ({frameTimeMs:F1} ms) | GC Mem: {gcMemoryMb:F1} MB";
            GUI.Label(new Rect(10, 10, 300, 25), statsText, style);
        }
    }
}
