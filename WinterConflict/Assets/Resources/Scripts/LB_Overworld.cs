using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB_Overworld : MonoBehaviour
{
    public EventSequence startingEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        startingEvent.Begin();
    }
}
