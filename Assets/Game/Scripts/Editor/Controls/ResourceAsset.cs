using System;
using UnityEditor;
using Object = UnityEngine.Object;

public struct ResourceAsset<T> where T : Object
{
    private T obj;
    public T Object
    {
        get { return obj; }
        set
        {
            obj = value;
            UpdateFilePath();
        }
    }

    private string filePath;
    public string FilePath => string.IsNullOrEmpty(filePath) ? string.Empty : filePath;

    private void UpdateFilePath()
    {
        if (obj == null)
        {
            filePath = string.Empty;
            return;
        }

        string objectPath = AssetDatabase.GetAssetPath(obj);
        int start = objectPath.IndexOf("Resources/", StringComparison.Ordinal) + "Resources/".Length;
        string pathWithinResources = objectPath.Substring(start);

        filePath = pathWithinResources;
    }
}