using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public delegate void HealthPointsChangedEventHandler(object sender, HealthPointsChangedEventArgs args);
public class HealthPointsChangedEventArgs : EventArgs
{
    public Enemy Enemy { get; }
    public int OldHealthPoints { get; }
    public int NewHealthPoints { get; }

    public HealthPointsChangedEventArgs(Enemy enemy, int oldHealthPoints, int newHealthPoints)
    {
        Enemy = enemy;
        OldHealthPoints = oldHealthPoints;
        NewHealthPoints = newHealthPoints;
    }
}

public class Enemy : IContent<Enemy>
{
    public static string AssetFilePath => Path.Combine(Application.streamingAssetsPath, "Enemies");

    public const string NamePropertyJson = "name";
    public const string SpritePathPropertyJson = "sprite_path";
    public const string DescriptionPropertyJson = "description";
    public const string AttackPointsPropertyJson = "attack_points";
    public const string HealthPointsJson = "health_points";
    public const string BackgroundColourPropertyJson = "background_colour";

    [JsonProperty(NamePropertyJson, Required = Required.Always)]
    public string Name { get; set; }

    [JsonProperty(SpritePathPropertyJson, Required = Required.Always)]
    public string SpritePath { get; set; }

    [JsonProperty(DescriptionPropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Description { get; set; }

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
            HealthPointsChanged?.Invoke(this, new HealthPointsChangedEventArgs(this, old, healthPoints));
        }
    }

    [JsonProperty(BackgroundColourPropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public Color BackgroundColour { get; set; }

    [JsonIgnore]
    public Sprite Sprite => Resources.Load<Sprite>(SpritePath);

    public event AttackPointsChangedEventHandler AttackPointsChanged;
    public event HealthPointsChangedEventHandler HealthPointsChanged;

    public Enemy()
    {
        AttackPoints = 0;
        BackgroundColour = Color.white;
        SpritePath = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
        HealthPoints = 0;
    }

    public Enemy(Enemy enemy)
    {
        AttackPoints = enemy.attackPoints;
        BackgroundColour = enemy.BackgroundColour;
        SpritePath = enemy.SpritePath;
        Name = enemy.Name;
        Description = enemy.Description;
        HealthPoints = enemy.healthPoints;

        AttackPointsChanged = null;
        HealthPointsChanged = null;
    }

    public string Save()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new ColorJsonConverter());
    }

    public Enemy Load(string json)
    {
        return JsonConvert.DeserializeObject<Enemy>(json, new ColorJsonConverter());
    }
}