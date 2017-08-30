using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public GameObject GraphicsRootGameObject { get; private set; }

    private const string ResourcePath = "Prefabs/CardTemplate";

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer cardFrame;
    [SerializeField]
    private SpriteRenderer image;
    [SerializeField]
    private SpriteRenderer imageMask;
    [SerializeField]
    private TextMeshPro nameLabel;
    [SerializeField]
    private TextMeshPro descriptionLabel;
    [SerializeField]
    private TextMeshPro attackPointLabel;
    [SerializeField]
    private TextMeshPro immediatePointLabel;
    private Card card;

    private void Start()
    {
        GraphicsRootGameObject = transform.Find("Graphics").gameObject;

        image.sprite = card.Sprite;
        nameLabel.text = card.Name;
        descriptionLabel.text = card.Description;
        attackPointLabel.text = card.AttackPoints.ToString();

        card.AttackPointsChanged += (sender, args) => attackPointLabel.text = card.AttackPoints.ToString();
        card.ImmediateActionPointsChanged += OnImmediateActionPointsChanged;
        OnImmediateActionPointsChanged(this, new ImmediateActionPointsChangedEventArgs(card.ImmediateActionPoints, card.ImmediateActionPoints));
    }

    public void SetupSortingLayers(int offset = 0)
    {
        offset *= 10;

        image.sortingLayerName = "Card";
        image.sortingOrder = 0 + offset;

        cardFrame.sortingLayerName = "Card";
        cardFrame.sortingOrder = 1 + offset;

        imageMask.sortingLayerName = "Card";
        imageMask.sortingOrder = 0 + offset;

        nameLabel.renderer.sortingLayerName = "Card";
        nameLabel.renderer.sortingOrder = 2 + offset;

        descriptionLabel.renderer.sortingLayerName = "Card";
        descriptionLabel.renderer.sortingOrder = 2 + offset;

        attackPointLabel.renderer.sortingLayerName = "Card";
        attackPointLabel.renderer.sortingOrder = 2 + offset;

        immediatePointLabel.renderer.sortingLayerName = "Card";
        immediatePointLabel.renderer.sortingOrder = 2 + offset;
    }

    public static GameObject Create(Card card)
    {
        GameObject cardGameObject = Instantiate(Resources.Load<GameObject>(ResourcePath));
        CardDisplay cardDisplay = cardGameObject.GetComponent<CardDisplay>();

        cardDisplay.SetupSortingLayers();
        cardDisplay.card = card;

        return cardGameObject;
    }

    private void OnImmediateActionPointsChanged(object o, ImmediateActionPointsChangedEventArgs args)
    {
        immediatePointLabel.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, args.NewImmediateActionPoints <= 0 ? 0 : 1);
        immediatePointLabel.text = args.NewImmediateActionPoints.ToString();
    }
}
