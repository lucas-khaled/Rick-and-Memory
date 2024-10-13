using RickAndMemory.Audio;
using RickAndMemory.Core;
using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public abstract class BaseModeManager : IModeManager
    {
        public int baseScorePerMatch = 100;
        public int baseStreakBonus = 20;
        public AudioClip winningGameClip;

        public Action OnUpdate { get; set; }

        protected Action<string> gameFinishedCallback;
        protected int streak;
        protected int errors;
        protected int score;
        protected Layout layout;
        protected List<CardInfo> cardInfos;

        protected CardsManager cardsManager;
        protected BaseInGameUIManager UIManager;


        public abstract string GetModeName();

        protected abstract BaseInGameUIManager GetUIManagerPrefab();
        protected abstract CardsManager GetCardsManagerPrefab();

        protected virtual void CardsMatched(CardInfo card1, CardInfo card2) 
        {
            score += baseScorePerMatch + baseStreakBonus * streak;
            streak++;

            UIManager.SetScore(score);
            cardInfos.Remove(card1);
            cardInfos.Remove(card2);

            OnUpdate?.Invoke();
        }

        protected virtual void CardsUnmatched() 
        {
            streak = 0;
            errors++;

            UIManager.SetErrors(errors);
            OnUpdate?.Invoke();
        }

        protected virtual void OnCardsFinished() 
        {
            AudioManager.Instance.PlayClip(winningGameClip);

            cardsManager.gameObject.SetActive(false);
            UIManager.gameObject.SetActive(false);

            gameFinishedCallback?.Invoke($"You've done it with {score} score and {errors} errors");
        }

        public virtual SaveInfo GetSaveInfo()
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

        public void SetGameEndedCallback(Action<string> callback)
        {
            gameFinishedCallback += callback;
        }

        public virtual void StartGame(Layout layout, List<CardInfo> cards, int errors = 0, int score = 0)
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

        private void InstantiateUIManagerIfNeeded()
        {
            if (UIManager != null) return;

            UIManager = GameObject.Instantiate(GetUIManagerPrefab());
        }

        private void InstantiateCardsManagerIfNeeded()
        {
            if (cardsManager != null) return;

            cardsManager = GameObject.Instantiate(GetCardsManagerPrefab(), UIManager.transform);
            cardsManager.onCardsMatched += CardsMatched;
            cardsManager.OnCardsUnmatched += CardsUnmatched;
            cardsManager.OnCardsFinished += OnCardsFinished;
        }
    }
}
