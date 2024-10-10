using RickAndMemory.Modes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RickAndMemory.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ModeToggle : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonText;

        private Toggle toggle;
        private IModeManager modeManager;

        public Action<IModeManager> modeSelected;

        public void SetMode(IModeManager mode, ToggleGroup group) 
        {
            modeManager = mode;

            buttonText.text = mode.GetModeName();

            toggle = GetComponent<Toggle>();
            toggle.group = group;
            toggle.onValueChanged.AddListener(OnToggleSelected);
        }

        private void OnToggleSelected(bool selected) 
        {
            if(selected)
                modeSelected?.Invoke(modeManager);
        }
    }
}
