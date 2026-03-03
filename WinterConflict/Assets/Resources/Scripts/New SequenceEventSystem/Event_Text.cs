using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Event_Text : Event
{
    public EventConcludesWhen eventConcludesWhen = EventConcludesWhen.textboxClosed;
    
    public enum EventConcludesWhen
    {
        textboxOpened,
        frameIsDonePrinting,
        textboxClosed,
    }
    
    public bool overrideExistingEvents;
    public TextEvent textEvent;
    private GI_TextboxManager textboxManager;
    
    public override IEnumerator<EventSequence.Instruction> Call()
    {
        if (textboxManager == null)
        {
            textboxManager = GameInstance.Get<GI_TextboxManager>();
        }
        
        textboxManager.TryStartTextEvent(textEvent, overrideExistingEvents);

        var eventConcluded = false;

        if (eventConcludesWhen == EventConcludesWhen.textboxOpened)
        {
            textboxManager.OnTextboxStarted += (() => {eventConcluded = true;});
        }

        else if (eventConcludesWhen == EventConcludesWhen.frameIsDonePrinting)
        {
            textboxManager.OnPrintFrameCompleted += (() => {eventConcluded = true;});
        }

        else if (eventConcludesWhen == EventConcludesWhen.textboxClosed)
        {
            textboxManager.OnTextboxEnded += (() => {eventConcluded = true;});
        }

        while (eventConcluded == false)
        {
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
