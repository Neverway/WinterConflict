using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_CabinCycleStateController : FlagEvent<bool>
{
    public GameObject[] restCycleObjects;
    public GameObject[] investigationCycleObjects;
    
    public override void OnFlagChanged()
    {
        foreach (GameObject obj in restCycleObjects) obj.SetActive(referencedStoryFlag.Value);
        foreach (GameObject obj in investigationCycleObjects) obj.SetActive(referencedStoryFlag.Value);
    }
}