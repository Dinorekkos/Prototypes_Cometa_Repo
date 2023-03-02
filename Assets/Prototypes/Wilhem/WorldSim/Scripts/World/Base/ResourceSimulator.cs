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

        public bool CanDie()
        {
            return (this.hunger >= 100);
        }
    }

    public class ResourceSimulator : MonoBehaviour
    {
        [Header("Starting Values")]
        [SerializeField] private int startingPopulation = 2;
        [SerializeField] private int startingMaxPopulation = 0;

        [SerializeField] private int startingFood = 0;
        [SerializeField] private int startingWood = 0;

        [SerializeField] private int startingBerryTrees = 0;
        [SerializeField] private int startingTrees = 0;

        [SerializeField] private float startingPersonFaithPerSecond = 0.2f;
        [SerializeField] private float startingPersonInvestigationPerSecond = 0.1f;

        [Header("Population")]
        [ReadOnly] [SerializeField] public int maxPopulation;
        [SerializeField] private float birthInterval = 30f;
        [SerializeField] private float birthsByPersonPerInterval = 0.5f;

        [Header("Food Consumption")]
        [SerializeField] private float foodConsumptionInterval = 5.0f;
        [SerializeField] private int hungerIncreasePerInterval = 2;

        [Header("Bery Harvesting")]
        [SerializeField] private float berryHarvestInterval = 10f;
        [SerializeField, Min(1)] private int minBerriesPerTree = 3;
        [SerializeField, Min(1)] private int maxBerriesPerTree = 4;

        [Header("Tree Felling")]
        [SerializeField] private float treeCuttingInterval = 10f;
        [SerializeField, Min(1)] private int minWoodPerTree = 3;
        [SerializeField, Min(1)] private int maxWoodPerTree = 4;

        [Header("Farm Building")]
        [SerializeField] private float buildFarmInterval = 20f;

        [Header("House Building")]
        [SerializeField] private float buildHouseInterval = 20f;
        [SerializeField] private int populationPerHouse = 4;

        [Header("Farm Production")]
        [SerializeField] private float farmProductionInterval = 15f;
        [SerializeField, Min(1)] private int minFoodPerFarm = 3;
        [SerializeField, Min(1)] private int maxFoodPerFarm = 5;

        [Header("The Art of Sowing")]
        [SerializeField] private float plantBerryInterval = 20f;

        [Header("The Arborist Way")]
        [SerializeField] private float plantTreeInterval = 30f;

        [Header("Investigations")]
        [ReadOnly] [SerializeField] public bool discoveredAxe = false;
        [ReadOnly] [SerializeField] public bool discoveredFarm = false;
        [ReadOnly] [SerializeField] public bool discoveredHouse = false;
        [ReadOnly] [SerializeField] public bool unlockedTheArtOfSowing = false;
        [ReadOnly] [SerializeField] public bool unlockedTheArboristWay = false;

        [Header("Debug Info")]
        [ReadOnly] [SerializeField] public List<Person> people;

        [ReadOnly] [SerializeField] public int food;
        [ReadOnly] [SerializeField] public int wood;
        [ReadOnly] [SerializeField] public int berryTrees;
        [ReadOnly] [SerializeField] public int trees;
        [ReadOnly] [SerializeField] public int farms;
        [ReadOnly] [SerializeField] public int houses;

        [ReadOnly] [SerializeField] public float faithPoints;
        [ReadOnly] [SerializeField] public float investigationPoints;

        [ReadOnly] [SerializeField] public float personFaithPerSecond;
        [ReadOnly] [SerializeField] public float personInvestigationPerSecond;

        [ReadOnly] [SerializeField] private float timeSinceLastBirth;
        [ReadOnly] [SerializeField] private float timeSinceLastFoodConsumption;
        [ReadOnly] [SerializeField] private float timeSinceLastBerryHarvest;
        [ReadOnly] [SerializeField] private float timeSinceLastTreeCutting;
        [ReadOnly] [SerializeField] private float timeSinceLastFarmBuilt;
        [ReadOnly] [SerializeField] private float timeSinceLastFarmProduction;
        [ReadOnly] [SerializeField] private float timeSinceLastHouseBuilt;
        [ReadOnly] [SerializeField] private float timeSinceLastBerryPlanted;
        [ReadOnly] [SerializeField] private float timeSinceLastTreePlanted;

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
            maxPopulation = startingMaxPopulation;

            food = startingFood;
            wood = startingWood;

            berryTrees = startingBerryTrees;
            trees = startingBerryTrees;

            farms = 0;
            houses = 0;

            personFaithPerSecond = startingPersonFaithPerSecond;
            personInvestigationPerSecond = startingPersonInvestigationPerSecond;

            timeSinceLastFoodConsumption = 0.0f;
            timeSinceLastBerryHarvest = 0.0f;
        }

        private void Update()
        {
            if (people.Count > 0)
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
                    }
                }
            }
            else
            {
                timeSinceLastFoodConsumption = 0f;
            }

            //remove people that can die
            people.RemoveAll(person => person.CanDie());

            if (CanPeopleBeBorn())
            {
                timeSinceLastBirth += Time.deltaTime;

                if (timeSinceLastBirth >= birthInterval)
                {
                    timeSinceLastBirth -= birthInterval;

                    int numBirths = (int)(people.Count * birthsByPersonPerInterval);

                    for (int i=0;i<numBirths;i++)
                    {
                        BirthPerson();
                    }
                }
            } else
            {
                timeSinceLastBirth = 0f;
            }

            if (CanHarvestBerries())
            {
                // Update time since last berry harvest and harvest berries
                timeSinceLastBerryHarvest += Time.deltaTime;

                if (timeSinceLastBerryHarvest >= berryHarvestInterval)
                {
                    timeSinceLastBerryHarvest -= berryHarvestInterval;

                    HarvestBerries();
                }
            } else
            {
                timeSinceLastBerryHarvest = 0f;
            }

            if (discoveredAxe && CanCutTrees())
            {
                timeSinceLastTreeCutting += Time.deltaTime;

                if (timeSinceLastTreeCutting >= treeCuttingInterval)
                {
                    timeSinceLastTreeCutting -= treeCuttingInterval;
            
                    CutTrees();
                }
            } else
            {
                timeSinceLastTreeCutting = 0f;
            }

            if (CanFarmProduceFood())
            {
                timeSinceLastFarmProduction += Time.deltaTime;

                if (timeSinceLastFarmProduction >= farmProductionInterval)
                {
                    timeSinceLastFarmProduction -= farmProductionInterval;

                    ProduceFoodPerFarm();
                }
            }

            if (discoveredFarm && CanBuildFarm())
            {
                timeSinceLastFarmBuilt += Time.deltaTime;

                if (timeSinceLastFarmBuilt >= buildFarmInterval)
                {
                    timeSinceLastFarmBuilt -= buildFarmInterval;

                    BuildFarm();
                }
            } else
            {
                timeSinceLastFarmBuilt = 0f;
            }

            if (discoveredHouse && CanBuildHouse())
            {
                timeSinceLastHouseBuilt += Time.deltaTime;

                if (timeSinceLastHouseBuilt >= buildHouseInterval)
                {
                    timeSinceLastHouseBuilt -= buildHouseInterval;

                    BuildHouse();
                }
            } else
            {
                timeSinceLastHouseBuilt = 0f;
            }

            if (unlockedTheArtOfSowing)
            {
                timeSinceLastBerryPlanted += Time.deltaTime;

                if (timeSinceLastBerryPlanted >= plantBerryInterval)
                {
                    timeSinceLastBerryPlanted -= plantBerryInterval;

                    berryTrees++;
                }
            }

            if (unlockedTheArboristWay)
            {
                timeSinceLastTreePlanted += Time.deltaTime;

                if (timeSinceLastTreePlanted >= plantTreeInterval)
                {
                    timeSinceLastTreePlanted -= plantTreeInterval;

                    trees++;
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

        //Give birth a person with needs and hapiness
        public void BirthPerson()
        {
            people.Add(new Person(0, 50));
        }

        private bool CanHarvestBerries()
        {
            return berryTrees > 0 && people.Count > 0;
        }

        private void HarvestBerries()
        {
            int harvestedBerries = Random.Range(minBerriesPerTree, maxBerriesPerTree + 1);
            berryTrees--;
            food += harvestedBerries;
        }

        private bool CanFarmProduceFood()
        {
            return farms > 0 && people.Count > 0;
        }

        private void ProduceFoodPerFarm()
        {
            int foodPerFarm = Random.Range(minFoodPerFarm, maxFoodPerFarm + 1);
            food += farms * foodPerFarm;
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

        private bool CanBuildFarm()
        {
            return wood >= 15 && food < people.Count * 2;
        }

        private void BuildFarm()
        {
            farms++;
            wood -= 15;
        }

        private bool CanBuildHouse()
        {
            return wood > 15 && people.Count > maxPopulation;
        }

        private void BuildHouse()
        {
            houses++;
            maxPopulation += populationPerHouse;
            wood -= 15;
        }

        private bool CanPeopleBeBorn()
        {
            //Cant be born if we have just one or 0 people
            if (people.Count < 2)
                return false;
            //Cant be born if we reached max population
            if (people.Count >= maxPopulation + 1)
                return false;

            //Cant be born if we have not the necessary food
            if (food < people.Count * 1.5f)
                return false;

            return true;
        }

        private void IncreaseFaith()
        {
            faithPoints += totalFaithPerSecond * Time.deltaTime;
        }

        private void IncreaseInvestigation()
        {
            investigationPoints += totalInvestigationPerSecond * Time.deltaTime;
        }

        public float totalFaithPerSecond
        {
            get { return personFaithPerSecond * people.Count; }
        }

        public float totalInvestigationPerSecond
        {
            get { return personInvestigationPerSecond * people.Count; }
        }
    }

}
        
