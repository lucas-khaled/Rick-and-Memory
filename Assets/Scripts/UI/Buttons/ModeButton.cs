using RickAndMemory.Modes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RickAndMemory.UI
{
    [RequireComponent(typeof(Button))]
    public class ModeButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonText;

        private Button button;
        private IModeManager modeManager;

        public Action<IModeManager> modeSelected;

        public void SetMode(IModeManager mode) 
        {
            modeManager = mode;

            buttonText.text = mode.GetModeName();

            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick() 
        {
            modeSelected?.Invoke(modeManager);
        }
    }
}
