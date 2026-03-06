using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event_UnityEvent : Event
{
    public UnityEvent unityEvent = new UnityEvent();
    public override IEnumerator<EventSequence.Instruction> Call()
    {
        unityEvent?.Invoke();
        yield break;
    }
}
