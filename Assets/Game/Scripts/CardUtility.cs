using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class CardUtility
{
    public static void SaveToFile(string filePath, Card card)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        File.WriteAllText(filePath, JsonConvert.SerializeObject(card, Formatting.Indented, new ColorJsonConverter()));
    }

    public static bool LoadFromFile(string filePath, out Card card)
    {
        card = null;
        if (string.IsNullOrEmpty(filePath)) return false;

        card = JsonConvert.DeserializeObject<Card>(File.ReadAllText(filePath), new ColorJsonConverter());
        return true;
    }
}
