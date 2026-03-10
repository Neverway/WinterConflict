using System;
using System.Linq;
using UnityEngine;

//Compares Two Values together
public class If_CompareValues : EventConditional
{
    //Compare-From strategy to use (Which later defines Compare-To strategy)
    [Space]
    [Space]
    [SerializeReference, Polymorphic] public SomeValue thisValue;
    [Space]
    [SerializeReference, Polymorphic] public SomeValue thatValue;
    [Space]
    [SerializeReference, Polymorphic("CompareFilter")] public CompareStrategy comparison;

    public override bool GetComparisonResult()
    {
        if (comparison == null || thisValue == null || thatValue == null)
            throw new Exception("One of your fields on an \"If_CompareValues\" event are not filled in on object" +
                ", cannot determine compare result");

        if (!ValueTypesMatchCompareStrategyGenericTypes(thisValue, thatValue, comparison.GetType()))
            throw new Exception("CompareStrategy in field on an \"If_CompareValues\" event does not match" +
                " types of \"thisValue\" and \"thatValue\" provided.");

        if (!thisValue.TryGetValue(out object thisObject))
            throw new Exception("Could not retrieve value from \"thisValue\" field on an \"If_CompareValues\" event");

        if (!thatValue.TryGetValue(out object thatObject))
            throw new Exception("Could not retrieve value from \"thatValue\" field on an \"If_CompareValues\" event");

        return comparison.GetComparisonResult(thisObject, thatObject);
    }

    //Filters the list of Polymorphic fields 
    [ReferenceTag("CompareFilter")]
    public bool CompareFilter(Type compareType)
    {
        //If somehow the Type given is null, or if it somehow has no base type, return false (shouldnt ever happen)
        if (compareType == null || compareType.BaseType == null)
            throw new Exception("This shouldn't ever happen, what????");

        //Everything else either doesn't have matching types, or isn't generic, so don't display it in the list
        return ValueTypesMatchCompareStrategyGenericTypes(thisValue, thatValue, compareType);
    }

    public static bool ValueTypesMatchCompareStrategyGenericTypes(SomeValue _this, SomeValue _that, Type compareType)
    {
        //If "this" or "that" values don't exist, don't display any types in compare strategy list
        if (_this == null || _that == null)
            return false;

        Type thisType = _this.GetValueType();
        Type thatType = _that.GetValueType();
        compareType = compareType.BaseType; //Use base type instead, since it should have the generic arguments

        //Validate that compareType is valid
        if (compareType != null &&
            compareType.IsGenericType &&
            compareType.GetGenericTypeDefinition() == typeof(CompareStrategy<,>))
        {
            //Get the generic arguments of the CompareStrategy type thats trying to be displayed in the list..
            // ..if it doesn't have EXACTLY 2 arguments for the "this" type and "that" type, then it can't be trusted >:O
            Type[] targetGenericArgs = compareType.GetGenericArguments();
            if (targetGenericArgs.Length != 2)
                return false;

            //Make sure the "this"/"that" value types actually correspond to the CompareStrategies "this"/"that" generic arguments
            if (targetGenericArgs[0].IsAssignableFrom(thisType) &&
                targetGenericArgs[1].IsAssignableFrom(thatType))
            {
                //You've passed the seven trials, you, my worthy CompareStrategy Type, may be now be listed as a valid Type
                return true; 
            }
        }
        //YOU FAIL, LEAVE MY PREMISES, YOU SHALL NOT BE LISTED
        return false;
    }
}

public abstract class CompareStrategy 
{
    public abstract bool GetComparisonResult(object _this, object _that);
}
public abstract class CompareStrategy<TThisValue, TThatValue> : CompareStrategy
{
    public override bool GetComparisonResult(object _this, object _that) =>
        Compare((TThisValue)_this, (TThatValue)_that);
    public abstract bool Compare(TThisValue _this, TThatValue _that);
}

//Base interface for ValueType<T>, does not have a defined type here, refer to ValueType<T> for more info
public abstract class SomeValue
{
    public abstract bool TryGetValue(out object value);
    public abstract Type GetValueType();
    public abstract bool HasValue();
}

//Represents a value of a certain specified type, used to make StoryFlags comparable to non-StoryFlag types for example
public abstract class SomeValue<T> : SomeValue
{
    public override bool TryGetValue(out object value)
    {
        if (HasValue())
        {
            value = GetValue();
            return true;
        }
        value = null;
        return false;
    }
    public override Type GetValueType() => typeof(T);
    public abstract T GetValue();


    public static implicit operator T(SomeValue<T> value) => 
        value == null ? default : value.GetValue();
}