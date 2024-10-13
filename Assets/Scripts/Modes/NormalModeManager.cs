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
    public class NormalModeManager : BaseModeManager
    {
        public CardsManager cardsManagerPrefab;
        public NormalInGameUIManager UIManagerPrefab;
        public AudioClip winningGameClip;
        public int baseScorePerMatch = 100;
        public int baseStreakBonus = 20;

        private int streak;

        public override string GetModeName()
        {
            return "Normal Mode";
        }

        protected override void OnCardsFinished()
        {
            AudioManager.Instance.PlayClip(winningGameClip);

            cardsManager.gameObject.SetActive(false);
            UIManager.gameObject.SetActive(false);

            gameFinishedCallback?.Invoke($"You've done it with {score} score and {errors} errors");
        }

        protected override void CardsUnmatched()
        {
            streak = 0;
            errors++;

            UIManager.SetErrors(errors);
            OnUpdate?.Invoke();
        }

        protected override void CardsMatched(CardInfo card1, CardInfo card2)
        {
            score += baseScorePerMatch + baseStreakBonus * streak;
            streak++;

            UIManager.SetScore(score);
            cardInfos.Remove(card1);
            cardInfos.Remove(card2);

            OnUpdate?.Invoke();
        }

        protected override BaseInGameUIManager GetUIManagerPrefab()
        {
            return UIManagerPrefab;
        }

        protected override CardsManager GetCardsManagerPrefab()
        {
            return cardsManagerPrefab;
        }
    }
}
