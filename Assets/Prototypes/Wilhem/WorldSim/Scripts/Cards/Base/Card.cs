using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.WorldSim
{
    public abstract class Card
    {
        public enum CardType
        {
            Faith,
            Investigation
        }

        public CardType type;
        public List<Card> cardsToUnlock;

        public abstract string id { get; }
        public abstract int maxDrawCount { get; }

        public abstract void UseCard();
    }

    #region FaithCards
    public class TheGenesis: Card
    {
        public override string id => "thegenesis";

        public override int maxDrawCount => 1;

        public override void UseCard()
        {
            ResourceSimulator.Instance.BirthPerson();
        }
    }
    #endregion

    #region InvestigationCards

    #endregion
}