using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CometaPrototypes.WorldSim
{
    public class CardUI : MonoBehaviour
    {
        [ReadOnly] public int handIndex;
        [ReadOnly] public Card card;

        public void SetCard(int handIndex, Card card)
        {
            this.handIndex = handIndex;
            this.card = card;
        }

        public void UseCard()
        {
            CardsManager.Instance.UseCard(handIndex, card);
        }
    }
}