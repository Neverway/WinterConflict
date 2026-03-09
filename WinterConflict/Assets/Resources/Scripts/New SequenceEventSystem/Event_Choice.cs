using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Event_Choice : Event
{
    [Tooltip("The prompt the player is presented with")]
    [TextArea] public string dialogText;
    [Tooltip("The choices going North, South, East, West")]
    public TextChoiceOption[] choices;
    [Tooltip("How long the player has to respond before the game auto selects (0 is infinite time)")]
    public float duration;
    
    // This stuff isn't needed for WiCo
    [Tooltip("Whether the player can just close out of the choice box")]
    [HideInInspector] public bool allowQuittingMenu = false;
    [Tooltip("Fired when the player closed out of the choice box prematurely")]
    [HideInInspector] public UnityEvent onQuitMenu = new UnityEvent();
    
    // I dunno
    private Coroutine currentChoices;

    [Serializable]
    public struct TextChoiceOption
    {
        public string choiceName;
        [SerializeReference,Polymorphic] public EventSequence.Instruction OnChoiceSelected;
    }

    public override IEnumerator<EventSequence.Instruction> Call()
    {
        yield return new EventSequence.Instruction.EnumeratorYield(
            WB_TextChoice.WaitForChoice(dialogText, allowQuittingMenu, choices.Select(choice => choice.choiceName).ToArray()));
            
        int choiceIndex = WB_TextChoice.GetLastSelectedChoice();
        if (choiceIndex == -1)
        {
            if (allowQuittingMenu) onQuitMenu?.Invoke();
            yield break;
        }
        Debug.Log(choiceIndex);
        Debug.Log(choices[choiceIndex]);
        if (choices.IsIndexOutOfRange(choiceIndex))
        {
            Debug.LogError("Text Choice was out of range somehow? There must be a bug in the text choice code." +
                           " Maybe multiple text choices running?");
            yield break;
        }
        yield return choices[choiceIndex].OnChoiceSelected;
    }
}    
