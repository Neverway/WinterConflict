using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class EventConditional : Event
{
    [SerializeReference, Polymorphic] public EventSequence.Instruction OnSucceed;
    [SerializeReference, Polymorphic] public EventSequence.Instruction OnFail;

    public override IEnumerator<EventSequence.Instruction> Call()
    {
        bool conditionResult = GetComparisonResult();

        if (conditionResult)
            yield return OnSucceed;
        else
            yield return OnFail;
    }

    public abstract bool GetComparisonResult();
}
