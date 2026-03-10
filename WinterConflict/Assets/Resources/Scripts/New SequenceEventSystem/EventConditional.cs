using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class EventConditional : Event, IReferenceEventSequence
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
    public EventSequence[] GetConnectedEventSequences()
    {
        var yorm = new EventSequence.Instruction[] { OnSucceed, OnFail };
        return yorm
            .OfType<IReferenceEventSequence>()
            .SelectMany((e) => e.GetConnectedEventSequences())
            .Where((e) => e != null)
            .ToArray();
    }
}
