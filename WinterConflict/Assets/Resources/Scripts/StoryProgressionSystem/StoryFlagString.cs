using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventConditional_CompareValues;
using static StoryFlagString.CompareFromString;

[CreateAssetMenu(fileName = "New String Flag", menuName = "StoryFlags/String")]
public class StoryFlagString : StoryFlag<string>, ValueType<string>
{
    public string GetValue() => Value;

    //Contains all classes used for modifying the StoryString through Event_StoryFlag_Modify
    [Serializable]
    public class ModifyString : ModifyStoryFlagStrategy 
    {
        public StoryFlagString storyFlag;
        [SerializeReference, Polymorphic] 
        public ModifyStrategy modifyStrategy;

        public override void Apply()
        {
            if (modifyStrategy == null)
                throw new Exception("No strategy was set for modifying Story Flag on an event");

            modifyStrategy.ApplyTo(storyFlag);
        }

        [Serializable]
        public abstract class ModifyStrategy
        {
            public abstract void ApplyTo(StoryFlagString storyFlag);
        }

        //Sets StoryFlag to given string
        [Serializable]
        public class SetTo : ModifyStrategy
        {
            public string newString;

            public override void ApplyTo(StoryFlagString storyFlag) => storyFlag.Set(newString);
        }
    }

    [Serializable]
    public class CompareFromString : CompareFromType<ValueType<string>, CompareTo_Base>
    {
        public abstract class CompareTo_Base : CompareTo_StrategyBase<ValueType<string>> { }
        public abstract class CompareTo<TToValue> : CompareTo_Base
        {
            //Second value
            [SerializeReference, Polymorphic] public TToValue to_Value;

            //Compare-To method to be defined
            public override bool GetComparisonResult(ValueType<string> from) => GetComparisonResult(from, to_Value);
            public abstract bool GetComparisonResult(ValueType<string> from, TToValue to);
        }




        public class IsEqualToString : CompareTo<ValueType<string>>
        {
            public override bool GetComparisonResult(ValueType<string> from, ValueType<string> to) =>
                from == to;
        }
    }
}

[Serializable]
public class InputStringValue : ValueType<string>
{
    public string inputString;
    public string GetValue() => inputString;
}
[Serializable]
public class StoryFlagStringValue : ValueType<string>
{
    StoryFlagString storyFlag;
    public string GetValue() => storyFlag.Value;
}