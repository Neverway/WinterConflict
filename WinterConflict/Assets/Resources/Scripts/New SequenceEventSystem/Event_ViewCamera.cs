using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Event_ViewCamera : Event
{
    public Camera viewCamera;
    public bool doNotPlayTransition = false;
    private GI_TransitionManager transitionManager;
    
    public override IEnumerator<EventSequence.Instruction> Call()
    {
        // Safety check for reference
        if (transitionManager == null)
        {
            transitionManager = GameInstance.Get<GI_TransitionManager>();
        }
        
        // View switch transition
        if (doNotPlayTransition == false)
        {
            transitionManager.Fadecross();

            while (transitionManager.transitionInProgress)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        
        // Disable original camera
        if (Camera.current)
        {
            Camera.current.gameObject.SetActive(false);
        }
        
        // Enable new camera
        viewCamera.gameObject.SetActive(true);
        
        // Done
        yield break;
    }
}
