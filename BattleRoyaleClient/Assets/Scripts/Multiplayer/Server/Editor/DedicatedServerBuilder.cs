using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace BattleRoyale.Multiplayer.Server.Editor
{
    public static class DedicatedServerBuilder
    {
        [MenuItem("BattleRoyale/Build/Build Dedicated Server (Windows)")]
        public static void BuildWindowsDedicatedServer()
        {
            BuildDedicatedServer(BuildTarget.StandaloneWindows64, "Builds/Server/Windows/BattleRoyaleServer.exe");
        }

        [MenuItem("BattleRoyale/Build/Build Dedicated Server (Linux)")]
        public static void BuildLinuxDedicatedServer()
        {
            BuildDedicatedServer(BuildTarget.StandaloneLinux64, "Builds/Server/Linux/BattleRoyaleServer.x86_64");
        }

        private static void BuildDedicatedServer(BuildTarget target, string outputPath)
        {
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/MainMap.unity" },
                locationPathName = outputPath,
                target = target,
                options = BuildOptions.EnableHeadlessMode | BuildOptions.Development
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[Server Build] Dedicated Server build succeeded: {summary.totalSize} bytes -> {outputPath}");
            }
            else
            {
                Debug.LogError($"[Server Build] Dedicated Server build failed with {summary.totalErrors} errors.");
            }
        }
    }
}
#endif
