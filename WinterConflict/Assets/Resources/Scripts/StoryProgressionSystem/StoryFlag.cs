using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryFlag : ScriptableObject 
{
    public abstract Type GetFlagType();
}
public abstract class StoryFlag<T> : StoryFlag
{
    public T Value 
    { 
        get
        {
            return _value;
        }
        set
        {
            if (logOnValueChanged) 
                Debug.Log($"StoryFlag {name} changed : {_value} -> {value}", this);

            _value = value;
        }
    }

    [SerializeField] private T _value;
    [SerializeField] private bool logOnValueChanged = false;

    public override Type GetFlagType() => typeof(T);

    public static implicit operator T(StoryFlag<T> storyFlag) => storyFlag._value;
    public T Set(T newValue) => _value = newValue;
}
