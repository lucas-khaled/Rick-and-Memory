using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RickAndMemory
{
    public class EndGamePanel : MonoBehaviour, IPanel
    {
        [SerializeField] private TMP_Text finalMessageText;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetMessage(string message)
        {
            finalMessageText.text = message;
        }

    }
}
