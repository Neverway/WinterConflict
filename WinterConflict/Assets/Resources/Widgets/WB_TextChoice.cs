using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WB_TextChoice : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The template widget for a choice")]
    public WidgetSelectable_TMPText textChoiceTemplate;

    public WidgetSelectable_TMPText selectableNeutralChoice;
    public WidgetSelectable_TMPText[] selectableTextChoices;
    [Tooltip("The widget navigator for the choices the player can select")]
    public WidgetNavigator choiceNavigator;
    [Tooltip("Used to set the question the player is prompted with")]
    public TextMeshProUGUI dialogText;
    [Tooltip("The root of the time limit object so we can hide or show it based on if this is a timed choice")]
    public GameObject timeLimitObject;
    [Tooltip("The progress bars for the time remaining (It's an array of them so I can mirror one and set both to make it shrink inwards)")]
    public Image[] timeLimitProgressBars;
    
    [Tooltip("I have no idea, I think this was for actually checking what option the player selected")]
    [Reload] private static int lastSelectedChoice;

    public void SetupChoices(string dialog, string[] choices)
    {
        dialogText.text = dialog;
        
        // This old code is for setting the choices dynamically, WiCo doesn't use this, so I am gonna set it on fire
        /*
         /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\ <<< This is fire
        choiceNavigator.selectableElements.Clear();
        for (int i = 0; i < choices.Length; i++)
        {
            var textChoice = Instantiate(textChoiceTemplate, choiceNavigator.transform, false);
            textChoice.gameObject.SetActive(true);
            textChoice.SetText(choices[i]);
            textChoice.SetSelected(i == 0); //select first index
            int choiceToSelect = i;
            textChoice.OnInteracted.AddListener(() => lastSelectedChoice = choiceToSelect);
            choiceNavigator.selectableElements.Add(textChoice);
        }*/

        //choiceNavigator.selectableElements.Clear(); // Clear any old data
        //choiceNavigator.selectableElements.Add(neutralChoice);
        // Select the first element in the list
        //choiceNavigator.selectableElements[0].SetSelected(true);

        choiceNavigator.SetIsNavigating(true);
        
        for (int i = 0; i < 4; i++)
        {
            var thisSelectableChoice = selectableTextChoices[i];
            thisSelectableChoice.OnInteracted.RemoveAllListeners();
            if (choices.IsIndexInRange(i))
            {
                thisSelectableChoice.disableInteraction = false;
                thisSelectableChoice.SetText(choices[i]);
                int choiceToSelect = i; // This is needed to avoid 'i' being set to 4 (Errynei tells me it's to do with lamba witchcraft)
                thisSelectableChoice.OnInteracted.AddListener(() => lastSelectedChoice = choiceToSelect);
            }
            else
            {
                thisSelectableChoice.disableInteraction = true;
                thisSelectableChoice.SetText("");
            }
        }
        
        choiceNavigator.SetIsNavigating(true);
    }
    
    public static IEnumerator WaitForChoice(string dialog, params string[] choices) 
        => WaitForChoice(dialog, false, choices);
    
    public static IEnumerator WaitForChoice(string dialog, bool allowQuittingMenu, params string[] choices)
    {
        lastSelectedChoice = -1;
        var widgetManager = GameInstance.Get<GI_WidgetManager>();
        if (choices.IsEmptyOrNull())
        {
            Debug.LogWarning("Trying to display text choices with no choices? Not really able to display that, skipping");
            yield break;
        }
        if (!widgetManager.TryAddOrGetWidget(out WB_TextChoice textChoiceManager))
        {
            Debug.LogError("Trying to display textchoice but was unable to add or get the widget");
            yield break;
        }

        textChoiceManager.SetupChoices(dialog, choices); //Setup widget

        while (lastSelectedChoice < 0)
        {
            if (allowQuittingMenu)
            {
                if (GameInstance.Inputs.Action.WasPressedThisFrame() || GameInstance.Inputs.Select.WasPressedThisFrame())
                    break;
            }
            yield return null; //Wait for selection
        }

        Destroy(textChoiceManager.gameObject); //Get rid of widget
        yield return null;
    }

    public static int GetLastSelectedChoice() => lastSelectedChoice;
    
}
