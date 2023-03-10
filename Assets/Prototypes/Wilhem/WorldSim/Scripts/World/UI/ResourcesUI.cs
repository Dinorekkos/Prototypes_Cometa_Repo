using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManagerPooling;
using System;

namespace CometaPrototypes.WorldSim {
    public class ResourcesUI : MonoBehaviour
    {
        public Text populationText;
        public Text foodText;
        public Text woodText;

        public Text faithText;
        public Text investigationText;

        public Text berriesText;
        public Text treesText;

        public Text farmsText;
        public Text housesText;

        public SimplePooling personUIPooling;

        private float lastUpdateTime = 0f;
        private float updateDelay = 0.25f;

        private List<PersonUI> peopleSpawnedList;

        private void Start()
        {
            peopleSpawnedList = new List<PersonUI>();
        }

        private void Update()
        {
            if (Time.time < lastUpdateTime + updateDelay)
            {
                return;
            }

            UpdateUI();
            lastUpdateTime = Time.time;
        }

        private void UpdateUI()
        {
            populationText.text = "Population: " + ResourceSimulator.Instance.people.Count+" / "+ResourceSimulator.Instance.maxPopulation;
            foodText.text = "Food: " + ResourceSimulator.Instance.food;
            woodText.text = "Wood: " + ResourceSimulator.Instance.wood;

            faithText.text = "Faith: "+(int)ResourceSimulator.Instance.faithPoints +Environment.NewLine+
                "+"+ResourceSimulator.Instance.totalFaithPerSecond.ToString("F2")+" faith/s";

            investigationText.text = "Investigation: "+(int)ResourceSimulator.Instance.investigationPoints+Environment.NewLine+
                "+"+ResourceSimulator.Instance.totalInvestigationPerSecond.ToString("F2")+" inv/s";

            berriesText.text = "Berries: " + ResourceSimulator.Instance.berryTrees;
            treesText.text = "Trees: " + ResourceSimulator.Instance.trees;

            farmsText.text = "Farms: "+ResourceSimulator.Instance.farms;
            housesText.text = "Houses: " + ResourceSimulator.Instance.houses;

            List<Person> people = ResourceSimulator.Instance.people;

            for (int i = 0; i < people.Count; i++)
            {
                if (i>= peopleSpawnedList.Count)
                {
                    GameObject spawned = personUIPooling.Spawn();
                    spawned.transform.localScale = Vector3.one;
                    peopleSpawnedList.Add(spawned.GetComponent<PersonUI>());
                    spawned.SetActive(true);
                }

                peopleSpawnedList[i].SetData(people[i]);
            }

            for (int i= people.Count; i < peopleSpawnedList.Count; i++)
            {
                personUIPooling.BackToPool(peopleSpawnedList[i].gameObject);
                peopleSpawnedList.RemoveAt(peopleSpawnedList.Count - 1);
            }
        }
    }
}