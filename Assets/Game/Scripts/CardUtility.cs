using System.IO;
using System.Text;
using Newtonsoft.Json;

public static class CardUtility
{
    public static void SaveToFile(string filePath, string name, string cardSpriteName, string description, int attackPoints, int immediateActionPoints)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        StringBuilder jsonStringBuilder = new StringBuilder();
        using (JsonWriter writer = new JsonTextWriter(new StringWriter(jsonStringBuilder)))
        {
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();

            writer.WritePropertyName(Entity.NamePropertyJson);
            writer.WriteValue(name);

            writer.WritePropertyName(Entity.SpriteNamePropertyJson);
            writer.WriteValue(cardSpriteName);

            writer.WritePropertyName(Entity.DescriptionPropertyJson);
            writer.WriteValue(description);

            writer.WritePropertyName(Card.AttackPointsPropertyJson);
            writer.WriteValue(attackPoints);

            writer.WritePropertyName(Card.ImmediateActionPointsJson);
            writer.WriteValue(immediateActionPoints);

            writer.WriteEndObject();
        }  

        File.WriteAllText(filePath, jsonStringBuilder.ToString());
    }

    public static bool LoadFromFile(string filePath, out string name, out string spriteName, out string description, out int attackPoints, out int immediateActionPoints)
    {
        name = string.Empty;
        spriteName = string.Empty;
        description = string.Empty;
        attackPoints = 0;
        immediateActionPoints = 0;

        if (string.IsNullOrEmpty(filePath)) return false;

        Card card = JsonConvert.DeserializeObject<Card>(File.ReadAllText(filePath));
        name = card.Name;
        spriteName = card.SpriteName;
        description = card.Description;
        attackPoints = card.AttackPoints;
        immediateActionPoints = card.ImmediateActionPoints;

        return true;
    }
}
