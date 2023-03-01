using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace CometaPrototypes.WorldSim
{
    public class CardUI : MonoBehaviour
    {
        public Text titleText;
        public Image backgroundImage;
        public Button cardButton;

        public Text costText;

        public Color faithCardColor = Color.white;
        public Color investigationCardColor = Color.white;
       
        [ReadOnly] public int handIndex;
        [ReadOnly] public Card card;

        private void Update()
        {
            if (card == null)
                return;

            if (card.type == Card.CardType.Faith)
            {
                if (card.cost <= (int)ResourceSimulator.Instance.faithPoints)
                {
                    cardButton.interactable = true;
                }
                else
                {
                    cardButton.interactable = false;
                }
            }

            if (card.type == Card.CardType.Investigation)
            {
                if (card.cost <= (int)ResourceSimulator.Instance.investigationPoints)
                {
                    cardButton.interactable = true;
                }
                else
                {
                    cardButton.interactable = false;
                }
            }
        }

        public void SetCard(int handIndex, Card card)
        {
            this.handIndex = handIndex;
            this.card = card;

            this.titleText.text = card.title;

            if (card.type == Card.CardType.Faith)
            {
                this.backgroundImage.color = faithCardColor;
                this.costText.text = "Faith: " + card.cost;
            }

            if (card.type == Card.CardType.Investigation)
            {
                this.backgroundImage.color = investigationCardColor;
                this.costText.text = "Investigation: "+card.cost;
            }
        }

        public void UseCard()
        {
            CardsManager.Instance.UseCard(handIndex, card);
        }
    }
}