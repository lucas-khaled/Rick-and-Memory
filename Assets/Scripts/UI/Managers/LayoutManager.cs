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
        private Vector2 cardFinalSize;
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

            if (preserveCardSizeRatio)
            {
                if (cardInitialRect.width - finalWidth > cardInitialRect.height - finalHeight)
                {
                    finalHeight = finalWidth / ratio;
                }
                else
                    finalWidth = finalHeight * ratio;
            }

            cardFinalSize = new Vector2(finalWidth, finalHeight);

            for(int x = 0; x < layout.height; x++) 
            {
                for(int y = 0; y < layout.width; y++) 
                {
                    int index = x * layout.width + y;
                    float xPos = horizontalSectorSize * 0.5f + horizontalSectorSize*y;
                    float yPos = verticalSectorSize * 0.5f + verticalSectorSize*x;

                    positions[index] = new Vector3(xPos, yPos);
                }
            }
        }

        public Vector3 GetPosition(int index) 
        {
            return positions[index];
        }

        public Vector2 GetCardSize() 
        {
            return cardFinalSize;
        }
    }
}
