using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Event_Audio : Event
{
    [Tooltip("The audio to play (Can be set to none to fade out music or ambience)")]
    public AudioClip audioClip;
    public SoundChannels soundChannel=SoundChannels.SoundEffects;
    [Tooltip("Only applies to Ambient and Music sound channels")]
    public AudioTransition transition = AudioTransition.None;
    public EventConcludesWhen eventConcludesWhen = EventConcludesWhen.audioPlayed;
    public enum EventConcludesWhen
    {
        audioPlayed, // The event concludes as soon as the event is called
        audioFinished // The event concludes when the duration of the audio clip's length has passed
    }
    private GI_AudioManager audioManager;
    
    public override IEnumerator<EventSequence.Instruction> Call()
    {
        // Safety check for reference
        if (audioManager == null)
        {
            audioManager = GameInstance.Get<GI_AudioManager>();
        }

        switch (soundChannel)
        {
            case SoundChannels.Ambient:
                audioManager.SetAmbience(audioClip, transition);
                break;
            case SoundChannels.Music:
                audioManager.SetMusic(audioClip, transition);
                break;
            default:
                audioManager.PlayClip(audioClip);
                break;
        }

        if (eventConcludesWhen == EventConcludesWhen.audioFinished)
        {
            yield return new WaitForSeconds(audioClip.length);
        }
        
        yield break;
    }
}
