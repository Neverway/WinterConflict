using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New String Flag", menuName = "StoryFlags/String")]
public class StoryFlagString : StoryFlag<string>
{
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

    public class CompareThisString
    {
        [Serializable]
        public class This_Contains_That : CompareStrategy<string, string>
        {
            public override bool Compare(string _this, string _that) =>
                _this.Contains(_that);
        }
        [Serializable]
        public class This_StartsWith_That : CompareStrategy<string, string>
        {
            public override bool Compare(string _this, string _that) =>
                _this.StartsWith(_that);
        }
        [Serializable]
        public class This_EndsWith_That : CompareStrategy<string, string>
        {
            public override bool Compare(string _this, string _that) =>
                _this.EndsWith(_that);
        }

        [Serializable]
        public class This_Has_That_ManyLetters : CompareStrategy<string, int>
        {
            public override bool Compare(string _this, int _that) =>
                _this.Length == _that;
        }
    }
}

[Serializable]
public class StringValue : SomeValue<string>
{
    [SerializeReference, Polymorphic] public StringValueType stringValue;
    public override string GetValue() => stringValue.GetValue();
    public override bool HasValue() => stringValue != null;

    public interface StringValueType { public abstract string GetValue(); }

    [Serializable]
    public class Input : StringValueType
    {
        public string input;
        public string GetValue() => input;
    }

    [Serializable]
    public class StoryFlag : StringValueType
    {
        public StoryFlagString storyFlag;
        public string GetValue() => storyFlag.Value;
    }
}