using UnityEngine;

public class If_RandomChance : EventConditional
{
    [Tooltip("If the result is less or equal to this, the random chance will succeed")]
    [SerializeField] private IntValue percentChanceToSucceed = new(50);
    [Tooltip("The resulting random number will be between 0 and this")]
    [SerializeField] private IntValue outOf = new(100);

    public override bool GetComparisonResult()
    {
        return Random.Range(0, outOf) <= percentChanceToSucceed;
    }
}
