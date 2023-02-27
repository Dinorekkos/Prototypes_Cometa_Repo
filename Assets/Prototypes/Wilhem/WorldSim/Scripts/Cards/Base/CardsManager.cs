using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.WorldSim
{
    public class CardsManager : MonoBehaviour
    {
        public List<Card> unlockedCards;
        public List<Card> handCards;
        public Dictionary<string, int> collectedCards;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            //initialize collected cards and hand cards
            collectedCards = new Dictionary<string, int>();
            handCards = new List<Card>();

            //Initialize unlocked cards with The Genesis card
            unlockedCards = new List<Card>();
            unlockedCards.Add(CardFactory.CreateTheGenesisCard());


        }

        public void UseCard(Card card)
        {

        }

        public Card DrawCard()
        {
            //get and create drawed card
            int drawIndex = Random.Range(0, unlockedCards.Count);
            Card drawedCard = unlockedCards[drawIndex];

            //add card to collected count
            if (collectedCards.ContainsKey(drawedCard.id))
            {
                collectedCards[drawedCard.id]++;
            }
            else
            {
                collectedCards.Add(drawedCard.id, 1);
            }

            //remove unlocked card if max draw count is reached
            if (collectedCards[drawedCard.id] >= drawedCard.maxDrawCount)
                unlockedCards.RemoveAt(drawIndex);

            return drawedCard;
        }

    }
}