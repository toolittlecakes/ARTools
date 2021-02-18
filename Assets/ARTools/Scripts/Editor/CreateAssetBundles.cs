using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles/Compressed")]
    static void BuildAllAssetBundlesCompressed()
    {
        BuildAllAssetBundles(BuildAssetBundleOptions.None);
    }

    [MenuItem("Assets/Build AssetBundles/Uncompressed")]
    static void BuildAllAssetBundlesUncomressed()
    {
        BuildAllAssetBundles(BuildAssetBundleOptions.UncompressedAssetBundle);
    }

    static void BuildAllAssetBundles(BuildAssetBundleOptions assetBundleOptions)
    {
        var dir = Directory.CreateDirectory("Assets/DllBytes");
        CreateBytesDllAssets();

        Directory.CreateDirectory("Assets/AssetBundles");
        BuildPipeline.BuildAssetBundles(
            "Assets/AssetBundles",
            assetBundleOptions,
            BuildTarget.Android
        );

        Directory.Delete("Assets/DllBytes", true);
        File.Delete("Assets/DllBytes.meta");

        Debug.Log("AssetBundles successfully created");
    }

    static void CreateBytesDllAssets()
    {
        var labels = AssetDatabase.GetAllAssetBundleNames();
        foreach (var label in labels)
        {
            var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(label);
            foreach (var assetPath in assetPaths)
            {
                if (assetPath.EndsWith(".asmdef"))
                {
                    ImportDllAsBytes(Path.GetFileNameWithoutExtension(assetPath), label);
                }
            }
            ImportDllAsBytes("ARLens.ARTools.Runtime", label, label.Split('/')[1]);
        }
    }
    static void ImportDllAsBytes(string name, string label, string suffix = "")
    {

        var dllPath = Path.Combine("Library/ScriptAssemblies", name + ".dll");

        var bytesPath = Path.Combine("Assets/DllBytes/", name + "." + suffix + ".bytes");

        File.Copy(dllPath, bytesPath, true);

        AssetDatabase.ImportAsset(bytesPath, ImportAssetOptions.DontDownloadFromCacheServer);
        AssetDatabase.SaveAssets();

        AssetImporter assetImporter = AssetImporter.GetAtPath(bytesPath);
        assetImporter.assetBundleName = label;
        assetImporter.name = name;
        assetImporter.SaveAndReimport();
    }
}