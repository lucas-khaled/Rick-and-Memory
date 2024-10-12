using RickAndMemory.Audio;
using RickAndMemory.Core;
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
        public NormalInGameUIManager UIManagerPrefab;
        public AudioClip winningGameClip;
        public int baseScorePerMatch = 100;

        private CardsManager cardsManager;
        private NormalInGameUIManager UIManager;

        private Action<string> gameFinishedCallback;
        private int errors;
        private int score;

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
            score = 0;

            InstantiateUIManagerIfNeeded();
            InstantiateCardsManagerIfNeeded();

            UIManager.gameObject.SetActive(true);
            UIManager.SetErrors(errors);
            UIManager.SetScore(score);

            cardsManager.gameObject.SetActive(true);
            cardsManager.SetLayout(layout);
            cardsManager.InstantiateCards(cards);
        }

        private void OnCardsFinished()
        {
            AudioManager.Instance.PlayClip(winningGameClip);

            cardsManager.gameObject.SetActive(false);
            UIManager.gameObject.SetActive(false);

            gameFinishedCallback?.Invoke($"You've done it with {score} score and {errors} errors");
        }

        private void CardsUnmatched()
        {
            errors++;

            UIManager.SetErrors(errors);
        }

        private void CardsMatched(CardInfo card)
        {
            score += baseScorePerMatch;

            UIManager.SetScore(score);
        }

        private void InstantiateUIManagerIfNeeded()
        {
            if (UIManager != null) return;

            UIManager = GameObject.Instantiate(UIManagerPrefab);
        }

        private void InstantiateCardsManagerIfNeeded() 
        {
            if (cardsManager != null) return;

            cardsManager = GameObject.Instantiate(cardsManagerPrefab, UIManager.transform);
            cardsManager.onCardsMatched += CardsMatched;
            cardsManager.OnCardsUnmatched += CardsUnmatched;
            cardsManager.OnCardsFinished += OnCardsFinished;
        }
    }
}
