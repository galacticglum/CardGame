using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

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


public class Card : Entity
{
    public static string AssetFilePath => Path.Combine(Application.streamingAssetsPath, "Cards");
    public const string AttackPointsPropertyJson = "attack_points";
    public const string ImmediateActionPointsJson = "immediate_action_points";

    private int attackPointsP;
    [JsonProperty(AttackPointsPropertyJson, Required = Required.Always)]
    public int AttackPoints
    {
        get { return attackPointsP; }
        set
        {
            int old = attackPointsP;
            attackPointsP = value;

            if (attackPointsP == old) return;
            AttackPointsChanged?.Invoke(this, new AttackPointsChangedEventArgs(old, attackPointsP));
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

    public override Sprite Sprite => Resources.Load<Sprite>(Path.Combine(AssetFilePath, SpriteName));

    public event AttackPointsChangedEventHandler AttackPointsChanged;
    public event ImmediateActionPointsChangedEventHandler ImmediateActionPointsChanged;

    public override string ToString()
    {
        return $"{Name}: AP {AttackPoints}, IAP {ImmediateActionPoints}";
    }
}
