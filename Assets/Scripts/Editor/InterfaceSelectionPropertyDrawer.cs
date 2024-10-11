using RickAndMemory.Attributes;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RickAndMemory.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceSelectionAttribute))]
    public class InterfaceSelectionPropertyDrawer : PropertyDrawer
    {
        private bool folded;
        private int propertyCounts;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            propertyCounts = 0;
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var padding = EditorGUIUtility.standardVerticalSpacing;

            var attribute = (InterfaceSelectionAttribute)base.attribute;

            if (!attribute.InterfaceType.IsInterface)
            {
                EditorGUI.LabelField(position, label.text, "Error: Not an interface!");
                propertyCounts++;
                return;
            }

            var implementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => attribute.InterfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();

            int currentIndex = -1;
            var options = implementations.Select(t => t.Name).ToArray();
            if (property.managedReferenceValue != null)
            {
                currentIndex = Array.IndexOf(options, property.managedReferenceValue.GetType().Name);
            }

            position.height = lineHeight;
            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, options);
            position.y += lineHeight + padding;
            propertyCounts++;

            if (newIndex >= 0 && newIndex < implementations.Length)
            {
                var implementation = implementations[newIndex];
                if(property.managedReferenceValue == null || property.managedReferenceValue.GetType() != implementation)
                    property.managedReferenceValue = Activator.CreateInstance(implementations[newIndex]);

                DrawObjectProperties(new Rect(position), property.managedReferenceValue);
            }

            EditorGUI.EndProperty();
        }

        private void DrawObjectProperties(Rect position, object obj)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var padding = EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;
            position.height = lineHeight;
            folded = EditorGUI.BeginFoldoutHeaderGroup(position, folded, obj.GetType().Name);
            
            if (folded)
            {
                position.y += lineHeight + padding;
                propertyCounts++;
                FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    position.height = lineHeight;

                    if (field.FieldType == typeof(int))
                    {
                        int newValue = EditorGUI.IntField(position, field.Name, (int)fieldValue);
                        field.SetValue(obj, newValue);
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        float newValue = EditorGUI.FloatField(position, field.Name, (float)fieldValue);
                        field.SetValue(obj, newValue);
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        string newValue = EditorGUI.TextField(position, field.Name, (string)fieldValue);
                        field.SetValue(obj, newValue);
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        bool newValue = EditorGUI.Toggle(position, field.Name, (bool)fieldValue);
                        field.SetValue(obj, newValue);
                    }
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                    {
                        UnityEngine.Object unityObj = EditorGUI.ObjectField(position, field.Name, (UnityEngine.Object)fieldValue, field.FieldType, false);
                        field.SetValue(obj, unityObj);
                    }
                    else
                        EditorGUI.LabelField(position, field.Name, "Unsupported type: " + field.FieldType.Name);

                    position.y += lineHeight + padding;
                    propertyCounts++;
                }

            }
            
            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var padding = EditorGUIUtility.standardVerticalSpacing;

            return base.GetPropertyHeight(property, label) + (lineHeight*propertyCounts) + (padding * propertyCounts-1);
        }
    }
}
