using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace BattleRoyale.Editor
{
    public class BattleRoyaleBuilderWindow : EditorWindow
    {
        [MenuItem("Window/Battle Royale Builder")]
        [MenuItem("BattleRoyale/Open Battle Royale Control Panel")]
        public static void ShowWindow()
        {
            var window = GetWindow<BattleRoyaleBuilderWindow>("Battle Royale Builder");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(15);
            GUILayout.Label("🎮 YudhX Battle Royale 3D Control Panel", EditorStyles.boldLabel);
            GUILayout.Label("Generate scenes, build standalone executables, and configure settings.", EditorStyles.wordWrappedLabel);
            GUILayout.Space(15);

            GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f);
            if (GUILayout.Button("🏗️ 1-Click Build MainMap Scene (3D Visuals & HUD)", GUILayout.Height(45)))
            {
                SceneSetupWindow.GenerateMainMapScene();
                EditorUtility.DisplayDialog("Scene Setup Complete", "MainMap scene generated with textured terrain, POIs, weapons, vehicles, and HUD UI overlay!", "OK");
            }

            GUILayout.Space(15);
            GUI.backgroundColor = new Color(0.2f, 0.5f, 0.9f);
            if (GUILayout.Button("📦 1-Click Build Standalone Game (.exe)", GUILayout.Height(45)))
            {
                BattleRoyale.Multiplayer.Server.Editor.DedicatedServerBuilder.BuildWindowsClient();
            }

            GUILayout.Space(15);
            GUI.backgroundColor = new Color(0.8f, 0.4f, 0.2f);
            if (GUILayout.Button("🖥️ Build Dedicated Server (.exe)", GUILayout.Height(35)))
            {
                BattleRoyale.Multiplayer.Server.Editor.DedicatedServerBuilder.BuildWindowsDedicatedServer();
            }

            GUI.backgroundColor = Color.white;
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("Django REST Backend running at http://127.0.0.1:8000/api/", MessageType.Info);
        }
    }
}
#endif
