using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_EventSequenceManager : MonoBehaviour
{
    private static EventSequence currentEventSequence;
    
    public static void SetCurrentEvent(EventSequence eventSequence)
    {
        if (currentEventSequence)
        {
            currentEventSequence.End();
        }

        currentEventSequence = eventSequence;
    }
    
    public static EventSequence GetCurrentEvent()
    {
        return currentEventSequence;
    }
}
