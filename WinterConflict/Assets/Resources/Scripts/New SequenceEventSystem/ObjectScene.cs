using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles enabling and disabling a list of child game objects used as individual 'scenes' for a frame-by-frame cutscene
/// </summary>
public class ObjectScene : MonoBehaviour
{
    [Tooltip("A quick way of setting the objectScenes to the child objects in the correct order")]
    public MethodButton setObjectScenesToCurrentChildren = "Set objectScenes To Current Children";
    [Tooltip("Put each of the child game objects here in the order they will be toggled for the cutscene, " +
             "these need to be filled in the editor so that previewing the event can work correctly")]
    public List<GameObject> objectScenes = new List<GameObject>();
    [Tooltip("Used to keep track of which child object should be the currently visible one")]
    public int currentFrame;


    public void Begin()
    {
        // Set the current frame to the first
        currentFrame = 0;
        
        // Toggle the appropriate child object
        for (int i = 0; i < objectScenes.Count; i++)
        {
            if (i == currentFrame) objectScenes[i].SetActive(true);
            else objectScenes[i].SetActive(false);
        }
    }

    public void JumpTo(int _frameIndex)
    {
        // Go to the next frame
        currentFrame= _frameIndex;

        // If next is called but there are no more frames, end the cutscene
        if (currentFrame > objectScenes.Count)
        {
            End();
            return;
        }
        
        // Toggle the appropriate child object
        for (int i = 0; i < objectScenes.Count; i++)
        {
            if (i == currentFrame) objectScenes[i].SetActive(true);
            else objectScenes[i].SetActive(false);
        }
    }

    public void Next()
    {
        // Go to the next frame
        currentFrame++;

        // If next is called but there are no more frames, end the cutscene
        if (currentFrame > objectScenes.Count)
        {
            End();
            return;
        }
        
        // Toggle the appropriate child object
        for (int i = 0; i < objectScenes.Count; i++)
        {
            if (i == currentFrame) objectScenes[i].SetActive(true);
            else objectScenes[i].SetActive(false);
        }
    }

    public void End()
    {
        // Reset the index, just because I feel like it I guess
        // (Maybe this could be good if multiple 'nexts' are called after the event has ended for repeating the cutscene? Idk.)
        currentFrame = 0;
        
        // Disable all the child objects
        for (int i = 0; i < objectScenes.Count; i++)
        {
            objectScenes[i].SetActive(false);
        }
    }
    
    [ReferenceTag("Set objectScenes To Current Children")]
    public void SetObjectScenesToCurrentChildren()
    {
        objectScenes.Clear();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            objectScenes.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    public void DisableAllChildren()
    {
        // Disable all the child objects
        for (int i = 0; i < objectScenes.Count; i++)
        {
            objectScenes[i].SetActive(false);
        }
    }
}
