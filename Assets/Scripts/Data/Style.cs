using System;
using UnityEngine;

namespace RickAndMemory.Data
{
    [Serializable]
    public class Style
    {
        [Header("Card")]
        public Sprite cardCover;
        public Sprite cardbackgound;
        public Color cardBackgroundColor;

        [Header("Background")]
        public Sprite backgroundSprite;
        public Color backgroundColor;

        [Header("Colors")]
        public Color textColor1;
        public Color textColor2;
        public Color buttonsColor1;
        public Color buttonsColor2;
    }
}
