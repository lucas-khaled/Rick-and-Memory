using RickAndMemory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RickAndMemory
{
    [RequireComponent(typeof(Canvas))]
    public class NormalInGameUIManager : BaseInGameUIManager
    {
        [SerializeField] private TMP_Text errorsText;
        [SerializeField] private TMP_Text scoreText;

        public override void SetErrors(float errors)
        {
            errorsText.text = errors.ToString();
        }

        public override void SetScore(float score)
        {
            scoreText.text = score.ToString();
        }
    }
}
