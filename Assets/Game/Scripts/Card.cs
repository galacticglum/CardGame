using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

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

public class Card
{
    public const string NamePropertyJson = "name";
    public const string SpriteNamePropertyJson = "sprite_name";
    public const string DescriptionPropertyJson = "description";
    public const string SpriteAssetPath = "Sprites/Cards";
    public const string AttackPointsPropertyJson = "attack_points";
    public const string HealthCostPropertyJson = "health_cost";
    public const string IsImmediatePropertyJson = "is_immediate";
    public const string BackgroundColourPropertyJson = "background_colour";

    public static string AssetFilePath => Path.Combine(Application.streamingAssetsPath, "Cards");

    [JsonProperty(NamePropertyJson, Required = Required.Always)]
    public string Name { get; set; }

    [JsonProperty(SpriteNamePropertyJson, Required = Required.Always)]
    public string SpriteName { get; set; }

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
    public Sprite Sprite => Resources.Load<Sprite>(Path.Combine(SpriteAssetPath, SpriteName));

    public event AttackPointsChangedEventHandler AttackPointsChanged;
    public event HealthCostChangedEventHandler HealthCostChanged;

    public Card()
    {
        AttackPoints = 0;
        HealthCost = 0;
        IsImmediate = false;
        BackgroundColour = Color.white;
        SpriteName = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
    }

    public override string ToString()
    {
        return $"{Name}: AP {AttackPoints}, HC {HealthCost}, Immediate {IsImmediate}";
    }
}
