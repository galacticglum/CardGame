using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class CardManager : IEnumerable<Card>
{
    public Card this[string name] => cards[name];
    public Card Random => cards.Values.ElementAt(UnityEngine.Random.Range(0, cards.Count));

    private readonly Dictionary<string, Card> cards;

    public CardManager()
    {
        cards = new Dictionary<string, Card>();
        Load();
    }

    private void Load()
    {
        FileInfo[] files = new DirectoryInfo(Card.AssetFilePath).GetFiles("*.json", SearchOption.TopDirectoryOnly);
        foreach (FileInfo file in files)
        {
            string json = File.ReadAllText(file.FullName);
            Card card = JsonConvert.DeserializeObject<Card>(json);
            cards.Add(card.Name, card);
        }
    }


    public IEnumerator<Card> GetEnumerator()
    {
        return cards.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}