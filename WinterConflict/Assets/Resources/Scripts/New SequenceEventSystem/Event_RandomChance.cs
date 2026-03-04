using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_RandomChance : Event
{
    [Tooltip("If the result is less or equal to this, the random chance will succeed")]
    [SerializeField] private float percentChanceToSucceed = 50;
    [Tooltip("The resulting random number will be between 0 and this")]
    [SerializeField] private float outOf = 100;
    [Tooltip("If the random chance succeeds, the current event sequence will end, and this one will begin")]
    [SerializeReference,Polymorphic] private EventSequence.Instruction OnSuccess;

    public override IEnumerator<EventSequence.Instruction> Call()
    {
        
        var result =  Random.Range(0, 100);
        // Event succeeded, end current sequence
        if (result <= percentChanceToSucceed)
        {
            yield return OnSuccess;
        }
        // Event failed, continue current sequence
        else
        {
            yield break;
        }
    }
}
