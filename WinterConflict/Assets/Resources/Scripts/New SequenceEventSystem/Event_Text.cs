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
    
    public override IEnumerator Call()
    {
        if (textboxManager == null)
        {
            textboxManager = GameInstance.Get<GI_TextboxManager>();
        }

        var result = textboxManager.TryStartTextEvent(textEvent, overrideExistingEvents);
        if (result is false)
        {
            OnCallFailed.Invoke();
        }

        if (eventConcludesWhen == EventConcludesWhen.textboxOpened)
        {
            Debug.Log("Hey dumdum, you need to make this actually wait!");
            yield break;
        }

        if (eventConcludesWhen == EventConcludesWhen.frameIsDonePrinting)
        {
            Debug.Log("Hey dumdum, you need to make this actually wait!");
            yield break;
        }

        if (eventConcludesWhen == EventConcludesWhen.textboxClosed)
        {
            Debug.Log("Hey dumdum, you need to make this actually wait!");
            yield break;
        }
    }
}
