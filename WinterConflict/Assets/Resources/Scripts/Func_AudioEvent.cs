using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Func_AudioEvent : MonoBehaviour
{
    [Tooltip("The audio to play (Can be set to none to fade out music or ambience)")]
    public AudioClip audioClip;
    public SoundChannels soundChannel=SoundChannels.SoundEffects;
    [Tooltip("Only applies to Ambient and Music sound channels")]
    public AudioTransition transition = AudioTransition.None;
    private GI_AudioManager audioManager;
    
    public void CallEvent()
    {
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
    }
}
