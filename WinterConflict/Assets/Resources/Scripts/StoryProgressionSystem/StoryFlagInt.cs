using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Flag", menuName = "StoryFlags/Int")]
public class StoryFlagInt : StoryFlag<int>
{
    //Contains all classes used for modifying the StoryString through Event_StoryFlag_Modify
    public static class ModifyInt
    {
        [Serializable]
        public class SetIntTo : ModifyStoryFlagStrategy<int>
        {
            public IntValue boolToSet = new(1);

            protected override void ApplyTo(StoryFlag<int> storyFlag) =>
                storyFlag.Set(boolToSet);
        }

        [Serializable]
        public class AddToInt : ModifyStoryFlagStrategy<int>
        {
            public IntValue intToAdd = new(1);

            protected override void ApplyTo(StoryFlag<int> storyFlag) =>
                storyFlag.Set(storyFlag + intToAdd);
        }

        [Serializable]
        public class MultiplyIntBy : ModifyStoryFlagStrategy<int>
        {
            public IntValue intToMultiplyBy = new(1);

            protected override void ApplyTo(StoryFlag<int> storyFlag) =>
                storyFlag.Set(storyFlag * intToMultiplyBy);
        }
    }

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
    public IntValue() { }
    public IntValue(int value) => intValue = new Input(value);

    [SerializeReference, Polymorphic] public IntValueType intValue;
    public override int GetValue() => intValue.GetValue();
    public override bool HasValue() => intValue != null;

    public interface IntValueType { public abstract int GetValue(); }

    [Serializable]
    public class Input : IntValueType
    {
        public Input() { }
        public Input(int value) => input = value;

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