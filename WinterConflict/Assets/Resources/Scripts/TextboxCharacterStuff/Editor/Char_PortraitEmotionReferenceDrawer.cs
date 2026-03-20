using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// BEWARE: ERRYNEI WITCHCRAFT BELOW!!!!
// Errynei will try to say that the forbidden tongues are 'bullshit',
// they are correct. (Good luck)

//[CustomPropertyDrawer(typeof(Char_PortraitEmotionReference))]
public class Char_PortraitEmotionReferenceDrawer : PropertyDrawer
{
    private bool hasFoundMYASS = false;
    private const float Padding = 4f;
    private const float PreviewSize = 64f;
    private string selectedEmotion = "";
    private List<Sprite> spriteList = new List<Sprite>();
    private Char_PortraitProfile portraitProfile;
    private int selectedSpriteIndex = 0;
    private string portraitProfileClassRef = nameof(TextFrames.portraitProfile); // <== This needs to be the same as GI_TextboxManager's portraitProfile reference
    
    public void GetSpriteList(SerializedProperty property)
    {
        
        hasFoundMYASS = false;
        try
        {
            var parentObject = PolymorphicDrawer.GetParent(property);
            portraitProfile = YormExtensionMethods.GetFieldValue<Char_PortraitProfile>(parentObject, portraitProfileClassRef);
        
            spriteList = portraitProfile.portraitEmotes.Select(e => e.portrait).ToList();
            for (int i = 0; i < portraitProfile.portraitEmotes.Length; i++)
            {
                if (portraitProfile.portraitEmotes[i].emotionName == selectedEmotion) selectedSpriteIndex = i;
            }

            hasFoundMYASS = true;
        }
        catch
        {
            // Throw a ball for the code dog >>> ()
            hasFoundMYASS = false;
        }
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Wtf am I wrong with me?! ~Errynei 2026
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property, GUIContent.none);
        
        var emotionNameField = property.FindPropertyRelative(nameof(Char_PortraitEmotionReference.emotionName));
        
        selectedEmotion = emotionNameField.stringValue;
        // The fitnessgram pacer test is a multi-stage aerobic capacity test...
        GetSpriteList(property);

        // Draw label
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);
        if (hasFoundMYASS is false)
        {
            EditorGUI.EndProperty();
            return;
        }

        float yOffset = position.y + EditorGUIUtility.singleLineHeight + Padding;

        int count = spriteList.Count;
        int columns = Mathf.Max(1, Mathf.FloorToInt(position.width / (PreviewSize + Padding)));

        for (int i = 0; i < count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            Rect rect = new Rect(
                position.x + col * (PreviewSize + Padding),
                yOffset + row * (PreviewSize + Padding),
                PreviewSize,
                PreviewSize
            );

            var sprite = spriteList[i];

            if (sprite != null)
            {
                Texture2D preview = AssetPreview.GetAssetPreview(sprite);

                GUIContent content = preview != null 
                    ? new GUIContent(preview) 
                    : GUIContent.none;

                if (GUI.Button(rect, content))
                {
                    Debug.Log("I love you");
                    selectedSpriteIndex = i;
                    property.serializedObject.ApplyModifiedProperties();
                }

                // Highlight selected
                if (selectedSpriteIndex == i)
                {
                    Handles.color = Color.green;
                    Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 1, 0, 0.2f), Color.green);
                }
            }
        }

        selectedEmotion = portraitProfile.portraitEmotes[selectedSpriteIndex].emotionName;
        emotionNameField.stringValue = selectedEmotion;
        
        emotionNameField.serializedObject.ApplyModifiedProperties();
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (hasFoundMYASS is false)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        int count = spriteList.Count;
        int columns = Mathf.Max(1, Mathf.FloorToInt(EditorGUIUtility.currentViewWidth / (PreviewSize + Padding)));
        int rows = Mathf.CeilToInt((float)count / columns);

        return rows * (PreviewSize + Padding) + EditorGUIUtility.singleLineHeight;
    }
}




public static class YormExtensionMethods
{
    public static object GetFieldValue(object obj, string fieldName)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(fieldName)) throw new ArgumentException("Field name is null or empty");

        Type type = obj.GetType();

        FieldInfo field = type.GetField(
            fieldName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        if (field == null)
            throw new MissingFieldException(type.FullName, fieldName);

        return field.GetValue(obj);
    }
    public static T GetFieldValue<T>(object obj, string fieldName)
    {
        object value = GetFieldValue(obj, fieldName);

        if (value is T typedValue)
            return typedValue;

        throw new InvalidCastException(
            $"Field '{fieldName}' is not of type {typeof(T)}"
        );
    }
}