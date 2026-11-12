#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OceanTales.EditorTools
{
    /// <summary>
    /// EmojiOne.png 的 .meta 是 Unity 5 时代的老格式（textureType: 0 = Default），
    /// 结果它只被导入成一张 Texture2D，没有 Sprite 子资源 —— 场景里 Help 按钮的
    /// Image.sprite 因此是 null，运行时就是一个白块。
    /// 这里把它改成正常的 Sprite (2D and UI) / Multiple，并保留原来的切片。
    /// </summary>
    public static class FixLegacySprite
    {
        const string Path = "Assets/TextMesh Pro/Sprites/EmojiOne.png";

        [MenuItem("Tools/OceanTales/修复 EmojiOne 贴图导入设置", false, 30)]
        public static void Fix() => FixInternal(true);

        public static void FixBatch()
        {
            FixInternal(true);
            EditorApplication.Exit(0);
        }

        static void FixInternal(bool verbose)
        {
            var ti = AssetImporter.GetAtPath(Path) as TextureImporter;
            if (ti == null)
            {
                Debug.LogError("[fix] 找不到 TextureImporter: " + Path);
                return;
            }

            Debug.Log($"[fix] before: textureType={ti.textureType} spriteImportMode={ti.spriteImportMode}");

            var settings = new TextureImporterSettings();
            ti.ReadTextureSettings(settings);
            settings.textureType = TextureImporterType.Sprite;
            settings.spriteMode = (int)SpriteImportMode.Multiple;
            settings.spritePixelsPerUnit = 100f;
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteExtrude = 1;
            settings.alphaIsTransparency = true;
            settings.mipmapEnabled = false;
            ti.SetTextureSettings(settings);
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.filterMode = FilterMode.Bilinear;
            ti.SaveAndReimport();

            var all = AssetDatabase.LoadAllAssetsAtPath(Path);
            var sprites = all.OfType<Sprite>().ToList();
            Debug.Log($"[fix] after: textureType={ti.textureType} spriteImportMode={ti.spriteImportMode} sprites={sprites.Count}");
            foreach (var s in sprites.Take(6))
                Debug.Log($"[fix]   {s.name} fileID={s.GetInstanceID()} rect={s.rect}");

            if (verbose && !Application.isBatchMode)
                EditorUtility.DisplayDialog("修复完成", $"EmojiOne 现在是 Sprite 了，共 {sprites.Count} 个切片。\n\n请重新构建 WebGL。", "好");
        }

        /// <summary>把场景里 sprite 为 null 的 Image 重新按名字绑回去（如果 ID 变了）。</summary>
        [MenuItem("Tools/OceanTales/重绑 Help 按钮图标", false, 31)]
        public static void RelinkHelp()
        {
            var sprites = AssetDatabase.LoadAllAssetsAtPath(Path).OfType<Sprite>().ToList();
            var target = sprites.FirstOrDefault(s => s.name == "EmojiOne_12") ?? sprites.FirstOrDefault();
            if (target == null)
            {
                Debug.LogError("[fix] EmojiOne 里没有 Sprite，先跑修复导入设置");
                return;
            }

            var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
                "Assets/Scenes/Land.unity");
            int fixedCount = 0;
            foreach (var img in Object.FindObjectsByType<UnityEngine.UI.Image>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (img != null && img.gameObject.name == "Help")
                {
                    img.sprite = target;
                    EditorUtility.SetDirty(img);
                    fixedCount++;
                    Debug.Log($"[fix] Help 按钮 -> {target.name}");
                }
            }
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
            Debug.Log($"[fix] 重新绑定 {fixedCount} 个");
        }
    }
}
#endif
