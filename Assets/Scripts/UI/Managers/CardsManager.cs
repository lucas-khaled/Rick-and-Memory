using RickAndMemory.Audio;
using RickAndMemory.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RickAndMemory.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CardsManager : MonoBehaviour
    {
        [SerializeField] private LayoutManager layoutManager;
        [SerializeField] private RectTransform cardsContent;
        [SerializeField] private Card cardPrefab;

        [Header("Audios")]
        [SerializeField] private AudioClip matchingCardAudio;
        [SerializeField] private AudioClip unmatchingCardAudio;


        public Action<CardInfo> onCardsMatched;
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

        public void InstantiateCards(CardInfo[] cardInfo) 
        {
            ClearCards();
            InitializeUsedPositions();
            for (int i = 0; i < layout.Amount*.5; i++) 
            {
                int randomCardIndex = UnityEngine.Random.Range(0, cardInfo.Length);
                CardInfo info = cardInfo[randomCardIndex];

                Card card1 = InstantiateCard(info);
                Card card2 = InstantiateCard(info);

                cards.Add(card1);
                cards.Add(card2);
            }
        }

        private void InitializeUsedPositions()
        {
            usedPositions = new List<int>();
            for(int i = 0; i< layout.Amount; i++) 
            {
                usedPositions.Add(i);
            }
        }

        private Card InstantiateCard(CardInfo info) 
        {
            Card card = Instantiate(cardPrefab, cardsContent);
            card.SetInfo(info);
            card.onShow += OnCardSelected;

            int randomPositionIndex = UnityEngine.Random.Range(0, usedPositions.Count);
            Vector3 position = layoutManager.GetPosition(usedPositions[randomPositionIndex]);
            usedPositions.RemoveAt(randomPositionIndex);

            var cardRectTransform = card.GetComponent<RectTransform>();
            cardRectTransform.anchorMax = Vector2.zero;
            cardRectTransform.anchorMin = Vector2.zero;
            cardRectTransform.anchoredPosition = position;

            cardRectTransform.sizeDelta = layoutManager.GetCardSize();

            return card;
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

            onCardsMatched?.Invoke(lastSelectedCard.CardInfo);

            if (cards.Count == 0)
                OnCardsFinished?.Invoke();
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
