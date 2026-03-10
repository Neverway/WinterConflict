using System.Collections.Generic;
using UnityEngine;

public class Event_Instruction : Event, IHasEventConnections
{
    [SerializeReference, Polymorphic] public EventSequence.Instruction instruction;

    public override IEnumerator<EventSequence.Instruction> Call()
    {
        yield return instruction;
    }

    public EventConnection[] GetEventConnections(EventSequence source) => 
        instruction.GetEventConnections(source);
}
