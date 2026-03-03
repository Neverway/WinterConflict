using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

// ReSharper disable once HollowTypeName
public class GI_AudioManager : MonoBehaviour
{
    public static GI_AudioManager Instance => GameInstance.Get<GI_AudioManager>();


    //========Audio Clips========//
    [Header("Audio Tracks")]

    //==========Music============//
    [Header("Music Tracks")]

    //===========================//
    
    // AudioSource for general sfx
    [SerializeField] private AudioSource soundSource;
    /// <summary>
    /// Unique audio source for playing sword slash sounds, so that they will be interruptible.
    /// </summary>
    [SerializeField] private AudioSource slashSource;
    /// <summary>
    /// Unique audio source for looping music.
    /// </summary>
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;

    /// <summary>
    ///Initialize some things.
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    ///Play the given sound effect once at the specified volume.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogError ("AudioClip wasn't assigned.");
            return;
        }
        soundSource.PlayOneShot(clip, volume);
    }

    // Start playing the specified song, unless the song is already playing, in which case do nothing
    public void SetMusic(AudioClip song, AudioTransition transition=AudioTransition.None)
    {
        switch (transition)
        {
            case AudioTransition.None:
                musicSource.clip = song;
                musicSource.Play();
                break;
            case AudioTransition.CrossFade:
                musicSource.DOFade(0, 0.5f);
                musicSource.clip = song;
                musicSource.Play();
                musicSource.DOFade(1, 0.5f);
                break;
            case AudioTransition.FadeIn:
                musicSource.volume = 0f;
                musicSource.clip = song;
                musicSource.Play();
                musicSource.DOFade(1, 1);
                break;
        }
    }

    // Start playing the specified song, unless the song is already playing, in which case do nothing
    public void SetAmbience(AudioClip song, AudioTransition transition=AudioTransition.None)
    {
        switch (transition)
        {
            case AudioTransition.None:
                ambientSource.clip = song;
                ambientSource.Play();
                break;
            case AudioTransition.CrossFade:
                ambientSource.DOFade(0, 0.5f);
                ambientSource.clip = song;
                ambientSource.Play();
                ambientSource.DOFade(1, 0.5f);
                break;
            case AudioTransition.FadeIn:
                ambientSource.volume = 0f;
                ambientSource.clip = song;
                ambientSource.Play();
                ambientSource.DOFade(1, 1);
                break;
        }
    }
}
    
public enum SoundChannels
{
    Ambient,
    CharacterChatter,
    Menus,
    Music,
    SoundEffects,
}
public enum AudioTransition
{
    None,
    CrossFade,
    FadeIn,
}