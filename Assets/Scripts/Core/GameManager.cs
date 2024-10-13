using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.Providers;
using RickAndMemory.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

namespace RickAndMemory.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private BaseMenuManager uiManager;
        [SerializeField]
        private GameSetup gameSetup;

        private IModeManager selectedMode;

        private void Start()
        {
            Initialize();
        }

        private void Initialize() 
        {
            SaveInfo save = SavingManager.Load();

            if(save != null) 
            {
                IModeManager mode = gameSetup.avaiableModes.FirstOrDefault(x => x.GetModeName() == save.mode);
                if (mode != null)
                {
                    selectedMode = mode;
                    uiManager.HideAll();
                    InitializeModeManager(save.layout, save.cardsInfo, save.errors, save.score);
                    return;
                }
            }

            uiManager.Initialize(StartGame, gameSetup.avaiableModes);
        }

        public async void StartGame(Layout layout, IModeManager modeManager) 
        {
            selectedMode = modeManager;
            uiManager.SetLoading(true);

            List<CardInfo> cardInfos = await gameSetup.provider.GetCards(layout.DifferentCardAmount);
            InitializeModeManager(layout, cardInfos);

            uiManager.SetLoading(false);
        }

        private void InitializeModeManager(Layout layout, List<CardInfo> cardInfos, int errors = 0, int score = 0)
        {
            selectedMode.OnUpdate += OnUpdateModeManager;
            selectedMode.SetGameEndedCallback(EndGame);
            selectedMode.StartGame(layout, cardInfos, errors, score);
        }

        private void EndGame(string message) 
        {
            SavingManager.ClearSave();
            uiManager.SetEndGame(message);
        }

        private void OnUpdateModeManager() 
        {
            SavingManager.Save(selectedMode.GetSaveInfo());
        }
    }
}
