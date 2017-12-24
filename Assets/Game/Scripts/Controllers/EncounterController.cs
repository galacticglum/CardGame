using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterController : MonoBehaviour
{
    public static EncounterController Instance { get; private set; }
    public Encounter CurrentEncounter { get; private set; }

    private readonly Vector3 offset = new Vector3(0, .5f, 0);
    private Dictionary<Enemy, GameObject> enemyGameObjects;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Update()
    {
        if (CurrentEncounter == null) return;
        if (enemyGameObjects.Count < 0)
        {
            EndEncounter();
        }
        else
        {
            CurrentEncounter.AttackPattern?.Attack();
        }
    }

    public void Spawn(Encounter encounter)
    {
        CurrentEncounter = encounter;
        enemyGameObjects = new Dictionary<Enemy, GameObject>();

        foreach (Enemy enemy in encounter.Enemies)
        {
            enemy.HealthPointsChanged += OnEnemyHealthChanged;
            GameObject enemyGameObject = EnemyDisplay.Create(enemy);
            enemyGameObjects.Add(enemy, enemyGameObject);
        }

        UpdateEnemyPositions();
    }

    private void OnEnemyHealthChanged(object sender, HealthPointsChangedEventArgs args)
    {
        if (args.NewHealthPoints > 0 || !enemyGameObjects.ContainsKey(args.Enemy)) return;

        GameObject enemyGameObject = enemyGameObjects[args.Enemy];
        enemyGameObjects.Remove(args.Enemy);
        Destroy(enemyGameObject);

        UpdateEnemyPositions();
    }

    private void UpdateEnemyPositions()
    {
        Enemy[] enemies = enemyGameObjects.Keys.ToArray();
        for (int i = 0; i < enemies.Length; i++)
        {
            Sprite targetSprite = enemyGameObjects[enemies[i]].GetComponent<EnemyDisplay>().CardSprite;
            Vector3 size = enemyGameObjects[enemies[i]].transform.localScale;

            float yOffset = targetSprite.texture.height / targetSprite.pixelsPerUnit / 2 * size.y;

            float gapBetweenCards = targetSprite.texture.width / targetSprite.pixelsPerUnit;
            float gapOffset = (enemyGameObjects.Count - 1) * gapBetweenCards / 2f;

            Vector3 startPosition = new Vector3(0, Camera.main.orthographicSize - yOffset, 0) - offset;
            enemyGameObjects[enemies[i]].transform.position = startPosition + new Vector3(i * gapBetweenCards - gapOffset, 0, -1);
        }
    }

    private void EndEncounter()
    {
        CurrentEncounter = null;
        enemyGameObjects = null;

        Debug.Log("Encounter complete");
    }
}