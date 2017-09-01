using UnityEngine;
using UnityUtilities;

public class DeckController : MonoBehaviour
{
    public Deck Deck { get; private set; }

    private static readonly Vector2 deckSize = new Vector3(0.5f, 0.5f);
    private static readonly Vector2 cardSpacing = new Vector2(-0.25f, -0.4f);
    private static readonly Vector2 padding = new Vector2(0.125f, 0.125f);
    private const int cardsInDeck = 10;

    private Sprite cardbackSprite;

    private void Start()
    {
        cardbackSprite = Resources.Load<Sprite>("Sprites/Card_Back");
        GenerateDeck();

        SpriteCornerAlignment.Attach(gameObject, Corner.BottomLeft, cardbackSprite, deckSize, padding);
    }

    private void GenerateDeck()
    {
        Deck = new Deck(cardsInDeck);
        Deck.Removed += OnCardRemoved;

        for (int i = cardsInDeck - 1; i >= 0; i--)
        {
            GameObject cardGameObject = new GameObject("Card");
            SpriteRenderer spriteRenderer = cardGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = cardbackSprite;
            spriteRenderer.sortingLayerName = "Deck";
            spriteRenderer.sortingOrder = i + 1;

            cardGameObject.transform.position = -(cardSpacing / cardsInDeck * deckSize.x * i);
            cardGameObject.transform.localScale = deckSize;

            cardGameObject.transform.SetParent(transform, true);
        }
    }

    private void OnCardRemoved(object sender, CardCollectionEventArgs args)
    {
        if (transform.childCount <= 0) return;
        Destroy(transform.GetChild(0).gameObject);
    }

    public static DeckController Create()
    {
        return new GameObject("Deck", typeof(DeckController)).GetComponent<DeckController>();
    }
}