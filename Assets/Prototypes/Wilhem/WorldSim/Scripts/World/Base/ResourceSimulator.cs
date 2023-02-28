using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace CometaPrototypes.WorldSim
{
    [System.Serializable]
    public class Person
    {
        public int hunger;
        //public int happiness;

        public Person(int hunger, int happiness)
        {
            this.hunger = hunger;
            //this.happiness = happiness;
        }
    }

    public class ResourceSimulator : MonoBehaviour
    {
        [Header("Starting Values")]
        [SerializeField] private int startingPopulation = 2;
        [SerializeField] private int startingFood = 0;

        [Header("Food Consumption")]
        [SerializeField] private float foodConsumptionInterval = 5.0f;
        [SerializeField] private int hungerIncreasePerInterval = 2;

        //[Header("Berry Harvesting")]
        //[SerializeField] private float harvestRatio = 0.5f;
        //[SerializeField] private float foodHarvestInterval = 10.0f;
        //[SerializeField, Min(1)] private int minBerriesPerTree = 3;
        //[SerializeField, Min(1)] private int maxBerriesPerTree = 4;

        [Header("Happiness")]
        [SerializeField] private int maxHappinessIncreasePerInterval = 2;

        [Header("Debug Info")]
        [ReadOnly] [SerializeField] public List<Person> people;

        [ReadOnly] [SerializeField] public int food;
        [ReadOnly] [SerializeField] private float timeSinceLastFoodConsumption;
        //[ReadOnly] [SerializeField] private float timeSinceLastFoodHarvest;

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
            timeSinceLastFoodConsumption = 0.0f;
            //timeSinceLastFoodHarvest = 0.0f;
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
                        //people[i].happiness -= 5;
                        //if (people[i].happiness < 0)
                        //{
                        //    people[i].happiness = 0;
                        //}

                        if (CanDie(i))
                        {
                            Kill(i);
                        }
                    }
                }
            }

            //// Update time since last food harvest and check if berries can be harvested
            //timeSinceLastFoodHarvest += Time.deltaTime;

            //if (timeSinceLastFoodHarvest >= foodHarvestInterval)
            //{
            //    timeSinceLastFoodHarvest -= foodHarvestInterval;
            //    if (CanHarvestBerries())
            //    {
            //        HarvestBerries();
            //    }
            //}

            //// Update happiness
            //for (int i = 0; i < people.Count; i++)
            //{
            //    IncreaseHappiness(i);
            //}

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
            int necessaryHunger = Random.Range(people[index].hunger, 100);
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

        //private bool CanHarvestBerries()
        //{
        //    return food < startingPopulation * 2;
        //}

        //private void HarvestBerries()
        //{
        //    int berries = 0;
        //    for (int i = 0; i < startingPopulation; i++)
        //    {
        //        int berriesPerTree = Random.Range(minBerriesPerTree, maxBerriesPerTree + 1);
        //        berries += (int)(harvestRatio * berriesPerTree);
        //    }
        //    food += berries;
        //}

        //private void IncreaseHappiness(int index)
        //{
        //    people[index].happiness += Random.Range(0, maxHappinessIncreasePerInterval + 1);
        //    if (people[index].happiness > 100)
        //    {
        //        people[index].happiness = 100;
        //    }
        //}
    }

}
        
