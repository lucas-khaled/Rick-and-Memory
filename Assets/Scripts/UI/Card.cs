using DG.Tweening;
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
        private bool isAnimating;

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
            if (isSelected || isAnimating) return;
            isSelected = true;
            isAnimating = true;

            Sequence showSequence = DOTween.Sequence();

            showSequence.OnComplete(FinishedShowing);
            showSequence.Append(transform.DOLocalRotate(Vector3.up * 90, 0.2f).OnComplete(SetObjectsBySelectedState))
                .Join(transform.DOScale(Vector3.one * 1.5f, 0.2f))
                .Append(transform.DOLocalRotate(Vector3.zero, 0.2f))
                .Join(transform.DOScale(Vector3.one, 0.2f))
                .AppendInterval(1f);
        }

        private void SetObjectsBySelectedState() 
        {
            shownObject.SetActive(isSelected);
            hiddenObject.SetActive(!isSelected);
        }

        private void FinishedShowing() 
        {
            isAnimating = false;
            onShow?.Invoke(this);
        }

        public void Hide() 
        {
            if (!isSelected) return;

            isSelected = false;
            isAnimating = true;

            Sequence hideSequence = DOTween.Sequence();

            hideSequence.OnComplete(() => isAnimating = false);
            hideSequence.Append(transform.DOLocalRotate(Vector3.up * 90, 0.2f).OnComplete(SetObjectsBySelectedState))
                .Append(transform.DOLocalRotate(Vector3.zero, 0.2f));
        }


    }
}
