using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Event_Transition : Event
{
    public EventConcludesWhen eventConcludesWhen = EventConcludesWhen.transitionStarts;
    
    public enum EventConcludesWhen
    {
        transitionStarts,
        transitionEnds,
    }
    
    [Tooltip("What kind of screen transition to play when the event is called")]
    public TransitionEvent_Type type;
    [Tooltip("How long the transition takes from start to finish (0=Use default transition duration)")]
    public float duration=0;
    private GI_TransitionManager transitionManager;
    
    public enum TransitionEvent_Type
    {
        FadeIn, // Fade in from black
        FadeOut, // Fade out (underground?) to black
        CutIn, // Cut in from black
        CutOut, // Cut out to black
    }

    public override IEnumerator Call()
    {
        if (transitionManager == null)
        {
            transitionManager = GameInstance.Get<GI_TransitionManager>();
        }

        switch (type)
        {
            case TransitionEvent_Type.FadeIn:
                if (duration != 0) transitionManager.Fadein(duration);
                else transitionManager.Fadein();
                break;
            case TransitionEvent_Type.FadeOut:
                if (duration != 0) transitionManager.Fadeout(duration);
                else transitionManager.Fadeout();
                break;
            case TransitionEvent_Type.CutIn:
                break;
            case TransitionEvent_Type.CutOut:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (eventConcludesWhen == EventConcludesWhen.transitionStarts)
        {
            yield break;
        }
        else
        {
            // Do something where you check if the fade coroutine has completed here
            Debug.Log("Hey dumdum, you need to make this actually wait!");
        }
        
    }
}
