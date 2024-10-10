using RickAndMemory.Attributes;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RickAndMemory.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceSelectionAttribute))]
    public class InterfaceSelectionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attribute = (InterfaceSelectionAttribute)base.attribute;

            if (!attribute.InterfaceType.IsInterface)
            {
                EditorGUI.LabelField(position, label.text, "Error: Not an interface!");
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

            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

            if (newIndex >= 0 && newIndex < implementations.Length)
            {
                var implementation = implementations[newIndex];
                if (typeof(UnityEngine.Object).IsAssignableFrom(implementation))
                    EditorGUI.ObjectField(position, property, implementation);
                else
                    property.managedReferenceValue = Activator.CreateInstance(implementations[newIndex]);
            }
        }
    }
}
