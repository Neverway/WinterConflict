using System;
using UnityEngine;
using static EventConditional_CompareValues;

//Compares Two Values together
public class EventConditional_CompareValues : EventConditional
{
    //Compare-From strategy to use (Which later defines Compare-To strategy)
    [SerializeReference, Polymorphic] public CompareFrom_StrategyBase compareFrom;

    public override bool GetComparisonResult() => compareFrom.GetComparisonResult();



    //First part of the comparison, just a base, CompareFromType<TFromValue> extends it and defines first value type there
    public abstract class CompareFrom_StrategyBase
    {
        public abstract bool GetComparisonResult();
    }
    //Second part of the comparison, just a base, CompareStrategy<TFromValue, TToValue> extends it and defines comparison types there
    public abstract class CompareTo_StrategyBase<TFromValue>
    {
        public abstract bool GetComparisonResult(TFromValue from);
    }

}
//Defines the first value type to compare, and then asks for what to compare it to
[Serializable]
public abstract class CompareFromType<TFromValue, TCompareToBase> : CompareFrom_StrategyBase 
    where TFromValue : ValueType
    where TCompareToBase : CompareTo_StrategyBase<TFromValue>
{
    //First value
    [SerializeReference, Polymorphic] public TFromValue from_Value;
    //Compare-To strategy to use
    [SerializeReference, Polymorphic] public TCompareToBase compareTo;

    public override bool GetComparisonResult() => compareTo.GetComparisonResult(from_Value);
}

[Serializable]
public abstract class CompareStrategy<TFromValue, TToValue> : CompareTo_StrategyBase<TFromValue> where TToValue : ValueType
{
    //Second value
    [SerializeReference, Polymorphic] public TToValue to_Value;

    //Compare-To method to be defined
    public override bool GetComparisonResult(TFromValue from) => GetComparisonResult(from, to_Value);
    public abstract bool GetComparisonResult(TFromValue from, TToValue to);
}

//Base interface for ValueType<T>, does not have a defined type here, refer to ValueType<T> for more info
public interface ValueType
{
    //A way to get the value of this type by providing a value type you expect, returns true if successfully provided type
    public bool TryGetValue<T>(out T value)
    {
        if (this is ValueType<T> castedType)
        {
            value = castedType.GetValue();
            return true;
        }

        value = default(T);
        return false;
    }
}

//Represents a value of a certain specified type, used to make StoryFlags comparable to non-StoryFlag types for example
public interface ValueType<T> : ValueType
{
    public T GetValue();
}