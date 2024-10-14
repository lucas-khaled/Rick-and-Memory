using RickAndMemory.Data;

namespace RickAndMemory.Core
{
    public static class StyleManager
    {
        public static Style actualStyle { get; set; }

        public static void SetStyle(Style style) 
        {
            actualStyle = style;
        }
    }
}
