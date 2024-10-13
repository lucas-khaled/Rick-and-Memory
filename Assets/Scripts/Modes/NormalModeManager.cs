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
        
        public override string GetModeName()
        {
            return "Normal Mode";
        }

        protected override BaseInGameUIManager GetUIManagerPrefab()
        {
            return UIManagerPrefab;
        }

        protected override CardsManager GetCardsManagerPrefab()
        {
            return cardsManagerPrefab;
        }

        protected override ModeInfo GetModeInfo()
        {
            return new ModeInfo
            {
                score = score,
                errors = errors,
                streak = streak
            };
        }
    }
}
