using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_StoryFlag_Modify : Event
{
    [SerializeReference, Polymorphic] public ModifyStoryFlagStrategy storyFlagModifyStrategy;
    public override IEnumerator<EventSequence.Instruction> Call()
    {
        if (storyFlagModifyStrategy == null) yield break;

        storyFlagModifyStrategy.Apply();
    }

    [Serializable]
    public abstract class CompareMethod<TStoryFlag>
    {
        public abstract bool Compare(TStoryFlag storyFlag);
    }

    [Serializable]
    public abstract class CompareStrategy
    {
        public abstract bool GetComparisonResult();
    }

    [Serializable]
    public abstract class CompareStrategy<TStoryFlag, TCompareMethod> : CompareStrategy
        where TStoryFlag : StoryFlag
        where TCompareMethod : CompareMethod<TStoryFlag>
    {
        public TStoryFlag storyFlag;
        [SerializeReference, Polymorphic] public TCompareMethod compareStrategy;
    }
}

[Serializable]
public abstract class ModifyStoryFlagStrategy
{
    public abstract void Apply();
}