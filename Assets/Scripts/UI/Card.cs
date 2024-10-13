using DG.Tweening;
using RickAndMemory.Audio;
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
        [SerializeField] private GameObject loadingObject;
        [SerializeField] private Image thumb;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private AudioClip flipCardClip;

        public Action<Card> onShow;

        public CardInfo CardInfo { get; private set; }

        private bool isSelected;
        private bool isAnimating;

        public void SetInfo(CardInfo info) 
        {
            SetLoadingAnimation();
            CardInfo = info;

            itemName.text = CardInfo.name;
            StartCoroutine(LoadThumb());
        }

        private void SetLoadingAnimation() 
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(loadingObject.transform.DOLocalRotate(Vector3.forward* 180, 0.25f).SetEase(Ease.InCubic))
                .Append(loadingObject.transform.DOLocalRotate(Vector3.forward * 360, 0.25f).SetEase(Ease.OutCubic))
                .SetLoops(-1);
            
        }

        private IEnumerator LoadThumb() 
        {
            yield return SpriteLoader.LoadSprite(CardInfo.cardURL, OnThumbLoaded);
        }

        private void OnThumbLoaded(Sprite sprite)
        {
            loadingObject.SetActive(false);
            thumb.color = Color.white;
            thumb.sprite = sprite;
            thumb.preserveAspect = true;
            CardInfo.imageSprite = sprite;
        }

        public void Show() 
        {
            if (isSelected || isAnimating) return;

            isSelected = true;
            isAnimating = true;

            AudioManager.Instance.PlayClip(flipCardClip, 1.4f);
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

            AudioManager.Instance.PlayClip(flipCardClip, 1.4f);
            Sequence hideSequence = DOTween.Sequence();

            hideSequence.OnComplete(() => isAnimating = false);
            hideSequence.Append(transform.DOLocalRotate(Vector3.up * 90, 0.2f).OnComplete(SetObjectsBySelectedState))
                .Append(transform.DOLocalRotate(Vector3.zero, 0.2f));
        }


    }
}
