using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class PanelBasedMenuManager : BaseMenuManager
    {
        [SerializeField] private LoadingPanel loadingPanel;
        [SerializeField] private EndGamePanel endGamePanel;
        [SerializeField] private GameSelectionPanel gameSelectionPanel;

        private IPanel activePanel;


        public override void Initialize(Action<Layout, IModeManager> callback, IModeManager[] avaliableModes)
        {
            SetAsActivePanel(gameSelectionPanel);
            gameSelectionPanel.SetModes(avaliableModes);
            gameSelectionPanel.SetStartGameCallback(callback);
        }

        public override void HideAll()
        {
            SetAsActivePanel(null);
        }

        public override void SetEndGame(string text)
        {
            endGamePanel.SetMessage(text);
            endGamePanel.gameObject.SetActive(true);
            SetAsActivePanel(endGamePanel);
        }

        public override void SetLoading(bool active)
        {
            if (active)
                SetAsActivePanel(loadingPanel);
            else if (activePanel is LoadingPanel)
                SetAsActivePanel(null);
        }

        private void SetAsActivePanel(IPanel panel)
        {
            if (activePanel != null)
                activePanel.Hide();

            activePanel = panel;

            if (activePanel != null)
                activePanel.Show();
        }
    }
}
