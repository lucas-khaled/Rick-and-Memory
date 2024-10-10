using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.Providers;
using RickAndMemory.UI;
using System;
using UnityEngine;

namespace RickAndMemory.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private BaseMenuManager uiManager;
        [SerializeField]
        private GameSetup gameSetup;

        private void Start()
        {
            uiManager.Initialize(StartGame, gameSetup.avaiableModes);
        }

        public async void StartGame(Layout layout, IModeManager modeManager) 
        {
            uiManager.SetLoading(true);

            CardInfo[] cardInfos = await gameSetup.provider.GetCards(layout.Amount);

            modeManager.SetGameEndedCallback(uiManager.SetEndGame);
            modeManager.StartGame(layout, cardInfos);

            uiManager.SetLoading(false);
        }

        #region Test
        bool isLoading = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                uiManager.SetEndGame("The game is over in a test!");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                isLoading = !isLoading;
                uiManager.SetLoading(isLoading);
            }
        }
        #endregion
    }
}
