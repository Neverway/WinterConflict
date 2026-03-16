using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRangeAttribute : PropertyAttribute
{
    public string endValueReference;
    public string getLabelReference;
    
    public DynamicRangeAttribute(string endValue)
    {
        endValueReference = endValue;
    }
    public DynamicRangeAttribute(string endValue, string getLabel)
    {
        endValueReference = endValue;
        getLabelReference = getLabel;
    }
}
