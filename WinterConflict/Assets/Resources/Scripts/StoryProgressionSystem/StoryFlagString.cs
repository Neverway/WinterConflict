using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New String Flag", menuName = "StoryFlags/String")]
public class StoryFlagString : StoryFlag<string>
{
    //Contains all classes used for modifying the StoryString through Event_StoryFlag_Modify
    public static class ModifyString
    {
        //Sets StoryFlag to given string
        [Serializable]
        public class SetStringTo : ModifyStoryFlagStrategy<string>
        {
            public StringValue newString;

            protected override void ApplyTo(StoryFlag<string> storyFlag) =>
                storyFlag.Value = newString;
        }

        [Serializable]
        public class AppendString : ModifyStoryFlagStrategy<string>
        {
            public StringValue appendedString;

            protected override void ApplyTo(StoryFlag<string> storyFlag) =>
                storyFlag.Value = $"{storyFlag}{appendedString}";
        }

        [Serializable]
        public class PrependString : ModifyStoryFlagStrategy<string>
        {
            public StringValue prependedString;

            protected override void ApplyTo(StoryFlag<string> storyFlag) =>
                storyFlag.Value = $"{prependedString}{storyFlag}";
        }
    }

    public static class CompareThisString
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
        public class This_Has_That_ManyCharacters : CompareStrategy<string, int>
        {
            public override bool Compare(string _this, int _that) =>
                _this.Length == _that;
        }
    }
}

[Serializable]
public class StringValue : SomeValue<string>
{
    public StringValue() { }
    public StringValue(string value) => stringValue = new Input(value);

    [SerializeReference, Polymorphic] public StringValueType stringValue;
    public override string GetValue() => stringValue.GetValue();
    public override bool HasValue() => stringValue != null;

    public interface StringValueType { public abstract string GetValue(); }

    [Serializable]
    public class Input : StringValueType
    {
        public Input() { }
        public Input(string value) => input = value;

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