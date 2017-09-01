using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public delegate void AttackPointsChangedEventHandler(object sender, AttackPointsChangedEventArgs args);
public class AttackPointsChangedEventArgs : EventArgs
{
    public int OldAttackPoints { get; }
    public int NewAttackPoints { get; }

    public AttackPointsChangedEventArgs(int oldAttackPoints, int newAttackPoints)
    {
        OldAttackPoints = oldAttackPoints;
        NewAttackPoints = newAttackPoints;
    }
}

public delegate void HealthPointsChangedEventHandler(object sender, HealthPointsChangedEventArgs args);
public class HealthPointsChangedEventArgs : EventArgs
{
    public int OldHealthPoints { get; }
    public int NewHealthPoints { get; }

    public HealthPointsChangedEventArgs(int oldHealthPoints, int newHealthPoints)
    {
        OldHealthPoints = oldHealthPoints;
        NewHealthPoints = newHealthPoints;
    }
}

public delegate void HealthCostChangedEventHandler(object sender, HealthCostChangedEventArgs args);
public class HealthCostChangedEventArgs : EventArgs
{
    public int OldHealthCost { get; }
    public int NewHealthCost { get; }

    public HealthCostChangedEventArgs(int oldHealthCost, int newHealthCost)
    {
        OldHealthCost = oldHealthCost;
        NewHealthCost = newHealthCost;
    }
}

public class Card : IContent<Card>
{
    public static string AssetFilePath => Path.Combine(Application.streamingAssetsPath, "Cards");

    public const string NamePropertyJson = "name";
    public const string SpritePathPropertyJson = "sprite_path";
    public const string DescriptionPropertyJson = "description";
    public const string SpriteContentAssetPath = "Sprites/Cards";
    public const string AttackPointsPropertyJson = "attack_points";
    public const string HealthCostPropertyJson = "health_cost";
    public const string IsImmediatePropertyJson = "is_immediate";
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

    private int healthCost;
    [JsonProperty(HealthCostPropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public int HealthCost
    {
        get { return healthCost; }
        set
        {
            int old = healthCost;
            healthCost = value;

            if (healthCost == old) return;
            HealthCostChanged?.Invoke(this, new HealthCostChangedEventArgs(old, healthCost));
        }
    }

    [JsonProperty(IsImmediatePropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool IsImmediate { get; set; }

    [JsonProperty(BackgroundColourPropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public Color BackgroundColour { get; set; }

    [JsonIgnore]
    public Sprite Sprite => Resources.Load<Sprite>(SpritePath);

    public event AttackPointsChangedEventHandler AttackPointsChanged;
    public event HealthCostChangedEventHandler HealthCostChanged;

    public Card()
    {
        AttackPoints = 0;
        HealthCost = 0;
        IsImmediate = false;
        BackgroundColour = Color.white;
        SpritePath = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
    }

    public override string ToString()
    {
        return $"{Name}: AP {AttackPoints}, HC {HealthCost}, Immediate {IsImmediate}";
    }

    public string Save()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new ColorJsonConverter());
    }

    public Card Load(string json)
    {
        return JsonConvert.DeserializeObject<Card>(json, new ColorJsonConverter());
    }
}
