using RickAndMemory.Audio;
using RickAndMemory.Data;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Core
{
    public class CardsManager : MonoBehaviour
    {
        [SerializeField] private LayoutManager layoutManager;
        [SerializeField] private RectTransform cardsContent;
        [SerializeField] private Card cardPrefab;

        [Header("Audios")]
        [SerializeField] private AudioClip matchingCardAudio;
        [SerializeField] private AudioClip unmatchingCardAudio;


        public Action<CardInfo, CardInfo> onCardsMatched;
        public Action OnCardsUnmatched;
        public Action OnCardsFinished;

        private List<Card> cards = new List<Card>();
        private Queue<Card> selectedCards = new Queue<Card>(); 
        private List<int> usedPositions;
        private Layout layout;
        private bool isChecking;

        public void SetLayout(Layout layout) 
        {
            this.layout = layout;
            layoutManager.SetLayout(layout, cardsContent.rect, cardPrefab.GetComponent<RectTransform>().rect);
        }

        public void InstantiateCards(List<CardInfo> cardInfo) 
        {
            ClearCards();
            InitializeUsedPositions();

            for (int i = 0; i < cardInfo.Count; i++) 
            {
                CardInfo info = cardInfo[i];

                int positionIndex = GetCardPositonIndex(info);
                Card card = InstantiateCard(info, layoutManager.GetPosition(positionIndex));
                info.positionIndex = positionIndex;

                cards.Add(card);
            }
        }

        private void InitializeUsedPositions()
        {
            usedPositions = new List<int>();
            for(int i = 0; i< layout.TotalAmount; i++) 
            {
                usedPositions.Add(i);
            }
        }

        private Card InstantiateCard(CardInfo info, Vector2 postion) 
        {
            Card card = Instantiate(cardPrefab, cardsContent);
            card.SetInfo(info);
            card.onShow += OnCardSelected;

            var cardRectTransform = card.GetComponent<RectTransform>();
            cardRectTransform.anchorMax = Vector2.zero;
            cardRectTransform.anchorMin = Vector2.zero;
            cardRectTransform.anchoredPosition = postion;

            cardRectTransform.sizeDelta = layoutManager.GetCardSize();

            return card;
        }

        private int GetCardPositonIndex(CardInfo card) 
        {
            if (card.positionIndex != -1)
            {
                usedPositions.Remove(card.positionIndex);
                return card.positionIndex;
            }
            
            int randomPositionIndex = UnityEngine.Random.Range(0, usedPositions.Count);
            int positionIndex = usedPositions[randomPositionIndex];
            usedPositions.RemoveAt(randomPositionIndex);

            return positionIndex;
        }

        private void ClearCards() 
        {
            foreach(var card in cards) 
            {
                Destroy(card.gameObject);
            }

            cards.Clear();
        }

        private void OnCardSelected(Card card) 
        {
            selectedCards.Enqueue(card);
            if (selectedCards.Count >= 2 && !isChecking) 
            {
                StartCoroutine(CheckSelection());
                return;
            }
        }

        private IEnumerator CheckSelection() 
        {
            isChecking = true;
            while (selectedCards.Count >= 2)
            {
                var firstSelectedCard = selectedCards.Dequeue();
                var lastSelectedCard = selectedCards.Dequeue();

                if (lastSelectedCard.CardInfo.id == firstSelectedCard.CardInfo.id)
                {
                    CardsMatched(firstSelectedCard, lastSelectedCard);
                }
                else
                    CardsUnmatched(firstSelectedCard, lastSelectedCard);

                yield return null;
            }

            isChecking = false;
        }

        private void CardsMatched(Card firstSelectedCard, Card lastSelectedCard)
        {
            AudioManager.Instance.PlayClip(matchingCardAudio);

            cards.Remove(lastSelectedCard);
            Destroy(lastSelectedCard.gameObject);

            cards.Remove(firstSelectedCard);
            Destroy(firstSelectedCard.gameObject);

            onCardsMatched?.Invoke(firstSelectedCard.CardInfo, lastSelectedCard.CardInfo);

            if (cards.Count == 0)
            {
                isChecking = false;
                OnCardsFinished?.Invoke();
            }
        }

        private void CardsUnmatched(Card firstSelectedCard, Card lastSelectedCard) 
        {
            AudioManager.Instance.PlayClip(unmatchingCardAudio);

            lastSelectedCard.Hide();
            firstSelectedCard.Hide();

            OnCardsUnmatched?.Invoke();
        }
    }
}
