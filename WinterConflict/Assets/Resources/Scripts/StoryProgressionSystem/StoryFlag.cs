using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryFlag : ScriptableObject 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Initialize()
    {
        StoryFlag[] storyFlags = Resources.LoadAll<StoryFlag>("");

        foreach (StoryFlag storyFlag in storyFlags)
            storyFlag.ResetValue();
    }
    public abstract void ResetValue();
    public abstract Type GetFlagType();
}
public abstract class StoryFlag<T> : StoryFlag
{
    public T Value 
    { 
        get
        {
            return _currentValue;
        }
        set
        {
            if (logOnValueChanged) 
                Debug.Log($"StoryFlag {name} changed : {_currentValue} -> {value}", this);

            _currentValue = value;
        }
    }
    [SerializeField] private T _defaultValue;
    [SerializeField] private T _currentValue;
    [SerializeField] private bool logOnValueChanged = false;

    public override Type GetFlagType() => typeof(T);
    public override void ResetValue() => _currentValue = _defaultValue;

    public static implicit operator T(StoryFlag<T> storyFlag) => storyFlag._currentValue;
    protected T Set(T newValue) => _currentValue = newValue;
}
