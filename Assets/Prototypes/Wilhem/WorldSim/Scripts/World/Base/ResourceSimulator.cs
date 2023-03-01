using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace CometaPrototypes.WorldSim
{
    [System.Serializable]
    public class Person
    {
        public int hunger;

        public Person(int hunger, int happiness)
        {
            this.hunger = hunger;
        }
    }

    public class ResourceSimulator : MonoBehaviour
    {
        [Header("Starting Values")]
        [SerializeField] private int startingPopulation = 2;
        [SerializeField] private int startingFood = 0;
        [SerializeField] private int startingWood = 0;

        [SerializeField] private int startingBerryTrees = 0;
        [SerializeField] private int startingTrees = 0;

        [SerializeField] private float startingPersonFaithPerSecond = 0.2f;
        [SerializeField] private float startingPersonInvestigationPerSecond = 0.1f;

        [Header("Food Consumption")]
        [SerializeField] private float foodConsumptionInterval = 5.0f;
        [SerializeField] private int hungerIncreasePerInterval = 2;

        [Header("Bery Harvesting")]
        [SerializeField] private float berryHarvestInterval = 10f;
        [SerializeField, Min(1)] private int minBerriesPerTree = 3;
        [SerializeField, Min(1)] private int maxBerriesPerTree = 4;

        [Header("Tree Felling")]
        [SerializeField] private float treeCuttingInterval = 15f;
        [SerializeField, Min(1)] private int minWoodPerTree = 3;
        [SerializeField, Min(1)] private int maxWoodPerTree = 4;

        [Header("Investigations")]
        [ReadOnly] [SerializeField] public bool discoveredAxe = false;

        [Header("Debug Info")]
        [ReadOnly] [SerializeField] public List<Person> people;

        [ReadOnly] [SerializeField] public int food;
        [ReadOnly] [SerializeField] public int wood;
        [ReadOnly] [SerializeField] public int berryTrees;
        [ReadOnly] [SerializeField] public int trees;

        [ReadOnly] [SerializeField] public float faithPoints;
        [ReadOnly] [SerializeField] public float investigationPoints;

        [ReadOnly] [SerializeField] public float personFaithPerSecond;
        [ReadOnly] [SerializeField] public float personInvestigationPerSecond;

        [ReadOnly] [SerializeField] private float timeSinceLastFoodConsumption;
        [ReadOnly] [SerializeField] private float timeSinceLastBerryHarvest;
        [ReadOnly] [SerializeField] private float timeSinceLastTreeCutting;

        public static ResourceSimulator Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            people = new List<Person>();

            for (int i=0; i < startingPopulation;i++)
            {
                BirthPerson();
            }

            food = startingFood;
            wood = startingWood;

            berryTrees = startingBerryTrees;
            trees = startingBerryTrees;

            personFaithPerSecond = startingPersonFaithPerSecond;
            personInvestigationPerSecond = startingPersonInvestigationPerSecond;

            timeSinceLastFoodConsumption = 0.0f;
            timeSinceLastBerryHarvest = 0.0f;
        }

        private void Update()
        {
            // Update time since last food consumption and check if population can eat
            timeSinceLastFoodConsumption += Time.deltaTime;
            if (timeSinceLastFoodConsumption >= foodConsumptionInterval)
            {
                timeSinceLastFoodConsumption -= foodConsumptionInterval;
                for (int i = 0; i < people.Count; i++)
                {
                    IncreaseHunger(i);

                    if (CanEat(i))
                    {
                        Eat(i);
                    }
                    else
                    {
                        if (CanDie(i))
                        {
                            Kill(i);
                        }
                    }
                }
            }

            // Update time since last berry harvest and check if berries can be harvested
            timeSinceLastBerryHarvest += Time.deltaTime;

            if (timeSinceLastBerryHarvest >= berryHarvestInterval)
            {
                timeSinceLastBerryHarvest -= berryHarvestInterval;
                if (CanHarvestBerries())
                {
                    HarvestBerries();
                }
            }

            if (discoveredAxe)
            {
                timeSinceLastTreeCutting += Time.deltaTime;

                if (timeSinceLastTreeCutting >= treeCuttingInterval)
                {
                    timeSinceLastTreeCutting -= treeCuttingInterval;
                    if (CanCutTrees())
                    {
                        CutTrees();
                    }
                }
            }

            IncreaseFaith();
            IncreaseInvestigation();
        }

        private void IncreaseHunger(int index)
        {
            people[index].hunger += hungerIncreasePerInterval;
            if (people[index].hunger > 100)
            {
                people[index].hunger = 100;
            }
        }

        private bool CanEat(int index)
        {
            int necessaryHunger = Random.Range(people[index].hunger, 40);
            return food > 0 && people[index].hunger >= necessaryHunger;
        }

        private void Eat(int index)
        {
            food--;
            people[index].hunger -= 20;

            if (people[index].hunger < 0)
            {
                people[index].hunger = 0;
            }
        }

        private bool CanDie(int index)
        {
            return people[index].hunger >= 100;
        }

        private void Kill(int index)
        {
            people.RemoveAt(index);
        }

        //Give birth a person with needs and hapiness
        public void BirthPerson()
        {
            people.Add(new Person(0, 50));
        }

        private bool CanHarvestBerries()
        {
            return berryTrees > 0;
        }

        private void HarvestBerries()
        {
            int harvestedBerries = Random.Range(minBerriesPerTree, maxBerriesPerTree + 1);
            berryTrees--;
            food += harvestedBerries;
        }

        private bool CanCutTrees()
        {
            return trees > 0;
        }

        private void CutTrees()
        {
            int obtainedWood = Random.Range(minWoodPerTree, maxWoodPerTree + 1);
            trees--;
            wood += obtainedWood;
        }

        private void IncreaseFaith()
        {
            faithPoints += personFaithPerSecond * Time.deltaTime * people.Count;
        }

        private void IncreaseInvestigation()
        {
            investigationPoints += personInvestigationPerSecond * Time.deltaTime * people.Count;
        }
    }

}
        
