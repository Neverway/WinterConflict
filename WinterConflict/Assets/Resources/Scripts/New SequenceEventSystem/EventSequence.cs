using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{
    [Polymorphic, SerializeReference] public List<Event> events = new List<Event>();
    public UnityEvent OnStarted = new UnityEvent();
    public UnityEvent OnInterrupted = new UnityEvent();
    public UnityEvent OnFinished = new UnityEvent();

    /// <summary>
    /// Start the sequence of events
    /// </summary>
    public void Begin()
    {
        StartCoroutine(PlayThroughEvents());
    }

    public IEnumerator PlayThroughEvents()
    {
        foreach (var _event in events)
        {
            yield return _event.Call();
        }
    }
}
