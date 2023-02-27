using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.WorldSim
{
    public abstract class Card : ScriptableObject
    {
        public enum CardType
        {
            Faith,
            Investigation
        }

        public CardType type;

        public abstract string id { get; }
        public abstract int maxDrawCount { get; }
        
    }

    #region FaithCards
    public class TheGenesis: Card
    {
        public override string id => "thegenesis";

        public override int maxDrawCount => 1;

    }
    #endregion

    #region InvestigationCards

    #endregion
}