using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.UI;
using System;
using UnityEngine;

namespace RickAndMemory.Modes
{
    public class NormalModeManager : IModeManager
    {
        public CardsManager cardsManagerPrefab;

        private CardsManager cardsManager;

        public string GetModeName()
        {
            return "Normal Mode";
        }

        public void SetGameEndedCallback(Action<string> callback)
        {
            
        }

        public void StartGame(Layout layout, CardInfo[] cards)
        {
            InstantiateCardsManagerIfNeeded();

            cardsManager.SetLayout(layout);
            cardsManager.InstantiateCards(cards);
        }

        private void InstantiateCardsManagerIfNeeded() 
        {
            if (cardsManager != null) return;

            cardsManager = GameObject.Instantiate(cardsManagerPrefab);
        }
    }
}
