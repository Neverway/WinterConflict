using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If_Route : EventConditional
{
    public Routes.Prim primRoute;
    public Routes.Sec secRoute;

    public override bool GetComparisonResult()
    {
        var currentRoute = GameInstance.Get<GI_RouteTracker>().GetRoute();
        var primCheck = (currentRoute.Item1 & primRoute) != 0;
        var secCheck = (currentRoute.Item2 & secRoute) != 0;


        return primCheck && secCheck;
    }
}
