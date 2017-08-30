using System.Collections;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    private static string draggingCardName;

    private const string ResourcePath = "Prefabs/CardTemplate";
    private const float PopInDuration = 0.25f;
    private const float ZoomMultiplier = 2f;
    private static readonly Vector3 RegularScale = new Vector3(1.25f, 1.25f, 1);

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

    private GameObject graphicsRootGameObject;
    private Card card;

    private LerpInformation<Vector3> popinMovementInformation;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    private bool isDragging;
    private int index;

    private void Start()
    {
        graphicsRootGameObject = transform.Find("Graphics").gameObject;
        
        card.AttackPointsChanged += (sender, args) => attackPointLabel.text = card.AttackPoints.ToString();
        card.ImmediateActionPointsChanged += OnImmediateActionPointsChanged;
    }

    private void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(draggingCardName)) return;

        index = CardController.Instance.RemoveCard(gameObject);

        transform.rotation = Quaternion.identity;
        graphicsRootGameObject.transform.position = originalPosition;
        graphicsRootGameObject.transform.rotation = Quaternion.identity;
        graphicsRootGameObject.transform.localScale = RegularScale;

        SetupSortingLayers(CardController.Instance.Count + 1);
        isDragging = true;
        draggingCardName = card.Name;
    }

    private void OnMouseEnter()
    {
        if (!string.IsNullOrEmpty(draggingCardName)) return;

        originalRotation = transform.rotation;
        originalPosition = graphicsRootGameObject.transform.position;

        graphicsRootGameObject.transform.rotation = Quaternion.identity;
        graphicsRootGameObject.transform.position = transform.position + new Vector3(0, 1.5f, 0);

        SetupSortingLayers(CardController.Instance.Count + 1);
        graphicsRootGameObject.transform.localScale = RegularScale * ZoomMultiplier;
    }

    private void OnMouseExit()
    {
        if (!string.IsNullOrEmpty(draggingCardName)) return;

        CardController.Instance.UpdateSortOrders();

        graphicsRootGameObject.transform.position = originalPosition;// + new Vector3(0, 0.25f, 0);
        graphicsRootGameObject.transform.rotation = originalRotation;
        graphicsRootGameObject.transform.localScale = RegularScale;
    }

    private void OnMouseUp()
    {
        if (draggingCardName != card.Name) return;

        isDragging = false;
        draggingCardName = string.Empty;

        graphicsRootGameObject.transform.localScale = RegularScale;
        CardController.Instance.InsertCardAt(this, index);
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, Camera.main.orthographicSize * 2f);
        }
    }

    private void HandlePopIn()
    {
        if (popinMovementInformation == null || popinMovementInformation.TimeLeft <= 0) return;
        graphicsRootGameObject.transform.position = popinMovementInformation.Step(Time.deltaTime);
    }

    private IEnumerator PopIn()
    {
        yield return new WaitForSeconds(0.1f);
        popinMovementInformation = new LerpInformation<Vector3>(graphicsRootGameObject.transform.position, originalPosition, PopInDuration, Vector3.Lerp);
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

        cardDisplay.image.sprite = card.Sprite;
        cardDisplay.nameLabel.text = card.Name;
        cardDisplay.descriptionLabel.text = card.Description;
        cardDisplay.attackPointLabel.text = card.AttackPoints.ToString();
        cardDisplay.immediatePointLabel.text = card.ImmediateActionPoints.ToString();
        cardDisplay.immediatePointLabel.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, card.ImmediateActionPoints <= 0 ? 0 : 1);

        return cardGameObject;
    }

    private void OnImmediateActionPointsChanged(object o, ImmediateActionPointsChangedEventArgs args)
    {
        immediatePointLabel.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, args.NewImmediateActionPoints <= 0 ? 0 : 1);
        immediatePointLabel.text = args.NewImmediateActionPoints.ToString();
    }
}
