using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_EventSequenceManager : MonoBehaviour
{
    private static EventSequence currentEventSequence;
    
    public static void SetCurrentEventSequence(EventSequence eventSequence)
    {
        currentEventSequence = eventSequence;
    }
    
    public static EventSequence GetCurrentEventSequence()
    {
        return currentEventSequence;
    }
}
