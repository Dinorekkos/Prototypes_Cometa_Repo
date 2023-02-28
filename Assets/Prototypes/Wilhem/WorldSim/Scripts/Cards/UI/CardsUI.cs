using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CometaPrototypes.WorldSim {
    public class CardsUI : MonoBehaviour
    {
        public CardUI[] cards;
        public Button drawButton;

        public void Start()
        {
            CardsManager.Instance.OnUpdatedHand += UpdateUI;
        }

        public void UpdateUI()
        {
            List<string> handIds = CardsManager.Instance.hand;

            for (int i=0;i<cards.Length;i++)
            {
                //Enable/Disable cards depending on hand ids
                if (handIds.Count > i)
                {
                    cards[i].gameObject.SetActive(true);

                    //Set card data
                    string id = handIds[i];
                    cards[i].SetCard(i, CardsManager.Instance.cardsDictionary[id]);
                } else
                {
                    cards[i].gameObject.SetActive(false);
                }
            }

            if (CardsManager.Instance.unlockedCards.Count > 0)
            {

            }
            drawButton.interactable = (CardsManager.Instance.unlockedCards.Count > 0);
        }

        public void DrawCard()
        {
            CardsManager.Instance.DrawCard();
        }
    }
}