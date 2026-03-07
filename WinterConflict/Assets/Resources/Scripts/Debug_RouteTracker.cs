using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_RouteTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private Slider primarySlider, bufferMinSlider, bufferMaxSlider;
    [SerializeField] private TMP_Text route, loneWolfCount, cooperativeCount, totalCount;
    private GI_RouteTracker routeTracker;
    
    // Start is called before the first frame update
    void Start()
    {
        routeTracker = GameInstance.Get<GI_RouteTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        /*debugText.text = $"Route:<color=#667777> {CurrentRoute()}<color=#ffffff>\n                \n                " +
                         "N-Ratio:<color=#ffff00> [50/50 | 10%]<color=#ffffff>\n                " +
                         "Lone Wolf:<color=#ffff00> 0 | 0%<color=#ffffff>\n                " +
                         "Cooperative:<color=#ffff00> 0 | 0%<color=#ffffff>\n                \n                " +
                         "N-Ratio:<color=#ff5500> [50/50 | 10%]<color=#ffffff>\n                " +
                         "Trickster:<color=#ff5500> 0 | 0%<color=#ffffff>\n                " +
                         "Honest:<color=#ff5500> 0 | 0%<color=#ffffff>\n                \n                " +
                         "Player Deaths: 0\n                " +
                         "Route Fatalities: 0";*/
        float sum = routeTracker.loneWolf + routeTracker.cooperative;
        primarySlider.minValue = 0;
        primarySlider.maxValue = 1;
        if (sum > 0) primarySlider.value = ((float)routeTracker.loneWolf) / sum;
        else primarySlider.value = 0.5f;

        var half = routeTracker.primaryNRatioBuffer*0.5f;
        bufferMinSlider.minValue = 0;
        bufferMinSlider.maxValue = 1;
        bufferMinSlider.value = 0.5f-half;
        bufferMaxSlider.minValue = 0;
        bufferMaxSlider.maxValue = 1;
        bufferMaxSlider.value = 0.5f+half;

        route.text = $"Route:<color=#667777> {routeTracker.GetCurrentRoute()}<color=#ffffff>";
        loneWolfCount.text = $"Lone Wolf: {routeTracker.loneWolf.Value}";
        cooperativeCount.text = $"Cooperative: {routeTracker.cooperative.Value}";
        totalCount.text = $"{routeTracker.cooperative+routeTracker.loneWolf.Value}";
    }
}
