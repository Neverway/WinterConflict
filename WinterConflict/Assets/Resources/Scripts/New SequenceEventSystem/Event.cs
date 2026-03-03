using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class Event : IEnumerable<EventSequence.Instruction>
{
    [Tooltip("This is just so you can see stuff in the inspector")]
    public string eventDescription;
    public UnityEvent OnCallSucceed = new UnityEvent();
    public UnityEvent OnCallFailed = new UnityEvent();

    /// <summary> Call this event </summary>
    /// <returns>Returns true if calling the event succeed</returns>
    public abstract IEnumerator<EventSequence.Instruction> Call();

    //Implementation for IEnumerable (This just lets you enumerate through the.. 
    //  ..Call() function such as with a foreach loop)
    public IEnumerator<EventSequence.Instruction> GetEnumerator() => Call();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
