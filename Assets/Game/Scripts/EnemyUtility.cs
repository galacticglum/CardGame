using System.IO;
using System.Text;
using Newtonsoft.Json;

public static class EnemyUtility
{
    public static void SaveToFile(string filePath, string name, string spriteName, string description, int attackPoints, int healthPoints)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        StringBuilder jsonStringBuilder = new StringBuilder();
        using (JsonWriter writer = new JsonTextWriter(new StringWriter(jsonStringBuilder)))
        {
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();

            writer.WritePropertyName(Enemy.NamePropertyJson);
            writer.WriteValue(name);

            writer.WritePropertyName(Enemy.SpritePathPropertyJson);
            writer.WriteValue(spriteName);

            writer.WritePropertyName(Enemy.DescriptionPropertyJson);
            writer.WriteValue(description);

            writer.WritePropertyName(Enemy.AttackPointsPropertyJson);
            writer.WriteValue(attackPoints);

            writer.WritePropertyName(Enemy.HealthPointsJson);
            writer.WriteValue(healthPoints);

            writer.WriteEndObject();
        }  

        File.WriteAllText(filePath, jsonStringBuilder.ToString());
    }

    public static bool LoadFromFile(string filePath, out string name, out string spriteName, out string description, out int attackPoints, out int healthPoints)
    {
        name = string.Empty;
        spriteName = string.Empty;
        description = string.Empty;
        attackPoints = 0;
        healthPoints = 0;

        if (string.IsNullOrEmpty(filePath)) return false;

        Enemy card = JsonConvert.DeserializeObject<Enemy>(File.ReadAllText(filePath));
        name = card.Name;
        spriteName = card.SpritePath;
        description = card.Description;
        attackPoints = card.AttackPoints;
        healthPoints = card.HealthPoints;

        return true;
    }
}
