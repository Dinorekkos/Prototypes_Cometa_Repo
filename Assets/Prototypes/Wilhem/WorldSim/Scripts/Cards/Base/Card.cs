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

        public string title;
        public CardType type;
        public List<Card> cardsToUnlock;

        public abstract string id { get; }
        public abstract int maxDrawCount { get; }
        public abstract int cost { get; }

        public abstract void UseCard();
    }

    #region FaithCards
    public class TheGenesis: Card
    {
        public override string id => "thegenesis";

        public override int maxDrawCount => 1;

        public override int cost => 0;

        public override void UseCard()
        {
            ResourceSimulator.Instance.BirthPerson();
        }
    }

    public class BlessingWithBerries : Card
    {
        public override string id => "blessing_berries";

        public override int maxDrawCount => 10;

        public override int cost => 10;

        public override void UseCard()
        {
            ResourceSimulator.Instance.berryTrees++;
        }
    }

    public class BlessingWithTrees : Card
    {
        public override string id => "blessing_trees";

        public override int maxDrawCount => 5;

        public override int cost => 10;

        public override void UseCard()
        {
            ResourceSimulator.Instance.trees++;
        }
    }
    #endregion

    #region InvestigationCards
    public class DiscoveryAxe : Card
    {
        public override string id => "discoveryAxe";

        public override int maxDrawCount => 1;

        public override int cost => 30;

        public override void UseCard()
        {
            ResourceSimulator.Instance.discoveredAxe = true;
        }
    }
    #endregion
}