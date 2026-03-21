using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlagEvent<T> : MonoBehaviour
{
    public StoryFlag<T> referencedStoryFlag;
    public T lastRecordedValue;

    public void Start()
    {
        lastRecordedValue = referencedStoryFlag;
    }

    public void Update()
    {
        if (referencedStoryFlag.Value.Equals(lastRecordedValue))
        {
            lastRecordedValue = referencedStoryFlag;
            OnFlagChanged();
        }
    }

    public abstract void OnFlagChanged();
}
