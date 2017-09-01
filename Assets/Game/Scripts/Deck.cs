using Newtonsoft.Json;

public sealed class Deck : CardCollection, IContent<Deck>
{
    public Deck()
    {
    }

    public Deck(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Add(GameController.Instance.CardManager.Random);
        }
    }

    public string Save()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new ColorJsonConverter());
    }

    public Deck Load(string json)
    {
        return JsonConvert.DeserializeObject<Deck>(json, new ColorJsonConverter());
    }
}
