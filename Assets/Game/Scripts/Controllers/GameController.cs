using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public CardManager CardManager { get; private set; }
    public DeckController DeckController { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }

    public void Start()
    {
        CardManager = new CardManager();
        DeckController = DeckController.Create();
    }

    public void Deal()
    {
        Card card = DeckController.Deck.Pop();
        CardController.Instance.AddCard(card);
    }
}