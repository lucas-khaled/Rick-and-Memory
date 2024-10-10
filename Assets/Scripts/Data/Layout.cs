using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Data
{
    public struct Layout
    {
        public int width;
        public int height;
        public int Amount => width * height;

        public Layout(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
