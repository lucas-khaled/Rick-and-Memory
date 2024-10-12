using RickAndMemory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RickAndMemory
{
    [RequireComponent(typeof(Canvas))]
    public class NormalInGameUIManager : MonoBehaviour, IInGameUIManager
    {
        [SerializeField] private TMP_Text errorsText;
        [SerializeField] private TMP_Text scoreText;

        public void SetErrors(float errors)
        {
            errorsText.text = errors.ToString();
        }

        public void SetScore(float score)
        {
            scoreText.text = score.ToString();
        }
    }
}
