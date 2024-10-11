using RickAndMemory.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class LayoutManager : MonoBehaviour
    {
        private Layout layout;
        private Rect contentRect;
        private Vector3[] positions;

        public void SetLayout(Layout layout, Rect content) 
        {
            this.layout = layout;
            this.contentRect = content;
            CalculatePositions();
        }

        private void CalculatePositions()
        {
            positions = new Vector3[layout.Amount];
            float horizontalSectorSize = contentRect.width / layout.width;
            float verticalSectorSize = contentRect.height / layout.height;

            for(int row = 0; row < layout.width; row++) 
            {
                for(int column = 0; column < layout.height; column++) 
                {
                    int index = row * layout.width + column;
                    float xPos = horizontalSectorSize * 0.5f + horizontalSectorSize*column;
                    float yPos = verticalSectorSize * 0.5f + verticalSectorSize*row;

                    positions[index] = new Vector3(xPos, yPos);
                }
            }
        }

        public Vector3 GetPosition(int index) 
        {
            return positions[index];
        }
    }
}
