using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.WorldSim
{
    public class CardsManager : MonoBehaviour
    {
        public List<string> unlockedCards;
        public List<string> hand;

        public Dictionary<string, int> collectedCards;
        public Dictionary<string, Card> cardsDictionary;

        public delegate void CardsManagerDelegate();
        public event CardsManagerDelegate OnUpdatedHand;

        public static CardsManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            //initialize collected cards and hand cards
            collectedCards = new Dictionary<string, int>();
            hand = new List<string>();
            cardsDictionary = new Dictionary<string, Card>();

            //Initialize cards dictionary
            Card firstCard = CardFactory.CreateTheGenesisCard();
            cardsDictionary.Add(firstCard.id, firstCard);

            //Initialize unlocked cards with The Genesis card
            unlockedCards = new List<string>();
            unlockedCards.Add(firstCard.id);

            //Draw the first card
            DrawCard();
        }

        public void UseCard(int handIndex, Card card)
        {
            //remove card from hand
            hand.RemoveAt(handIndex);

            //If doesnt exist cards to unlock, dont unlock anything
            if (card.cardsToUnlock == null ||
                card.cardsToUnlock.Count <= 0)
            {
                OnUpdatedHand?.Invoke();
                return;
            }

            //unlock cards
            foreach (Card tmpUnlocked in card.cardsToUnlock)
            {
                if (cardsDictionary.ContainsKey(tmpUnlocked.id) == false)
                {
                    unlockedCards.Add(tmpUnlocked.id);
                    cardsDictionary.Add(tmpUnlocked.id, tmpUnlocked);
                }
            }

            OnUpdatedHand?.Invoke();
        }

        public Card DrawCard()
        {
            //get and create drawed card
            int drawIndex = Random.Range(0, unlockedCards.Count);
            string drawId = unlockedCards[drawIndex];

            Card drawedCard = cardsDictionary[drawId];

            //add card to collected count
            if (collectedCards.ContainsKey(drawId))
            {
                collectedCards[drawId]++;
            }
            else
            {
                collectedCards.Add(drawId, 1);
            }

            //add drawed card to current hand
            hand.Add(drawId);
            
            //remove unlocked card if max draw count is reached
            if (collectedCards[drawId] >= drawedCard.maxDrawCount)
                unlockedCards.RemoveAt(drawIndex);

            OnUpdatedHand?.Invoke();

            return drawedCard;
        }

    }
}