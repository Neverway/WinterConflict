using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MethodButton))]
public class MethodButtonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        if (property.boxedValue is MethodButton button)
        {
            object obj = PolymorphicDrawer.GetParent(property);
            label.text = button.methodReferenceTag;

            if (GUILayout.Button(label) && obj != null)
                button.Invoke(obj);
        }
        
        EditorGUI.EndProperty();
    }
}
