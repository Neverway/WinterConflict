using UnityEngine;

//Compares Two Values together
public class If_CompareValues : EventConditional
{
    //Compare-From strategy to use (Which later defines Compare-To strategy)
    [SerializeReference, Polymorphic] public SomeValue compareFrom;
    [SerializeReference, Polymorphic] public SomeValue compareTo;

    public override bool GetComparisonResult() => compareFrom.GetComparisonResult();


}



//Base interface for ValueType<T>, does not have a defined type here, refer to ValueType<T> for more info
public interface SomeValue
{
    //A way to get the value of this type by providing a value type you expect, returns true if successfully provided type
    public bool TryGetValue<T>(out T value)
    {
        if (this is SomeValue<T> castedType)
        {
            value = castedType.GetValue();
            return true;
        }

        value = default(T);
        return false;
    }
}

//Represents a value of a certain specified type, used to make StoryFlags comparable to non-StoryFlag types for example
public interface SomeValue<T> : ValueType
{
    public T GetValue();
}