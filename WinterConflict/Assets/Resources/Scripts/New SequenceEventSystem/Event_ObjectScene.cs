using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows itterating through an object scene
/// </summary>
public class Event_ObjectScene : Event
{
    public ObjectScene objectScene;

    public ObjectSceneAction objectSceneAction;

    public int jumpToFrame;
    
    public enum ObjectSceneAction
    {
        begin, // Enables the first object group in this scene
        next, // Enables the next object group in this scene (if this is called on the last scene, this will end the scene)
        jumpTo, // Enables the target frame
        end, // Disables all the object groups
    }
        
    public override void OnPreviewEvent()
    {
        foreach (var _objectScene in GameObject.FindObjectsOfType<ObjectScene>())
        {
            if (_objectScene != objectScene) _objectScene.End();
        }
        switch (objectSceneAction)
        {
            case ObjectSceneAction.begin:
                objectScene.Begin();
                break;
            case ObjectSceneAction.jumpTo:
                objectScene.JumpTo(jumpToFrame);
                break;
            case ObjectSceneAction.next:
                objectScene.Next();
                break;
            case ObjectSceneAction.end:
                objectScene.End();
                break;
        }
    }
    
    public override IEnumerator<EventSequence.Instruction> Call()
    {
        switch (objectSceneAction)
        {
            case ObjectSceneAction.begin:
                objectScene.Begin();
                break;
            case ObjectSceneAction.jumpTo:
                objectScene.JumpTo(jumpToFrame);
                break;
            case ObjectSceneAction.next:
                objectScene.Next();
                break;
            case ObjectSceneAction.end:
                objectScene.End();
                break;
        }
        yield break;
    }
}
