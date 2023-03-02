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
                CreateTheGiftOfLifeCard(),

                //Investigation
                CreateDiscoveryAxeCard()
            };

            return card;
        }

        public static TheGiftOfLife CreateTheGiftOfLifeCard()
        {
            TheGiftOfLife card = new TheGiftOfLife();

            card.title = "The Gift of Life";
            card.type = Card.CardType.Faith;

            return card;
        }

        public static BlessingWithBerries CreateBlessingWithBerriesCard()
        {
            BlessingWithBerries card = new BlessingWithBerries();

            card.title = "Blessing With Berries";
            card.type = Card.CardType.Faith;

            card.cardsToUnlock = new List<Card>()
            {
                CreateTheArtOfSowingCard()
            };

            return card;
        }

        public static BlessingWithTrees CreateBlessingWithTreesCard()
        {
            BlessingWithTrees card = new BlessingWithTrees();

            card.title = "Blessing With Trees";
            card.type = Card.CardType.Faith;

            card.cardsToUnlock = new List<Card>()
            {
                CreateTheArboristWayCard()
            };

            return card;
        }
        #endregion

        #region InvestigationCards
        public static DiscoveryAxe CreateDiscoveryAxeCard()
        {
            DiscoveryAxe card = new DiscoveryAxe();

            card.title = "Discovery Axe";
            card.type = Card.CardType.Investigation;

            card.cardsToUnlock = new List<Card>()
            {
                CreateDiscoveryFarmCard(),
                CreateDiscoveryHouseCard()
            };

            return card;
        }

        public static TheArtOfSowing CreateTheArtOfSowingCard()
        {
            TheArtOfSowing card = new TheArtOfSowing();

            card.title = "The Art of Sowing";
            card.type = Card.CardType.Investigation;


            return card;
        }

        public static TheArboristWay CreateTheArboristWayCard()
        {
            TheArboristWay card = new TheArboristWay();

            card.title = "The Arborist Way";
            card.type = Card.CardType.Investigation;

            return card;
        }

        public static DiscoveryFarm CreateDiscoveryFarmCard()
        {
            DiscoveryFarm card = new DiscoveryFarm();

            card.title = "Discovery Farm";
            card.type = Card.CardType.Investigation;

            return card;
        }

        public static DiscoveryHouse CreateDiscoveryHouseCard()
        {
            DiscoveryHouse card = new DiscoveryHouse();

            card.title = "Discovery House";
            card.type = Card.CardType.Investigation;

            return card;
        }
        #endregion
    }
}