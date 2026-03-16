using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_EventSequenceManager : MonoBehaviour
{
    [Header("Preview Event Stuff")]
    [Tooltip("A list of all view cameras, used to disable all view cameras when previewing from a certain event")]
    [SerializeField] public static List<ViewCamera> viewCameras;
    [Tooltip("A list of all object scenes, used to disable all object scenes when previewing from a certain event")]
    [SerializeField] public static List<ObjectScene> objectScenes;
    
    private EventSequence currentEventSequence;

    public static GI_EventSequenceManager Instance => GameInstance.Get<GI_EventSequenceManager>();
    public static void SetCurrentEventSequence(EventSequence eventSequence) =>
        Instance.currentEventSequence = eventSequence;
    public static EventSequence GetCurrentEventSequence() =>
        Instance.currentEventSequence;

    public static void InitPreviewing()
    {
        foreach (var _viewCameras in viewCameras)
        {
            _viewCameras.gameObject.SetActive(false);
        }

        foreach (var _objectScenes in objectScenes)
        {
            _objectScenes.DisableAllChildren();
        }
    }
}
