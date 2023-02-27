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
            card.type = Card.CardType.Faith;

            return card;
        }
        #endregion

        #region InvestigationCards

        #endregion
    }
}