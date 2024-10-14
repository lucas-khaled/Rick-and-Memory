using RickAndMemory.Core;
using RickAndMemory.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static RickAndMemory.Core.StyleComponent;

[CustomEditor(typeof(StyleComponent))]
public class StyleComponentEditor : Editor
{
    private List<string> options = new();
    private int selectedOption;
    private SerializedProperty componentNameProperty;
    private StyleComponent targetComponent;

    private VisualElement eventElement;

    private void OnEnable()
    {
        targetComponent = (StyleComponent)target;
        componentNameProperty = serializedObject.FindProperty(nameof(targetComponent.componentName));
        FillOptions();
    }

    public override VisualElement CreateInspectorGUI()
    {
        Debug.Log("Creating UI");
        VisualElement container = new VisualElement();
        
        var dropdown = new DropdownField("Component Name", options, selectedOption);
        dropdown.RegisterCallback<ChangeEvent<string>>((_) => OnDropdownChanged(dropdown.index));

        container.Add(dropdown);

        SetComponentType();
        eventElement = GetCorrespondingEvent();

        container.Add(eventElement);

        serializedObject.ApplyModifiedProperties();

        return container;
    }

    private void SetComponentType()
    {
        if (string.IsNullOrEmpty(targetComponent.componentName)) return;

        var typeProperty = serializedObject.FindProperty(nameof(targetComponent.componentType));

        if(typeof(Style).GetField(targetComponent.componentName).FieldType == typeof(Sprite)) 
        {
            typeProperty.intValue = (int)ComponentType.Sprite;
        }
        else if(typeof(Style).GetField(targetComponent.componentName).FieldType == typeof(Color)) 
        {
            typeProperty.intValue = (int)ComponentType.Color;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private VisualElement GetCorrespondingEvent()
    {
        string name = targetComponent.componentType == ComponentType.Sprite ? nameof(targetComponent.SetSprite) : nameof(targetComponent.SetColor);
        Debug.Log($"Getting Corresponding event; Type {targetComponent.componentType} and name {name}");
        return new IMGUIContainer(() =>
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(name));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        });
    }

    private void OnDropdownChanged(int index) 
    {
        selectedOption = index;
        componentNameProperty.stringValue = options[index];
        serializedObject.ApplyModifiedProperties();

        SetComponentType();
        eventElement = GetCorrespondingEvent();

        eventElement.MarkDirtyRepaint();
    }

    private void FillOptions() 
    {
        options.Clear();
        FieldInfo[] infos = typeof(Style).GetFields();

        for(int i = 0; i < infos.Length; i++) 
        {
            var name = infos[i].Name;
            if (componentNameProperty.stringValue == name)
                selectedOption = i;
            options.Add(name);
        }
    }
}
