using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_StoryFlag_Modify : Event
{
    [SerializeReference, Polymorphic] public StoryFlag storyFlag;
    [SerializeReference, Polymorphic("ModifyFilter")] public ModifyStoryFlagStrategy modifyStrategy;


    public override IEnumerator<EventSequence.Instruction> Call()
    {
        if (modifyStrategy == null || storyFlag == null)
        {
            Debug.LogError("Fields were not fully setup for \"Event_StoryFlag_Modify\" event. " +
                "Skipping event");
            yield break;
        }
        if (!StoryFlagTypeMatchesModifyStrategyGenericType(storyFlag, modifyStrategy.GetType()))
        {
            Debug.LogError("Storyflag modify strategy does not match type of" +
                " \"Event_StoryFlag_Modify\" event. Skipping event");
            yield break;
        }

        modifyStrategy.ApplyTo(storyFlag);
    }


    [ReferenceTag("ModifyFilter")]
    private bool ModifyFilter(Type modifyType)
    {
        if (modifyType == null || modifyType.BaseType == null)
            throw new Exception("This shouldn't ever happen, what????");

        return StoryFlagTypeMatchesModifyStrategyGenericType(storyFlag, modifyType);
    }

    public static bool StoryFlagTypeMatchesModifyStrategyGenericType(StoryFlag storyFlag, Type modifyType)
    {
        //If storyflag don't exist, don't display any types in modify strategy list
        if (storyFlag == null)
            return false;

        Type storyFlagType = storyFlag.GetFlagType();
        modifyType = modifyType.BaseType; //Use base type instead, since it should have the generic arguments

        //Validate that modifyType is valid
        if (modifyType != null &&
            modifyType.IsGenericType &&
            modifyType.GetGenericTypeDefinition() == typeof(ModifyStoryFlagStrategy<>))
        {
            //Get the generic arguments of the ModifyStoryFlagStrategy type thats trying to be displayed in the list..
            // ..if it doesn't have EXACTLY 1 argument for the storyflag type, then it can't be trusted >:O
            Type[] targetGenericArgs = modifyType.GetGenericArguments();
            if (targetGenericArgs.Length != 1)
                return false;

            //Make sure the storyflag type actually corresponds to the ModifyStoryFlagStrategy's generic argument
            if (targetGenericArgs[0].IsAssignableFrom(storyFlagType))
            {
                //You've passed the seven trials, you, my worthy ModifyStoryFlagStrategy Type, may be now be listed as a valid Type
                return true;
            }
        }
        //YOU FAIL, LEAVE MY PREMISES, YOU SHALL NOT BE LISTED
        return false;
    }
}

[Serializable]
public abstract class ModifyStoryFlagStrategy
{
    public abstract void ApplyTo(StoryFlag storyFlag);
}
[Serializable]
public abstract class ModifyStoryFlagStrategy<TFlagType> : ModifyStoryFlagStrategy
{
    public override void ApplyTo(StoryFlag storyFlag) =>
        ApplyTo(storyFlag as StoryFlag<TFlagType>);
    protected abstract void ApplyTo(StoryFlag<TFlagType> storyFlag);
}