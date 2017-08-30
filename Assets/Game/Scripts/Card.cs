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

public delegate void ImmediateActionPointsChangedEventHandler(object sender, ImmediateActionPointsChangedEventArgs args);
public class ImmediateActionPointsChangedEventArgs : EventArgs
{
    public int OldImmediateActionPoints { get; }
    public int NewImmediateActionPoints { get; }

    public ImmediateActionPointsChangedEventArgs(int oldImmediateActionPoints, int newImmediateActionPoints)
    {
        OldImmediateActionPoints = oldImmediateActionPoints;
        NewImmediateActionPoints = newImmediateActionPoints;
    }
}


public class Card
{
    public static string AssetFilePath => Path.Combine(Application.streamingAssetsPath, "Cards");

    public const string NamePropertyJson = "name";
    public const string SpriteNamePropertyJson = "sprite_name";
    public const string DescriptionPropertyJson = "description";
    public const string AttackPointsPropertyJson = "attack_points";
    public const string ImmediateActionPointsJson = "immediate_action_points";

    [JsonProperty(NamePropertyJson, Required = Required.Always)]
    public string Name { get; set; }

    [JsonProperty(SpriteNamePropertyJson, Required = Required.Always)]
    public string SpriteName { get; set; }

    [JsonProperty(DescriptionPropertyJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Description { get; set; }

    private int attackPointsPoints;
    [JsonProperty(AttackPointsPropertyJson, Required = Required.Always)]
    public int AttackPoints
    {
        get { return attackPointsPoints; }
        set
        {
            int old = attackPointsPoints;
            attackPointsPoints = value;

            if (attackPointsPoints == old) return;
            AttackPointsChanged?.Invoke(this, new AttackPointsChangedEventArgs(old, attackPointsPoints));
        }
    }

    private int immediateActionPoints;
    [JsonProperty(ImmediateActionPointsJson, DefaultValueHandling = DefaultValueHandling.Populate)]
    public int ImmediateActionPoints
    {
        get { return immediateActionPoints; }
        set
        {
            int old = immediateActionPoints;
            immediateActionPoints = value;

            if (immediateActionPoints == old) return;
            ImmediateActionPointsChanged?.Invoke(this, new ImmediateActionPointsChangedEventArgs(old, immediateActionPoints));
        }
    }

    [JsonIgnore]
    public Sprite Sprite => Resources.Load<Sprite>($"Sprites/Cards/{SpriteName}");

    public event AttackPointsChangedEventHandler AttackPointsChanged;
    public event ImmediateActionPointsChangedEventHandler ImmediateActionPointsChanged;

    public override string ToString()
    {
        return $"{Name}: AP {AttackPoints}, IAP {ImmediateActionPoints}";
    }
}
