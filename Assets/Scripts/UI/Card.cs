using RickAndMemory.Data;
using RickAndMemory.Utility;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RickAndMemory.UI
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private GameObject shownObject;
        [SerializeField] private GameObject hiddenObject;
        [SerializeField] private Image thumb;
        [SerializeField] private TMP_Text itemName;

        public Action<CardInfo> onShow;

        private CardInfo cardInfo;
        private bool isSelected;

        public void SetInfo(CardInfo info) 
        {
            cardInfo = info;

            itemName.text = cardInfo.name;
            StartCoroutine(LoadThumb());
        }

        private IEnumerator LoadThumb() 
        {
            yield return SpriteLoader.LoadSprite(cardInfo.cardURL, OnThumbLoaded);
        }

        private void OnThumbLoaded(Sprite sprite)
        {
            thumb.sprite = sprite;
            thumb.preserveAspect = true;
            cardInfo.imageSprite = sprite;
        }

        public void Show() 
        {
            if (isSelected) return;

            shownObject.SetActive(true);
            hiddenObject.SetActive(false);
            isSelected = true;

            onShow?.Invoke(cardInfo);
        }

        public void Hide() 
        {
            if (!isSelected) return;

            shownObject.SetActive(false);
            hiddenObject.SetActive(true);
            isSelected = false;
        }
    }
}
