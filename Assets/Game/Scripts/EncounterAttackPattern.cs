public abstract class EncounterAttackPattern
{
    protected Encounter Encounter { get; }
    public abstract void Attack();

    protected EncounterAttackPattern(Encounter encounter)
    {
        Encounter = encounter;
    }
}