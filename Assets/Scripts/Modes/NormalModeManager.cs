using RickAndMemory.Audio;
using RickAndMemory.Core;
using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Modes
{
    public class NormalModeManager : IModeManager
    {
        public CardsManager cardsManagerPrefab;
        public NormalInGameUIManager UIManagerPrefab;
        public AudioClip winningGameClip;
        public int baseScorePerMatch = 100;
        public int baseStreakBonus = 20;

        private CardsManager cardsManager;
        private NormalInGameUIManager UIManager;

        private Action<string> gameFinishedCallback;
        private int errors;
        private int score;
        private int streak;
        private Layout layout;
        private List<CardInfo> cardInfos;

        public Action OnUpdate { get; set; }

        public string GetModeName()
        {
            return "Normal Mode";
        }

        public void SetGameEndedCallback(Action<string> callback)
        {
            gameFinishedCallback += callback;
        }

        public void StartGame(Layout layout, List<CardInfo> cards, int errors = 0, int score = 0)
        {
            this.layout = layout;
            this.cardInfos = cards;

            this.errors = errors;
            this.score = score;

            InstantiateUIManagerIfNeeded();
            InstantiateCardsManagerIfNeeded();

            UIManager.gameObject.SetActive(true);
            UIManager.SetErrors(errors);
            UIManager.SetScore(score);

            cardsManager.gameObject.SetActive(true);
            cardsManager.SetLayout(layout);
            cardsManager.InstantiateCards(cardInfos);

            OnUpdate?.Invoke();
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
            streak = 0;
            errors++;

            UIManager.SetErrors(errors);
            OnUpdate?.Invoke();
        }

        private void CardsMatched(CardInfo card1, CardInfo card2)
        {
            score += baseScorePerMatch + baseStreakBonus * streak;
            streak++;

            UIManager.SetScore(score);
            cardInfos.Remove(card1);
            cardInfos.Remove(card2);

            OnUpdate?.Invoke();
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

        public SaveInfo GetSaveInfo()
        {
            return new SaveInfo
            {
                mode = GetModeName(),
                layout = layout,
                cardsInfo = cardInfos,
                score = score,
                errors = errors
            };
        }
    }
}
