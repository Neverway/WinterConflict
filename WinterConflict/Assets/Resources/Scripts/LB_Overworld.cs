using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB_Overworld : MonoBehaviour
{
    public Func_TextEvent startingTextEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        startingTextEvent.textEvent.TryDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
