using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{
    [Tooltip("A list of all of the events that will be played out for this sequence")]
    [Polymorphic, SerializeReference] public List<Event> events = new List<Event>();
    [Tooltip("Used to keep track of the current event sequence coroutine")]
    private Coroutine currentEventSequenceCoroutine;

    /// <summary>
    /// Start the sequence of events
    /// </summary>
    public void Begin()
    {
        GI_EventSequenceManager.SetCurrentEvent(this);
        currentEventSequenceCoroutine = StartCoroutine(PlayThroughEvents());
    }

    /// <summary>
    /// Iterate through the events, waiting for each to conclude before moving on to the next
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayThroughEvents()
    {
        foreach (var _event in events)
        {
            yield return _event.Call();
        }
    }

    /// <summary>
    /// Stop this sequence of events
    /// </summary>
    public void End()
    {
        //GI_EventSequenceManager.SetCurrentEvent(null);
        if (currentEventSequenceCoroutine != null) StopCoroutine(currentEventSequenceCoroutine);
    }
}
