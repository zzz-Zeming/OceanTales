#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace OceanTales.EditorTools
{
    /// <summary>
    /// 把含中文的 UI Text 全部换成带中文字形的字体。
    /// WebGL 没有系统字体回退，原来的 LegacyRuntime 只有西文，中文会直接不显示。
    /// </summary>
    public static class ApplyCJKFont
    {
        const string FontPath = "Assets/Fonts/NotoSansSC-Subset.otf";
        static readonly Regex Cjk = new Regex(@"[\u4e00-\u9fa5]");

        public static void Run()
        {
            var font = AssetDatabase.LoadAssetAtPath<Font>(FontPath);
            if (font == null)
            {
                Debug.LogError("[font] 找不到字体：" + FontPath);
                EditorApplication.Exit(1);
                return;
            }

            int total = 0;

            foreach (var s in EditorBuildSettings.scenes.Where(x => x.enabled))
            {
                var scene = EditorSceneManager.OpenScene(s.path, OpenSceneMode.Additive);
                int n = FixScene(font);
                if (n > 0)
                {
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                }
                EditorSceneManager.CloseScene(scene, true);
                Debug.Log($"[font] {s.path}: {n} 个 Text");
                total += n;
            }

            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null) continue;
                var texts = go.GetComponentsInChildren<Text>(true).Where(t => t != null && Cjk.IsMatch(t.text)).ToArray();
                if (texts.Length == 0) continue;
                foreach (var t in texts) t.font = font;
                PrefabUtility.SavePrefabAsset(go);
                total += texts.Length;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[font] 完成，共替换 {total} 个 Text");
            EditorApplication.Exit(0);
        }

        static int FixScene(Font font)
        {
            int n = 0;
            foreach (var t in Object.FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (t == null || !Cjk.IsMatch(t.text ?? "")) continue;
                if (t.font == font) continue;
                t.font = font;
                EditorUtility.SetDirty(t);
                n++;
            }
            return n;
        }
    }
}
#endif
