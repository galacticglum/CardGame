using TMPro;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    public GameObject GraphicsRootGameObject { get; private set; }

    public Enemy Enemy { get; private set; } 
    public Sprite CardSprite { get; private set; }

    private const string ResourcePath = "Prefabs/Enemy_Template";

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
    private TextMeshPro healthLabel;

    private void Initialize()
    {
        GraphicsRootGameObject = cardFrame.gameObject;

        image.sprite = Enemy.Sprite;
        CardSprite = cardFrame.sprite;

        nameLabel.text = Enemy.Name;
        descriptionLabel.text = Enemy.Description;
        attackPointLabel.text = Enemy.AttackPoints.ToString();
        background.color = Enemy.BackgroundColour;

        Enemy.AttackPointsChanged += (sender, args) => attackPointLabel.text = Enemy.AttackPoints.ToString();
        Enemy.HealthPointsChanged += OnHealthPointsChanged;
        OnHealthPointsChanged(this, new HealthPointsChangedEventArgs(Enemy, Enemy.HealthPoints, Enemy.HealthPoints));
    }
    public static GameObject Create(Enemy enemy)
    {
        GameObject enemyGameObject = Instantiate(Resources.Load<GameObject>(ResourcePath));
        enemyGameObject.tag = "Enemy";

        EnemyDisplay enemyDisplay = enemyGameObject.GetComponent<EnemyDisplay>();

        enemyDisplay.Enemy = enemy;
        enemyDisplay.Initialize();

        return enemyGameObject;
    }

    private void OnHealthPointsChanged(object sender, HealthPointsChangedEventArgs args)
    {
        healthLabel.text = args.NewHealthPoints.ToString();
    }
}
