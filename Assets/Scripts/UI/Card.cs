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

        public Action<Card> onShow;

        public CardInfo CardInfo { get; private set; }

        private bool isSelected;

        public void SetInfo(CardInfo info) 
        {
            CardInfo = info;

            itemName.text = CardInfo.name;
            StartCoroutine(LoadThumb());
        }

        private IEnumerator LoadThumb() 
        {
            yield return SpriteLoader.LoadSprite(CardInfo.cardURL, OnThumbLoaded);
        }

        private void OnThumbLoaded(Sprite sprite)
        {
            thumb.sprite = sprite;
            thumb.preserveAspect = true;
            CardInfo.imageSprite = sprite;
        }

        public void Show() 
        {
            if (isSelected) return;

            shownObject.SetActive(true);
            hiddenObject.SetActive(false);
            isSelected = true;

            onShow?.Invoke(this);
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
