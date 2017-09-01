using UnityEngine;

public class CardInteractionController : MonoBehaviour
{
    public static CardInteractionController Instance { get; private set; }

    private const float TargetZoomScale = 1f;
    private Vector3 regularScale;

    private Quaternion originalRotation;
    private Vector3 originalPosition;

    private CardDisplay currentDragCardDisplay;
    private CardDisplay currentHoverCardDisplay;

    private int dragCardIndex;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Update()
    {
        if (currentDragCardDisplay == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            CardDisplay cardDisplay = hit.collider?.gameObject.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                StopHover(currentHoverCardDisplay);
                if (Input.GetMouseButtonDown(0))
                {
                    BeginDrag(cardDisplay);
                }
                else
                {
                    BeginHover(cardDisplay);
                }
            }
            else
            {
                StopHover(currentHoverCardDisplay);
            }     
        }
        else
        {
            currentDragCardDisplay.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, Camera.main.orthographicSize * 2f);

            if (Input.GetMouseButtonUp(0))
            {
                StopDrag(currentDragCardDisplay);
            }
        }
    }

    public void BeginDrag(CardDisplay cardDisplay)
    {
        currentDragCardDisplay = cardDisplay;
        cardDisplay.SetupOrdering(CardController.Instance.Count + 1);

        dragCardIndex = CardController.Instance.RemoveCard(cardDisplay.gameObject);
        cardDisplay.transform.rotation = Quaternion.identity;
    }

    public void StopDrag(CardDisplay cardDisplay)
    {
        if(currentDragCardDisplay == null) return;

        cardDisplay.GraphicsRootGameObject.transform.localScale = regularScale;
        CardController.Instance.InsertCardAt(cardDisplay, dragCardIndex);

        currentDragCardDisplay = null;
    }

    private void BeginHover(CardDisplay cardDisplay)
    {
        if (cardDisplay == null || currentHoverCardDisplay != null || currentDragCardDisplay != null) return;

        regularScale = cardDisplay.GraphicsRootGameObject.transform.localScale;

        float zoomMultiplierX = TargetZoomScale / regularScale.x;
        float zoomMultiplierY = TargetZoomScale / regularScale.y;

        originalRotation = cardDisplay.GraphicsRootGameObject.transform.rotation; 
        originalPosition = cardDisplay.GraphicsRootGameObject.transform.position;

        cardDisplay.GraphicsRootGameObject.transform.rotation = Quaternion.identity;
        cardDisplay.GraphicsRootGameObject.transform.position = cardDisplay.transform.position + new Vector3(0, 1.5f, 0);

        cardDisplay.SetupOrdering(CardController.Instance.Count + 1);
        cardDisplay.GraphicsRootGameObject.transform.localScale = new Vector3(zoomMultiplierX * regularScale.x, zoomMultiplierY * regularScale.y, 1);

        currentHoverCardDisplay = cardDisplay;
    }

    private void StopHover(CardDisplay cardDisplay)
    {
        if (currentHoverCardDisplay == null || currentDragCardDisplay != null) return;

        CardController.Instance.UpdateSortOrders();

        cardDisplay.GraphicsRootGameObject.transform.position = originalPosition;
        cardDisplay.GraphicsRootGameObject.transform.rotation = originalRotation;
        cardDisplay.GraphicsRootGameObject.transform.localScale = regularScale;

        currentHoverCardDisplay = null;
    }
}