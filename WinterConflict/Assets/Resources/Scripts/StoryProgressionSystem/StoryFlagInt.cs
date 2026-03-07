using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Flag", menuName = "StoryFlags/Int")]
public class StoryFlagInt : StoryFlag<int>
{
    public class CompareThisInt
    {
        [Serializable]
        public class This_Equals_That : CompareStrategy<int, int>
        {
            public override bool Compare(int _this, int _that) =>
                _this == _that;
        }
        [Serializable]
        public class This_IsGreaterThan_That : CompareStrategy<int, int>
        {
            public bool orEqualTo;
            public override bool Compare(int _this, int _that) =>
                orEqualTo ? _this >= _that : _this > _that;
        }
        [Serializable]
        public class This_IsLessThan_That : CompareStrategy<int, int>
        {
            public bool orEqualTo;
            public override bool Compare(int _this, int _that) =>
                orEqualTo ? _this <= _that : _this < _that;
        }
    }
}

[Serializable]
public class IntValue : SomeValue<int>
{
    [SerializeReference, Polymorphic] public IntValueType intValue;
    public override int GetValue() => intValue.GetValue();
    public override bool HasValue() => intValue != null;

    public interface IntValueType { public abstract int GetValue(); }

    [Serializable]
    public class Input : IntValueType
    {
        public int input;
        public int GetValue() => input;
    }

    [Serializable]
    public class StoryFlag : IntValueType
    {
        public StoryFlagInt storyFlag;
        public int GetValue() => storyFlag.Value;
    }
}