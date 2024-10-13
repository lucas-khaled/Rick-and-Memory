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
        public Action OnUpdate { get; set; }

        protected Action<string> gameFinishedCallback;
        protected int errors;
        protected int score;
        protected Layout layout;
        protected List<CardInfo> cardInfos;

        protected CardsManager cardsManager;
        protected BaseInGameUIManager UIManager;

        public abstract string GetModeName();

        protected abstract BaseInGameUIManager GetUIManagerPrefab();
        protected abstract CardsManager GetCardsManagerPrefab();
        protected abstract void CardsMatched(CardInfo card1, CardInfo card2);
        protected abstract void CardsUnmatched();
        protected abstract void OnCardsFinished();

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
