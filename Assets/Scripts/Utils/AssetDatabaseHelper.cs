using UnityEditor;
using System.Linq;

// Script by Bunny83
// source: https://answers.unity.com/questions/1377941/getassetpath-returning-incomplete-path-for-default.html?_ga=2.244309836.853815478.1588759949-327454147.1554941862
public class AssetDatabaseHelper
{
    public static T LoadAssetFromUniqueAssetPath<T>(string aAssetPath) where T : UnityEngine.Object
    {
        if (aAssetPath.Contains("::"))
        {
            string[] parts = aAssetPath.Split(new string[] { "::" },System.StringSplitOptions.RemoveEmptyEntries);
            aAssetPath = parts[0];
            if (parts.Length > 1)
            {
                string assetName = parts[1];
                System.Type t = typeof(T);
                var assets = AssetDatabase.LoadAllAssetsAtPath(aAssetPath)
                    .Where(i => t.IsAssignableFrom(i.GetType())).Cast<T>();
                var obj = assets.Where(i => i.name == assetName).FirstOrDefault();
                if (obj == null)
                {
                    int id;
                    if (int.TryParse(parts[1], out id))
                        obj = assets.Where(i => i.GetInstanceID() == id).FirstOrDefault();
                }
                if (obj != null)
                    return obj;
            }
        }
        return AssetDatabase.LoadAssetAtPath<T>(aAssetPath);
    }
    public static string GetUniqueAssetPath(UnityEngine.Object aObj)
    {
        string path = AssetDatabase.GetAssetPath(aObj);
        if (!string.IsNullOrEmpty(aObj.name))
            path += "::" + aObj.name;
        else
            path += "::" + aObj.GetInstanceID();
        return path;
    }
}