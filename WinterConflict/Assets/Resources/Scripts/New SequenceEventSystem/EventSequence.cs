using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{
    [Tooltip("A list of all of the events that will be played out for this sequence")]
    [Polymorphic, SerializeReference] public List<Event> events = new List<Event>();
    [Tooltip("Used to keep track of the current event sequence coroutine")]
    private Coroutine currentEventSequenceCoroutine;

    [Tooltip("")]
    private bool IsRunning => currentEventStack != null;
    private Stack<Event> currentEventStack = new Stack<Event>();
    private Event currentEvent;

    /// <summary> Start the sequence of events </summary>
    public void Begin()
    {
        //If the event is already running, something is not right, log error and end event properly
        if (IsRunning)
        {
            Debug.LogError("Trying to begin an event that has already begun, " +
                "or, was not ended properly. Ending it now, but something might be broken");
            End();
        }

        GI_EventSequenceManager.SetCurrentEventSequence(this);
        currentEventSequenceCoroutine = StartCoroutine(PlayThroughEvents());
    }

    /// <summary>
    /// Iterate through the events, waiting for each to conclude before moving on to the next
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayThroughEvents()
    {
        //If null events or no events assigned, just end here, do nothing
        if (events == null || events.Count == 0) yield break;

        //Create the event stack
        NewEventStack();

        //Loop through all Events on this EventSequence
        while (IsRunning && currentEventStack.IsNotEmpty())
        {
            //Take the next event off of the stack
            currentEvent = currentEventStack.Pop();

            //If the event is null, maybe you forgot to input an event?, lets just continue but warn you
            if (currentEvent == null)
            {
                Debug.LogError($"Null event was found on {name}'s EventSequence. " +
                    $"Continuing EventSequence as if Event was not there", this);
                continue;
            }

            //Loop through the event, stopping at each "yield return"ed instruction for processing
            foreach (Instruction instruction in currentEvent)
            {
                yield return ProcessInstruction(instruction);

                //If Coroutine was stopped OR event was stopped due to..
                //  ..instruction or other means, stop processing this event
                if (!IsRunning || currentEvent == null) break;
            }
        }
    }

    /// <summary> Stop this sequence of events </summary>
    public void End()
    {
        EndCurrentEvent();
        currentEventStack = null;

        GI_EventSequenceManager.SetCurrentEventSequence(null);
        if (currentEventSequenceCoroutine != null) 
            StopCoroutine(currentEventSequenceCoroutine);

        currentEventSequenceCoroutine = null;
    }

    /// <summary> Restart this sequence of events </summary>
    protected void Restart() { End(); Begin(); } //This might be sloppy, 2 coroutines exist for breif moment

    /// <summary> Stops the current event </summary>
    protected void EndCurrentEvent() => currentEvent = null;

    /// <summary> Adds all events into a stack to be processed </summary>
    protected Stack<Event> NewEventStack()
    {
        currentEventStack = new Stack<Event>();

        //Loop backwards through the events and push each one onto the stack so 1st event is at the top
        for (int i = events.Count - 1; i >= 0; i--)
            currentEventStack.Push(events[i]);

        return currentEventStack;
    }

    //Process an EventSequence Instruction given through an Event
    protected IEnumerator ProcessInstruction(Instruction instruction)
    {
        // On No Instruction: -----
        //   - Just return null for coroutine as well (waits a frame)
        if (instruction == null)
            yield return null;

        // On Yield-Returnable Instruction: -----
        //   - Yield Return the instruction itself
        if (instruction is Instruction.IYieldReturnable yieldReturnable)
            yield return yieldReturnable.ToYieldReturn();

        // On Executeable Instruction: -----
        //   - Restart this current sequence
        if (instruction is Instruction.IExecuteable executable)
            executable.DoInstruction(this);
    }

    //These are all the instructions for an Event telling the EventSequence what to do
    [Serializable]
    public abstract class Instruction
    {
        //Instructions marked "IExecuteable" will execute upon receiving the instruction
        public interface IExecuteable { public void DoInstruction(EventSequence sequence); }

        //Instructions marked "IYieldReturnable" will be "yield return"ed in the EventSequence
        public interface IYieldReturnable { public IEnumerator ToYieldReturn(); }


        //Automagically converts Unity's YieldInstructions (which Unity uses for Coroutines) into an..
        //  ..EventSequence.Instruction so you just have to do "yield return new WaitForEndOfFrame()"..
        //  ..instead of "yield return new EventSequence.Instruction.CoroutineYield(new WaitForEndOfFrame())"
        public static implicit operator Instruction(YieldInstruction yieldInstruction) =>
            new CoroutineYield(yieldInstruction);

        //Wraps a YieldInstruction into a EventSequence.Instruction to pass to the Coroutine
        [Serializable]
        public class CoroutineYield : Instruction, IYieldReturnable
        {
            public YieldInstruction yieldInstruction;
            public CoroutineYield(YieldInstruction yieldInstruction) =>
                this.yieldInstruction = yieldInstruction;

            public IEnumerator ToYieldReturn()
            { 
                yield return yieldInstruction;
            }
        }

        //Ends the EventSequence prematurely
        [Serializable]
        public class EndSequence : Instruction, IExecuteable
        {
            public void DoInstruction(EventSequence sequence) => sequence.End();
        }

        //Restarts the entire EventSequence from the beginning
        [Serializable]
        public class RestartSequence : Instruction, IExecuteable
        {
            public void DoInstruction(EventSequence sequence) => sequence.Restart();
        }

        //Restarts the currentEvent from the beginning
        [Serializable]
        public class RestartThisEvent : Instruction, IExecuteable
        {
            public void DoInstruction(EventSequence sequence)
            {
                //Push current event back onto the stack
                sequence.currentEventStack.Push(sequence.currentEvent);
                sequence.EndCurrentEvent();
            }
        }


        //Automagically converts an EventSequence into an instruction that waits for that EventSequence to finish
        public static implicit operator Instruction(EventSequence eventSequence) =>
            new WaitForNewEventSequence(eventSequence);

        //Pauses the current EventSequence, and continues when new given EventSequence finishes
        [Serializable]
        public class WaitForNewEventSequence : Instruction, IYieldReturnable
        {
            public EventSequence newEventSequence;

            private WaitForNewEventSequence() { }
            public WaitForNewEventSequence(EventSequence newEventSequence) => 
                this.newEventSequence = newEventSequence;

            public IEnumerator ToYieldReturn()
            {
                if (newEventSequence == GI_EventSequenceManager.GetCurrentEventSequence())
                    throw new Exception("EventSequence is already running? Not sure how to handle " +
                        "this situation since starting it again will abort the IEnumerator progress " +
                        "up until this point. Did you mean to call " +
                        "EventSequence.Instruction.RestartThisEvent?");

                //Store a reference to the old EventSequence
                EventSequence oldSequence = GI_EventSequenceManager.GetCurrentEventSequence();
                GI_EventSequenceManager.SetCurrentEventSequence(null);

                //Start the new EventSequence
                newEventSequence.Begin();

                //Wait for it to finish
                while (newEventSequence.IsRunning)
                    yield return null;

                //Resume the old EventSequence
                GI_EventSequenceManager.SetCurrentEventSequence(oldSequence);
            }
        }

        //
        [Serializable]
        public class EndCurrentAndStartNewSequence : Instruction, IExecuteable
        {
            public EventSequence newEventSequence;
            public EndCurrentAndStartNewSequence(EventSequence newEventSequence) =>
                this.newEventSequence = newEventSequence;

            public void DoInstruction(EventSequence sequence)
            {
                //End current EventSequence
                sequence.End();

                //Start the new EventSequence
                newEventSequence.Begin();
            }
        }
    }
}
