using System.IO;

public static class ContentUtility
{
    public static bool SaveToFile<T>(string filePath, T content) where T : IContent<T>, new()
    {
        if (string.IsNullOrEmpty(filePath)) return false;

        File.WriteAllText(filePath, content.Save());
        return true;
    }

    public static bool LoadFromFile<T>(string filePath, out T content) where T : IContent<T>, new()
    {
        content = default(T);
        if (string.IsNullOrEmpty(filePath)) return false;

        content = new T().Load(File.ReadAllText(filePath));
        return true;
    }
}
