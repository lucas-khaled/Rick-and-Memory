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
                if (property.managedReferenceValue == null || property.managedReferenceValue.GetType() != implementation)
                {
                    property.managedReferenceValue = Activator.CreateInstance(implementation);
                    property.serializedObject.ApplyModifiedProperties();
                }

                if(DrawObjectProperties(position, property)) 
                {
                    MethodInfo cloneMethod = property.managedReferenceValue.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                    property.managedReferenceValue = cloneMethod.Invoke(property.managedReferenceValue, null);
                }
            }

            if (property.serializedObject.ApplyModifiedProperties())
                EditorUtility.SetDirty(property.serializedObject.targetObject);

            EditorGUI.EndProperty();
        }

        private bool DrawObjectProperties(Rect position, SerializedProperty property)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var padding = EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel++;
            position.height = lineHeight;

            object obj = property.managedReferenceValue;
            if (obj == null) return false;

            folded = EditorGUI.BeginFoldoutHeaderGroup(position, folded, obj.GetType().Name);

            bool hasChange = false;
            if (folded)
            {
                position.y += lineHeight + padding;
                propertyCounts++;

                FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    position.height = lineHeight;

                    EditorGUI.BeginChangeCheck();

                    if (field.FieldType == typeof(int))
                    {
                        int newValue = EditorGUI.IntField(position, field.Name, (int)fieldValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            hasChange = true;
                            field.SetValue(obj, newValue);
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        float newValue = EditorGUI.FloatField(position, field.Name, (float)fieldValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            hasChange = true;
                            field.SetValue(obj, newValue);
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        string newValue = EditorGUI.TextField(position, field.Name, (string)fieldValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            hasChange = true;
                            field.SetValue(obj, newValue);
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        bool newValue = EditorGUI.Toggle(position, field.Name, (bool)fieldValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            hasChange = true;
                            field.SetValue(obj, newValue);
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                    {
                        UnityEngine.Object unityObj = EditorGUI.ObjectField(position, field.Name, (UnityEngine.Object)fieldValue, field.FieldType, false);
                        if (EditorGUI.EndChangeCheck())
                        {
                            hasChange = true;
                            field.SetValue(obj, unityObj);
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(position, field.Name, "Unsupported type: " + field.FieldType.Name);
                    }

                    position.y += lineHeight + padding;
                    propertyCounts++;
                }
            }

            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel--;

            return hasChange;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var padding = EditorGUIUtility.standardVerticalSpacing;

            return base.GetPropertyHeight(property, label) + (lineHeight * propertyCounts) + (padding * (propertyCounts - 1));
        }
    }
}
