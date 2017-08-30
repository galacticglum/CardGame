using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public CardManager CardManager { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }

    public void Deal()
    {
        CardManager = new CardManager();
        for (int i = 0; i < 5; i++)
        {
            CardController.Instance.AddCard(CardManager.Random);
        }
    }
}