using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_RouteTracker : MonoBehaviour
{
    public float primaryNRatioBuffer, secondaryNRatioBuffer;
    public int loneWolf, cooperative;
    public int trickster, honest;
    public int playerDeaths, currentFatalities;


    public int GetRoute(int _choicesA, int _choicesB, float _buffer)
    {
        int result = 0;
        
        float buffer = _buffer * 0.5f;
        float primaryTotal = _choicesA + _choicesB;


        // Is the split within x% of 50%/50%
        if ((_choicesA / primaryTotal) > (0.5+buffer))
        {
            // a route
            result = 1;
        }
        else if ((_choicesB / primaryTotal) > (0.5+buffer))
        {
            // b route
            result = 2;
        }
        else
        {
            // c route
            result = 3;
        }

        return result;
    }
    
    public string GetCurrentRoute()
    {
        string route = "";
        switch (GetRoute(loneWolf, cooperative, primaryNRatioBuffer))
        {
            case 1:
                route = "LoneWolf";
                break;
            case 2:
                route = "Cooperative";
                break;
            case 3:
                route = "Mixed";
                break;
        }
        switch (GetRoute(trickster, honest, primaryNRatioBuffer))
        {
            case 1:
                route += "Trickster";
                break;
            case 2:
                route += "Honest";
                break;
            case 3:
                route += "Mixed";
                break;
        }
        return route;
    }
}
