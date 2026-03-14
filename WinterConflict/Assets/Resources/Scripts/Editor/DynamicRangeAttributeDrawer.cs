using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DynamicRangeAttribute))]
public class DynamicRangeAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (attribute is not DynamicRangeAttribute dynamicRangeAttribute) return;
        object parent = PolymorphicDrawer.GetParent(property);
        
        if (!ReferenceTagAttribute.TryGet(dynamicRangeAttribute.endValueReference, out MethodInfo method)) return;
        int maxValue = (int)method.Invoke(parent, null);
        EditorGUI.IntSlider(position, property, 0, maxValue, label);
        
        if (!ReferenceTagAttribute.TryGet(dynamicRangeAttribute.getLabelReference, out method)) return;
        string yormLabel = (string)method.Invoke(parent, null);
        position.y += EditorGUIUtility.singleLineHeight;
        var guistyle = new GUIStyle("LargeLabel");
        guistyle.richText = true;
        EditorGUI.SelectableLabel(position, yormLabel, guistyle);
    }
}
