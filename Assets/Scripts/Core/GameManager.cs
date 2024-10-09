using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.Providers;
using RickAndMemory.UI;
using UnityEngine;

namespace RickAndMemory.Core
{
    public class GameManager : MonoBehaviour
    {
        private ICardInfoProvider provider;
        private IMainUIManager uiManager;

        private void Awake()
        {
            uiManager.Initialize(StartGame);
        }

        public async void StartGame(Layout layout, IModeManager modeManager) 
        {
            uiManager.SetLoading(true);

            CardInfo[] cardInfos = await provider.GetCards(layout.Amount);

            modeManager.SetGameEndedCallback(uiManager.SetEndGame);
            modeManager.StartGame(layout, cardInfos);

            uiManager.SetLoading(false);
        }
    }
}
