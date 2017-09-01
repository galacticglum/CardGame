using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public static CardController Instance { get; private set; }
    public int Count => cardGameObjects.Count;

    private List<GameObject> cardGameObjects;
    private Vector3 startPosition;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        cardGameObjects = new List<GameObject>();
        startPosition = new Vector3(0, -Camera.main.orthographicSize + 1.25f, 0);
    }

    public void AddCard(Card card)
    {
        cardGameObjects.Add(CardDisplay.Create(card));
        UpdateCardPositions();
    }

    public void InsertCard(CardDisplay cardDisplay)
    {
        cardGameObjects.Add(cardDisplay.gameObject);
        UpdateCardPositions();
    }

    public void InsertCardAt(CardDisplay cardDisplay, int index)
    {
        cardGameObjects.Insert(index, cardDisplay.gameObject);
        UpdateCardPositions();
    }

    public int RemoveCard(GameObject cardGameObject)
    {
        int index = cardGameObjects.IndexOf(cardGameObject);
        cardGameObjects.Remove(cardGameObject);
        UpdateCardPositions();

        return index;
    }

    private void UpdateCardPositions()
    {
        const float cardTotalRotation = 30f;
        const float turnOffset = -1f * (cardTotalRotation / 2f);
        const float gapBetweenCards = 1;

        float rotationPerCard = cardTotalRotation / cardGameObjects.Count;
        float offset = (cardGameObjects.Count - 1) * gapBetweenCards / 2f;
        float rotationOffset = turnOffset + (cardGameObjects.Count - 1) * rotationPerCard / 2f;

        for (int i = 0; i < cardGameObjects.Count; i++)
        {
            cardGameObjects[i].GetComponent<CardDisplay>().SetupOrdering(i + 1);

            float zRotation = -(turnOffset + i * rotationPerCard - rotationOffset);

            cardGameObjects[i].transform.position = startPosition + new Vector3(i * gapBetweenCards - offset, -(Mathf.Abs(zRotation) * 0.01f), 0);
            cardGameObjects[i].transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }

    public void UpdateSortOrders()
    {
        for (int i = 0; i < cardGameObjects.Count; i++)
        {
            cardGameObjects[i].GetComponent<CardDisplay>().SetupOrdering(i + 1);
        }
    }
}
