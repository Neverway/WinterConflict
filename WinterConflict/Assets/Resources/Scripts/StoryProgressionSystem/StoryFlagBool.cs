using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Bool Flag", menuName = "StoryFlags/Bool")]
public class StoryFlagBool : StoryFlag<bool>
{
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
    [SerializeReference, Polymorphic] public BoolValueType boolValue;
    public override bool GetValue() => boolValue.GetValue();
    public override bool HasValue() => boolValue != null;

    public interface BoolValueType { public abstract bool GetValue(); }

    [Serializable]
    public class Input : BoolValueType
    {
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