using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Enemy : Entity
{
    public static string AssetFilePath => Path.Combine(Application.streamingAssetsPath, "Enemies");
    public const string AttackPointsPropertyJson = "attack_points";
    public const string HealthPointsJson = "health_points";

    private int attackPoints;
    [JsonProperty(AttackPointsPropertyJson, Required = Required.Always)]
    public int AttackPoints
    {
        get { return attackPoints; }
        set
        {
            int old = attackPoints;
            attackPoints = value;

            if (attackPoints == old) return;
            AttackPointsChanged?.Invoke(this, new AttackPointsChangedEventArgs(old, attackPoints));
        }
    }

    private int healthPoints;
    [JsonProperty(HealthPointsJson, Required = Required.Always)]
    public int HealthPoints
    {
        get { return healthPoints; }
        set
        {
            int old = healthPoints;
            healthPoints = value;

            if (healthPoints == old) return;
            HealthPointsChanged?.Invoke(this, new HealthPointsChangedEventArgs(old, healthPoints));
        }
    }

    public override Sprite Sprite => Resources.Load<Sprite>(Path.Combine(AssetFilePath, SpriteName));

    public event AttackPointsChangedEventHandler AttackPointsChanged;
    public event HealthPointsChangedEventHandler HealthPointsChanged;
}