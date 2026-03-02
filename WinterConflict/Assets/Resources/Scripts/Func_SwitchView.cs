using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Func_SwitchView : MonoBehaviour
{
    public Camera viewCamera;
    public bool doNotPlayTransition = false;
    private GI_TransitionManager transitionManager;

    public void CallEvent()
    {
        if (transitionManager == null)
        {
            transitionManager = GameInstance.Get<GI_TransitionManager>();
        }
        
        if (doNotPlayTransition == false) transitionManager.Fadein(0.2f);
        if (Camera.current) Camera.current.enabled = false;
        viewCamera.enabled = true;
    }
}
