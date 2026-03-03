using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Contains all classes used for comparing the StoryString through Event_StoryFlag_Compare
    [Serializable]
    public static class CompareString
    {

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