using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GI_RouteTracker : MonoBehaviour
{
    public float primaryNRatioBuffer, secondaryNRatioBuffer;
    public StoryFlagInt loneWolf, cooperative;
    public StoryFlagInt trickster, honest;
    public StoryFlagInt playerDeaths, currentFatalities;
    
    
    public int GetRouteIndex(int _choicesA, int _choicesB, float _buffer)
    {
        var result = 0;
        float buffer = _buffer * 0.5f;
        float primaryTotal = _choicesA + _choicesB;


        // Is the split within x% of 50%/50%
        if ((_choicesA / primaryTotal) > (0.5+buffer))
        {
            // a route
            result = 0;
        }
        else if ((_choicesB / primaryTotal) > (0.5+buffer))
        {
            // b route
            result = 1;
        }
        else
        {
            // c route
            result = 2;
        }

        return result;
    }
    
    public (Routes.Prim, Routes.Sec) GetRoute()
    {
        var prim = Routes.Prim.Mixed;
        var sec = Routes.Sec.Mixed;
        var routePrimIndex = GetRouteIndex(loneWolf, cooperative, primaryNRatioBuffer);
        var routeSecIndex = GetRouteIndex(trickster, honest, secondaryNRatioBuffer);

        prim = (Routes.Prim)(1<<routePrimIndex);
        sec = (Routes.Sec)(1<<routeSecIndex);
        return (prim, sec);
    }
    
    public string GetCurrentRouteAsString()
    {
        string[] primRoutes = { "LoneWolf", "Cooperative", "Mixed" };
        string[] secRoutes = { "Trickster", "Honest", "Mixed" };
        
        var route = "";
        route = primRoutes[GetRouteIndex(loneWolf, cooperative, primaryNRatioBuffer)];
        route += secRoutes[GetRouteIndex(trickster, honest, primaryNRatioBuffer)];
        return route;
    }
}

public static class Routes
{
    [System.Flags]
    public enum Prim
    {
        LoneWolf    = 1<<0,
        Cooperative = 1<<1,
        Mixed       = 1<<2,
    }
    [System.Flags]
    public enum Sec
    {
        Trickster = 1<<0,
        Honest    = 1<<1,
        Mixed     = 1<<2,
    }
}
