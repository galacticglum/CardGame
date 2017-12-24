using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public CardManager CardManager { get; private set; }
    public DeckController DeckController { get; private set; }
    public DiscardPileController DiscardPileController { get; private set; }

    public EncounterController EncounterController { get; private set; }

    private bool isPlayersTurn;

    private void OnEnable()
    {
        Instance = this;
    }

    public void Start()
    {
        isPlayersTurn = true;
        CardManager = new CardManager();
        DeckController = DeckController.Create();
        DiscardPileController = DiscardPileController.Create();

        EncounterController = GetComponent<EncounterController>(); 

        Enemy enemy;
        ContentUtility.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Enemies/Orc.enemy"), out enemy);

        EncounterController.Spawn(new Encounter()
        {
            Enemies = new List<Enemy>
            {
                new Enemy(enemy),
                new Enemy(enemy),
                new Enemy(enemy)
            }
        });
    }

    public void Deal()
    {
        Card card = DeckController.Deck.Pop();
        if (card == null) return;

        CardController.Instance.AddCard(card);
    }

    public void AttackEnemy(Enemy target, Card card)
    {
        if (!isPlayersTurn) return;

        int difference = target.HealthPoints - card.AttackPoints;
        if (difference <= 0)
        {
            Debug.Log("Enemy is dead");
            // TODO: Enemy dead
        }

        target.HealthPoints = difference;
        if (!card.IsImmediate)
        {
            Debug.Log("Turn end");
            isPlayersTurn = false;
        }

        DiscardPileController.Add(card);
    }
}