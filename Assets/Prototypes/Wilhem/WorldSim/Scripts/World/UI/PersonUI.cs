using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.WorldSim
{
    public class PersonUI : MonoBehaviour
    {
        public FillBarController fillBar;

        public Person person;

        public void SetData(Person person)
        {
            this.person = person;

            float fillLevel = (float)(100 - person.hunger) / 100f;
            fillBar.SetFillLevel(fillLevel);
        }
    }
}