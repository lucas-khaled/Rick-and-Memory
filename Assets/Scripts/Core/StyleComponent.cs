using RickAndMemory.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace RickAndMemory.Core
{
    public class StyleComponent : MonoBehaviour
    {
        public string componentName;
        public ComponentType componentType;

        public UnityEvent<Sprite> SetSprite;
        public UnityEvent<Color> SetColor;

        private void Awake()
        {
            StyleManager.OnStyleSet += SetStyle;
            SetStyle();
        }

        private void SetStyle() 
        {
            if (StyleManager.actualStyle == null) return;

            FieldInfo info = typeof(Style).GetField(componentName);

            if (info == null)
            {
                Debug.LogWarning($"Component {componentName} not found in Style");
                return;
            }

            object value = info.GetValue(StyleManager.actualStyle);

            switch (componentType) 
            {
                case ComponentType.Sprite:
                    if(value is not Sprite sprite) 
                    {
                        Debug.LogWarning($"The value is not a {componentType}");
                        return;
                    }

                    SetSprite?.Invoke(sprite);
                    break;

                case ComponentType.Color:
                    if (value is not Color color)
                    {
                        Debug.LogWarning($"The value is not a {componentType}");
                        return;
                    }

                    SetColor?.Invoke(color);
                    break;
            }
        }

        public enum ComponentType 
        {
            Color,
            Sprite
        }
    }
}
