using RickAndMemory.Core;
using RickAndMemory.Data;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class TimeModeManager : BaseModeManager
    {
        public int timeInSecondsPerCard = 10;
        public CardsManager cardsManagerPrefab;
        public TimeInGameUIManager UIManagerPrefab;

        private TimeInGameUIManager timeUiManager;

        private const string FINISHED_TIME_STRING = "The time is over! You were not able to complete it :(";

        public override string GetModeName()
        {
            return "Time Mode";
        }

        protected override CardsManager GetCardsManagerPrefab()
        {
            return cardsManagerPrefab;
        }

        protected override BaseInGameUIManager GetUIManagerPrefab()
        {
            return UIManagerPrefab;
        }

        protected override void OnCardsFinished()
        {
            timeUiManager.StopTime();
            base.OnCardsFinished();
        }

        public override void StartGame(Layout layout, List<CardInfo> cards, int errors = 0, int score = 0)
        {
            base.StartGame(layout, cards, errors, score);

            timeUiManager = (TimeInGameUIManager)UIManager;
            var time = CalculateTime();
            timeUiManager.SetTime(time, OnFinishTime);
        }

        private void OnFinishTime()
        {
            EndGame(FINISHED_TIME_STRING);
        }

        private int CalculateTime() 
        {
            return layout.TotalAmount * timeInSecondsPerCard;
        }
    }
}
