using RickAndMemory.Data;
using System;

namespace RickAndMemory.Core
{
    public static class StyleManager
    {
        public static Action OnStyleSet { get; set; }
        public static Style actualStyle { get; set; }

        public static void SetStyle(Style style) 
        {
            actualStyle = style;
            OnStyleSet?.Invoke();
        }
    }
}
