using RickAndMemory.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class LayoutManager : MonoBehaviour
    {
        [SerializeField] private float verticalSpacing;
        [SerializeField] private float horizontalSpacing;
        [SerializeField] private bool preserveCardSizeRatio = true;

        private Layout layout;
        private Rect contentRect;
        private Rect cardInitialRect;
        private Vector2 cardFinalScale;
        private Vector3[] positions;

        public void SetLayout(Layout layout, Rect content, Rect cardRect) 
        {
            this.layout = layout;
            this.contentRect = content;
            this.cardInitialRect = cardRect;
            Calculate();
        }

        private void Calculate()
        {
            positions = new Vector3[layout.Amount];

            float ratio = cardInitialRect.width / cardInitialRect.height;
            float horizontalSectorSize = contentRect.width / layout.width;
            float verticalSectorSize = contentRect.height / layout.height;

            float finalWidth = (cardInitialRect.width > horizontalSectorSize - horizontalSpacing) 
                ? horizontalSectorSize - horizontalSpacing 
                : cardInitialRect.width;

            float finalHeight = (cardInitialRect.height > verticalSectorSize - verticalSpacing)
                ? verticalSectorSize - verticalSpacing
                : cardInitialRect.height;

            if (cardInitialRect.width - finalWidth > cardInitialRect.height - finalHeight)
            {
                finalHeight = finalWidth / ratio;
            }
            else
                finalWidth = finalHeight * ratio;

            cardFinalScale = new Vector2(finalWidth/cardInitialRect.width, finalHeight/cardInitialRect.height);

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

        public Vector2 GetCardScale() 
        {
            return cardFinalScale;
        }
    }
}
