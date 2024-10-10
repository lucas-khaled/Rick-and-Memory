using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private LoadingPanel loadingPanel;
        [SerializeField] private EndGamePanel endGamePanel;

        private IPanel activePanel;

        public void Initialize(Action<Layout, IModeManager> callback)
        {
                
        }

        public void HideAll()
        {
            SetAsActivePanel(null);
        }

        public void SetEndGame(string text)
        {
            endGamePanel.SetMessage(text);
            endGamePanel.gameObject.SetActive(true);
            SetAsActivePanel(endGamePanel);
        }

        public void SetLoading(bool active)
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

            if(activePanel != null)
                activePanel.Show();
        }
    }
}
