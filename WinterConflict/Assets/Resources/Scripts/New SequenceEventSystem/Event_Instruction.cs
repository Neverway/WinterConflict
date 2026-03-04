using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Instruction : Event
{
    [SerializeReference, Polymorphic] public EventSequence.Instruction instruction;

    public override IEnumerator<EventSequence.Instruction> Call()
    {
        yield return instruction;
    }
}
