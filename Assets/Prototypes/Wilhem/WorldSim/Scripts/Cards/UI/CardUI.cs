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
        public Color faithCardColor = Color.white;
        public Color investigationCardColor = Color.white;
       
        [ReadOnly] public int handIndex;
        [ReadOnly] public Card card;

        public void SetCard(int handIndex, Card card)
        {
            this.handIndex = handIndex;
            this.card = card;

            this.titleText.text = card.title;

            if (card.type == Card.CardType.Faith)
                this.backgroundImage.color = faithCardColor;

            if (card.type == Card.CardType.Investigation)
                this.backgroundImage.color = investigationCardColor;
        }

        public void UseCard()
        {
            CardsManager.Instance.UseCard(handIndex, card);
        }
    }
}