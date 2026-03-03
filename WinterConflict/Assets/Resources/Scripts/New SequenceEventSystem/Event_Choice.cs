using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Event_Choice : Event
{
    [TextArea] public string dialogText;
    public bool allowQuittingMenu = false;
    public UnityEvent onQuitMenu = new UnityEvent();
    public TextChoiceOption[] choices;
    private Coroutine currentChoices;

    [Serializable]
    public struct TextChoiceOption
    {
        public string choiceName;
        public UnityEvent OnChoiceSelected;
    }

    public override IEnumerator<EventSequence.Instruction> Call()
    {
        StopChoices();
        currentChoices = GameInstance.SendCoroutine(CoWaitForChoice());
        yield return currentChoices;
    }
    
    /// <summary>
    /// Cancel any current choice selection
    /// </summary>
    public void StopChoices()
    {
        if (currentChoices != null) GameInstance.StopCoroutine(currentChoices);
    }
    
    public IEnumerator CoWaitForChoice()
    {
        yield return WB_TextChoice.WaitForChoice(dialogText, allowQuittingMenu, choices.Select(choice => choice.choiceName).ToArray());
        int choiceIndex = WB_TextChoice.GetLastSelectedChoice();
        if (choiceIndex == -1)
        {
            if (allowQuittingMenu) onQuitMenu?.Invoke();
            yield break;
        }
        if (choices.IsIndexOutOfRange(choiceIndex))
        {
            Debug.LogError("Text Choice was out of range somehow? There must be a bug in the text choice code." +
                           " Maybe multiple text choices running?");
            yield break;
        }
        choices[choiceIndex].OnChoiceSelected?.Invoke();
    }
}    
