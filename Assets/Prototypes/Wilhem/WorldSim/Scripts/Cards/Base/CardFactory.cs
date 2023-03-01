using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.WorldSim
{
    public class CardFactory
    {
        #region FaithCards
        public static TheGenesis CreateTheGenesisCard()
        {
            TheGenesis card = new TheGenesis();

            card.title = "The Genesis";
            card.type = Card.CardType.Faith;

            card.cardsToUnlock = new List<Card>()
            {
                //Faith
                CreateBlessingWithBerriesCard(),
                CreateBlessingWithTreesCard(),

                //Investigation
                CreateDiscoveryAxeCard()
            };

            return card;
        }

        public static BlessingWithBerries CreateBlessingWithBerriesCard()
        {
            BlessingWithBerries card = new BlessingWithBerries();

            card.title = "Blessing With Berries";
            card.type = Card.CardType.Faith;

            return card;
        }

        public static BlessingWithTrees CreateBlessingWithTreesCard()
        {
            BlessingWithTrees card = new BlessingWithTrees();

            card.title = "Blessing With Trees";
            card.type = Card.CardType.Faith;

            return card;
        }
        #endregion

        #region InvestigationCards
        public static DiscoveryAxe CreateDiscoveryAxeCard()
        {
            DiscoveryAxe card = new DiscoveryAxe();

            card.title = "Discovery Axe";
            card.type = Card.CardType.Investigation;

            return card;
        }
        #endregion
    }
}