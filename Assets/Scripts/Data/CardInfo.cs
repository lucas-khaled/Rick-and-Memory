using System;
using UnityEngine;

namespace RickAndMemory.Data
{
    [Serializable]
    public class CardInfo
    {
        public int id;
        public string name;
        public string cardURL;
        public Sprite imageSprite;
        public int positionIndex;

        public CardInfo() 
        {
            positionIndex = -1;
        }

        public CardInfo(CardInfo copy) 
        {
            id = copy.id;
            name = copy.name;
            cardURL = copy.cardURL;
            imageSprite = copy.imageSprite;
            positionIndex = -1;
        }
    }
}
