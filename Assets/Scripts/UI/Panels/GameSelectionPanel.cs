using RickAndMemory.Data;
using RickAndMemory.Modes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RickAndMemory.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameSelectionPanel : MonoBehaviour, IPanel
    {
        [Header("Layout")]
        [SerializeField] private TMP_Dropdown layoutWidthDropdown;
        [SerializeField] private TMP_Dropdown layoutHeightDropdown;

        [Header("Modes")]
        [SerializeField] private Transform modesButtonContent;
        [SerializeField] private ToggleGroup modesToggleGroup;
        [SerializeField] private ModeToggle modeButtonPrefab;

        [Header("Game Objects")]
        [SerializeField] private GameObject startGameButton;
        [SerializeField] private GameObject notValidLayoutLabel;

        private CanvasGroup canvasGroup;
        private Layout actualSelectedLayout;
        private IModeManager selectedGameMode;

        private Action<Layout, IModeManager> startGameCallback;

        public void Hide()
        {
            canvasGroup.alpha = 0;
        }

        public void Show()
        {
            canvasGroup.alpha = 1;
        }

        public void SetModes(IModeManager[] modes) 
        {
            selectedGameMode = modes[0];
            
            foreach(var mode in modes)
            {
                var modeButton = Instantiate(modeButtonPrefab, modesButtonContent);
                modeButton.modeSelected += ModeSelected;
                modeButton.SetMode(mode, modesToggleGroup);
            }
        }

        private void ModeSelected(IModeManager mode)
        {
            selectedGameMode = mode;
        }

        public void SetStartGameCallback(Action<Layout, IModeManager> callback) 
        {
            startGameCallback = callback;
        }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            SetDropdownsCallbacks();
        }
        
        private void SetDropdownsCallbacks()
        {
            actualSelectedLayout = new Layout(layoutWidthDropdown.value+1, layoutHeightDropdown.value+1);

            layoutWidthDropdown.onValueChanged.AddListener(OnWidthChanged);
            layoutHeightDropdown.onValueChanged.AddListener(OnHeightChanged);
        }

        private void OnWidthChanged(int value) 
        {
            actualSelectedLayout.width = value + 1;
            CheckLayoutIsValid();
        }

        private void OnHeightChanged(int value) 
        {
            actualSelectedLayout.height = value + 1;
            CheckLayoutIsValid();
        }

        private void CheckLayoutIsValid()
        {
            bool isValid = actualSelectedLayout.TotalAmount % 2 == 0;
            startGameButton.SetActive(isValid);
            notValidLayoutLabel.SetActive(!isValid);
        }

        public void StartGame()
        {
            startGameCallback?.Invoke(actualSelectedLayout, selectedGameMode);
        }
    }
}
