using System;
using UnityEngine;

namespace RickAndMemory.Attributes
{
    public class InterfaceSelectionAttribute : PropertyAttribute
    {
        public Type InterfaceType { get; private set; }

        public InterfaceSelectionAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}
