using System;

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
