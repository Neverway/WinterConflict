using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_EventSequenceManager : MonoBehaviour
{
    private EventSequence currentEventSequence;

    public static GI_EventSequenceManager Instance => GameInstance.Get<GI_EventSequenceManager>();
    public static void SetCurrentEventSequence(EventSequence eventSequence) =>
        Instance.currentEventSequence = eventSequence;
    public static EventSequence GetCurrentEventSequence() =>
        Instance.currentEventSequence;
}
