using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public GameObject GraphicsRootGameObject { get; private set; }
    private const string ResourcePath = "Prefabs/CardTemplate_New";

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer cardFrame;
    [SerializeField]
    private SpriteRenderer image;
    [SerializeField]
    private SpriteRenderer background;
    [SerializeField]
    private SpriteRenderer imageMask;
    [SerializeField]
    private TextMeshPro nameLabel;
    [SerializeField]
    private TextMeshPro descriptionLabel;
    [SerializeField]
    private TextMeshPro attackPointLabel;
    [SerializeField]
    private TextMeshPro healthCostLabel;
    [SerializeField]
    private SpriteRenderer quickActionIcon;

    private Card card; 

    private void Initialize()
    {
        GraphicsRootGameObject = cardFrame.gameObject;
        image.sprite = card.Sprite;
        nameLabel.text = card.Name;
        descriptionLabel.text = card.Description;
        attackPointLabel.text = card.AttackPoints.ToString();
        background.color = card.BackgroundColour;

        if (card.IsImmediate)
        {
            quickActionIcon.gameObject.SetActive(true);
        }

        card.AttackPointsChanged += (sender, args) => attackPointLabel.text = card.AttackPoints.ToString();
        card.HealthCostChanged += OnHealthCostChanged;
        OnHealthCostChanged(this, new HealthCostChangedEventArgs(card.HealthCost, card.HealthCost));

        SetupOrdering();
    }

    public void SetupOrdering(int offset = 0)
    {
        SetupZOrder(Mathf.Abs(CardController.Instance.Count + 1 - offset));
        SetupRenderOrder(offset * 10);
    }

    private void SetupZOrder(int z)
    {
        Vector3 position = transform.position;
        position.z = z;

        transform.position = position;
    }

    private void SetupRenderOrder(int offset)
    {
        imageMask.sortingLayerName = "Card";
        imageMask.sortingOrder = 0 + offset;

        background.sortingLayerName = "Card";
        background.sortingOrder = 0 + offset;

        image.sortingLayerName = "Card";
        image.sortingOrder = 1 + offset;

        cardFrame.sortingLayerName = "Card";
        cardFrame.sortingOrder = 2 + offset;

        nameLabel.renderer.sortingLayerName = "Card";
        nameLabel.renderer.sortingOrder = 3 + offset;

        descriptionLabel.renderer.sortingLayerName = "Card";
        descriptionLabel.renderer.sortingOrder = 3 + offset;

        attackPointLabel.renderer.sortingLayerName = "Card";
        attackPointLabel.renderer.sortingOrder = 3 + offset;

        healthCostLabel.renderer.sortingLayerName = "Card";
        healthCostLabel.renderer.sortingOrder = 3 + offset;

        quickActionIcon.sortingLayerName = "Card";
        quickActionIcon.sortingOrder = 3 + offset;
    }

    public static GameObject Create(Card card)
    {
        GameObject cardGameObject = Instantiate(Resources.Load<GameObject>(ResourcePath));
        CardDisplay cardDisplay = cardGameObject.GetComponent<CardDisplay>();

        cardDisplay.card = card;
        cardDisplay.Initialize();

        return cardGameObject;
    }

    private void OnHealthCostChanged(object o, HealthCostChangedEventArgs args)
    {
        healthCostLabel.text = args.NewHealthCost.ToString();
    }
}
