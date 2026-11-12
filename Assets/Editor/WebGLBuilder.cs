#if UNITY_EDITOR
// =============================================================================
// OceanTales —— WebGL 构建脚本
//
// 菜单：Tools / OceanTales
//   · 应用 WebGL 推荐设置
//   · 压缩音频为 Vorbis
//   · 构建 WebGL
//
// 命令行（Unity 装好 WebGL 模块后）：
//   "D:\Unity\6000.3.21f1\Editor\Unity.exe" -quit -batchmode -nographics ^
//     -projectPath "E:\unity\OceanStories" -buildTarget WebGL ^
//     -executeMethod OceanTales.EditorTools.WebGLBuilder.BuildWebGLBatch ^
//     -logFile "E:\unity\webgl_build.log"
// =============================================================================
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace OceanTales.EditorTools
{
    public static class WebGLBuilder
    {
        public const string OutputDir = "E:/unity/OceanTales_WebGL";

        // ------------------------------------------------------------------ //
        // 菜单
        // ------------------------------------------------------------------ //
        [MenuItem("Tools/OceanTales/应用 WebGL 推荐设置", false, 10)]
        static void ApplySettingsMenu()
        {
            ApplySettings();
            EditorUtility.DisplayDialog("WebGL",
                "已应用推荐设置：\n\n" +
                "· 压缩格式 Brotli（带解压回退）\n" +
                "· Managed Stripping = High，Strip Engine Code\n" +
                "· 关闭 Development Build\n" +
                "· 分辨率 1280 × 720，Gamma 色彩空间\n\n" +
                "切到 WebGL 平台后直接 Build 即可。", "好");
        }

        [MenuItem("Tools/OceanTales/压缩音频为 Vorbis", false, 11)]
        static void CompressAudioMenu()
        {
            int n = CompressAudio();
            EditorUtility.DisplayDialog("音频压缩", $"已处理 {n} 个音频（Vorbis，质量 0.6）。", "好");
        }

        [MenuItem("Tools/OceanTales/构建 WebGL", false, 12)]
        static void BuildMenu()
        {
            Build(OutputDir);
        }

        // ------------------------------------------------------------------ //
        // 批处理入口
        // ------------------------------------------------------------------ //
        public static void BuildWebGLBatch()
        {
            try
            {
                Build(OutputDir);
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Debug.LogError("[WebGL] 构建失败：" + e);
                EditorApplication.Exit(1);
            }
        }

        // ------------------------------------------------------------------ //
        // 设置
        // ------------------------------------------------------------------ //
        static void ApplySettings()
        {
            // 压缩：Brotli 体积最小；打开解压回退，保证任何静态托管都能跑
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.WebGL.decompressionFallback = true;

            // 体积裁剪
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.High);
            PlayerSettings.stripEngineCode = true;
            PlayerSettings.WebGL.dataCaching = true;
            PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;

            // 非开发构建
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.allowDebugging = false;

            // 画面
            PlayerSettings.colorSpace = ColorSpace.Gamma;
            PlayerSettings.defaultWebScreenWidth = 1280;
            PlayerSettings.defaultWebScreenHeight = 720;
            PlayerSettings.runInBackground = true;

            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.WebGL, ApiCompatibilityLevel.NET_Standard);

            AssetDatabase.SaveAssets();
            Debug.Log("[WebGL] 推荐设置已应用");
        }

        /// <summary>把所有 AudioClip 压成 Vorbis 0.6，返回处理数量。</summary>
        static int CompressAudio()
        {
            var guids = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets" });
            int changed = 0;
            try
            {
                AssetDatabase.StartAssetEditing();
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!(AssetImporter.GetAtPath(path) is AudioImporter importer)) continue;

                    var settings = importer.defaultSampleSettings;
                    if (settings.compressionFormat == AudioCompressionFormat.Vorbis && settings.quality <= 0.61f)
                        continue;

                    settings.compressionFormat = AudioCompressionFormat.Vorbis;
                    settings.quality = 0.6f;
                    importer.defaultSampleSettings = settings;
                    importer.forceToMono = false;
                    importer.SaveAndReimport();
                    changed++;
                    Debug.Log($"[WebGL] 音频 -> Vorbis: {path}");
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }
            return changed;
        }

        // ------------------------------------------------------------------ //
        // 构建
        // ------------------------------------------------------------------ //
        static void Build(string outDir)
        {
            ApplySettings();
            CompressAudio();

            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
                throw new Exception("Build Settings 里没有任何启用的场景（先打开一次工程并确认 Build Profiles 的场景列表）。");

            Debug.Log($"[WebGL] 场景 {scenes.Length} 个 → {string.Join(", ", scenes)}");

            if (Directory.Exists(outDir)) Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outDir,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            Debug.Log($"[WebGL] 结果 {summary.result}，大小 {summary.totalSize / 1048576.0:0.0} MB，" +
                      $"耗时 {summary.totalTime}，输出 {outDir}");

            if (summary.result != BuildResult.Succeeded)
                throw new Exception("BuildPlayer 返回 " + summary.result);

            Debug.Log("[WebGL] ✔ 构建完成");
        }
    }
}
#endif
