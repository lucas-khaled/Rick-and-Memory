using RickAndMemory.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class Card : MonoBehaviour
    {
        private CardInfo cardInfo;
        public void SetInfo(CardInfo info) 
        {
            cardInfo = info;
        }
    }
}
