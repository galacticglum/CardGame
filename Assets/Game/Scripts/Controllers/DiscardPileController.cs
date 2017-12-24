using UnityEngine;
using UnityUtilities;

public class DiscardPileController : MonoBehaviour
{
    private static readonly Vector2 deckSize = new Vector3(0.5f, 0.5f);
    private static readonly Vector2 cardSpacing = new Vector2(-0.25f, -0.4f);
    private static readonly Vector2 padding = new Vector2(0.125f, 0.125f);

    private Sprite cardbackSprite;
    private CardCollection cards;

    private void Start()
    {
        cardbackSprite = Resources.Load<Sprite>("Sprites/Card_Back");

        cards = new CardCollection();
        cards.Added += OnAddCard;

        SpriteCornerAlignment.Attach(gameObject, Corner.BottomRight, cardbackSprite, deckSize, padding);
    }

    private void OnAddCard(object sender, CardCollectionEventArgs args)
    {
        GameObject cardGameObject = new GameObject("AttackEnemy Pile Card");
        SpriteRenderer spriteRenderer = cardGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardbackSprite;
        spriteRenderer.sortingLayerName = "Deck";
        spriteRenderer.sortingOrder = cards.Count;

        cardGameObject.transform.position = -(cardSpacing / cards.Count * deckSize.x * (cards.Count - 1));
        cardGameObject.transform.localScale = deckSize;

        cardGameObject.transform.SetParent(transform, false);
    }

    public void Add(Card card)
    {
        cards.Add(card);
    }

    public static DiscardPileController Create()
    {
        return new GameObject("AttackEnemy Pile", typeof(DiscardPileController)).GetComponent<DiscardPileController>();
    }
}