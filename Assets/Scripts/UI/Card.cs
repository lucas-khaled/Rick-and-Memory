using RickAndMemory.Data;
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
