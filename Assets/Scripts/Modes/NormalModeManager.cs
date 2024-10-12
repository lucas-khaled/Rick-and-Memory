using RickAndMemory.Audio;
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
        public AudioClip winningGameClip;

        private CardsManager cardsManager;

        private Action<string> gameFinishedCallback;
        private int errors;
        private int hits;

        public string GetModeName()
        {
            return "Normal Mode";
        }

        public void SetGameEndedCallback(Action<string> callback)
        {
            gameFinishedCallback += callback;
        }

        public void StartGame(Layout layout, CardInfo[] cards)
        {
            errors = 0;
            hits = 0;
            InstantiateCardsManagerIfNeeded();

            cardsManager.gameObject.SetActive(true);
            cardsManager.SetLayout(layout);
            cardsManager.InstantiateCards(cards);
        }

        private void OnCardsFinished()
        {
            AudioManager.Instance.PlayClip(winningGameClip);
            cardsManager.gameObject.SetActive(false);
            gameFinishedCallback?.Invoke($"You've done it with {errors} errors");
        }

        private void CardsUnmatched()
        {
            errors++;
        }

        private void CardsMatched(CardInfo card)
        {
            hits++;
        }

        private void InstantiateCardsManagerIfNeeded() 
        {
            if (cardsManager != null) return;

            cardsManager = GameObject.Instantiate(cardsManagerPrefab);
            cardsManager.onCardsMatched += CardsMatched;
            cardsManager.OnCardsUnmatched += CardsUnmatched;
            cardsManager.OnCardsFinished += OnCardsFinished;
        }
    }
}
