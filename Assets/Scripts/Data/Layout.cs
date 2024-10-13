using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Data
{
    [Serializable]
    public struct Layout
    {
        public int width;
        public int height;
        public int TotalAmount => width * height;
        public int DifferentCardAmount => Mathf.FloorToInt(width * height * 0.5f);

        public Layout(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
