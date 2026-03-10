using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bool Flag", menuName = "StoryFlags/Bool")]
public class StoryFlagBool : StoryFlag<bool>
{
    //Contains all classes used for modifying the StoryString through Event_StoryFlag_Modify
    public static class ModifyBool
    {
        [Serializable]
        public class SetBoolTo : ModifyStoryFlagStrategy<bool>
        {
            public BoolValue boolToSet = new(true);

            protected override void ApplyTo(StoryFlag<bool> storyFlag) =>
                storyFlag.Value = boolToSet;
        }

        [Serializable]
        public class ToggleBool : ModifyStoryFlagStrategy<bool>
        {
            protected override void ApplyTo(StoryFlag<bool> storyFlag) =>
                storyFlag.Value = !storyFlag.Value;
        }
    }

    public class CompareThisBool
    {
        [Serializable]
        public class This_AND_That : CompareStrategy<bool, bool>
        {
            public bool NOT;
            public override bool Compare(bool _this, bool _that) =>
                NOT ^ (_this && _that);
        }
        [Serializable]
        public class This_OR_That : CompareStrategy<bool, bool>
        {
            public bool NOT;
            public override bool Compare(bool _this, bool _that) =>
                NOT ^ (_this || _that);
        }
        [Serializable]
        public class This_XOR_That : CompareStrategy<bool, bool>
        {
            public bool NOT;
            public override bool Compare(bool _this, bool _that) =>
                NOT ^ (_this ^ _that);
        }
    }
}

[Serializable]
public class BoolValue : SomeValue<bool>
{
    public BoolValue() { }
    public BoolValue(bool value) => boolValue = new Input(value);

    [SerializeReference, Polymorphic] public BoolValueType boolValue;
    public override bool GetValue() => boolValue.GetValue();
    public override bool HasValue() => boolValue != null;

    public static implicit operator BoolValue(bool value) => new BoolValue(value);

    public interface BoolValueType { public abstract bool GetValue(); }

    [Serializable]
    public class Input : BoolValueType
    {
        public Input() { }
        public Input(bool value) => input = value;

        public bool input;
        public bool GetValue() => input;
    }

    [Serializable]
    public class StoryFlag : BoolValueType
    {
        public StoryFlagBool storyFlag;
        public bool GetValue() => storyFlag.Value;
    }
}