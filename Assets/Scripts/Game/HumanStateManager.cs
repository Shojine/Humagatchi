using System.Collections.Generic;
using UnityEngine;

public struct HumanStateChange
{
    public float healthChange;
    public float hungerChange;
    public float thirstChange;
    public float cleanlinessChange;
    public float energyChange;
    public float happinessChange; //happiness to anger/sadness scale
    public float entertainmentChange;
    public float fearChange;
}


public struct HumanState
{
    public float health;
    public float hunger;
    public float thirst;
    public float cleanliness;
    public float energy;
    public float happiness; //happiness to anger/sadness scale
    public float entertainment;
    public float fear;
}

public class HumanStateManager : MonoBehaviour, IDataPersistence
{
    private static HumanStateManager _instance;
    public static HumanStateManager Instance { get { return _instance; } }

    private List<IHumanSubscriber> humanSubscribers = new List<IHumanSubscriber>();

    private float health = 100;
    private float hunger = 100;
    private float thirst = 100;
    private float cleanliness = 100;
    private float energy = 100;
    private float happiness = 100;
    private float entertainment = 100;
    private float fear = 0;


    private float healthTimer = 100;
    private float hungerTimer = 100;
    private float thirstTimer = 100;
    private float cleanlinessTimer = 100;
    private float energyTimer = 100;
    private float happinessTimer = 100;
    private float entertainmentTimer = 100;
    private float fearTimer = 100;


    [SerializeField] private float startingHealth = 100;
    [SerializeField] private float startingHunger = 100;
    [SerializeField] private float startingThirst = 100;
    [SerializeField] private float startingCleanliness = 100;
    [SerializeField] private float startingEnergy = 100;
    [SerializeField] private float startingEntertainment = 100;
    [SerializeField] private float startingFear = 0;

    [SerializeField] private float healthChangeTime = 100;
    [SerializeField] private float hungerDecreaseTime = 10;
    [SerializeField] private float thirstDecreaseTime = 10;
    [SerializeField] private float cleanlinessDecreaseTime = 20;
    [SerializeField] private float energyChangeTime =15;
    [SerializeField] private float happinessChangeTime = 12;
    [SerializeField] private float entertainmentChangeTime = 7;
    [SerializeField]  private float fearChangeTime = 40;

    [SerializeField] private float baseHealthDecrease = 3;
    [SerializeField] private float baseHungerDecrease = 2;
    [SerializeField] private float baseThirstDecrease = 1;
    [SerializeField] private float baseCleanlinessDecrease = 4;
    [SerializeField] private float baseEnergyDecrease = 3;
    [SerializeField] private float baseEntertainmentDecrease = 3;
    [SerializeField] private float baseFearIncrease = 5;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        if( startingHealth<= 100 && startingHealth >= 0) health = startingHealth;
        if( startingHunger <= 100 && startingHunger >= 0) hunger = startingHunger;
        if( startingThirst <= 100 && startingThirst >= 0) thirst = startingThirst;
        if( startingCleanliness <= 100 && startingCleanliness >= 0) cleanliness = startingCleanliness;
        if(  startingEnergy <= 100 && startingEnergy >= 0) energy = startingEnergy;
        if( startingEntertainment <= 100 && startingEntertainment >= 0) entertainment = startingEntertainment;
        if( startingFear <= 100 && startingFear >= 0) fear = startingFear;

        happiness = ((health + hunger + thirst + cleanliness + energy + entertainment) / 6) - fear;

        healthTimer = healthChangeTime;
        hungerTimer = hungerDecreaseTime;
        thirstTimer = thirstDecreaseTime;
        cleanlinessTimer = cleanlinessDecreaseTime;
        energyTimer = energyChangeTime;
        happinessTimer = happinessChangeTime;
        entertainmentTimer = entertainmentChangeTime;
        fearTimer = fearChangeTime;
    }


    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.PLAY) return;

        bool changeOccured = false;

        healthTimer -= Time.deltaTime;
        hungerTimer -= Time.deltaTime;
        thirstTimer -= Time.deltaTime;
        cleanlinessTimer -= Time.deltaTime;
        energyTimer -= Time.deltaTime;
        happinessTimer -= Time.deltaTime;
        entertainmentTimer -= Time.deltaTime;
        fearTimer -= Time.deltaTime;

        if(healthTimer <= 0)
        {
            healthTimer = healthChangeTime;
            changeOccured = true;

            if(hunger <= 0)
            {
                health -= baseHealthDecrease;
            }

            if(thirst <= 0)
            {
                health -= baseHealthDecrease;
            }

            if(cleanliness <= 0)
            {
                health -= baseHealthDecrease;
            }

            if(energyTimer <= 0)
            {
                health -= baseHealthDecrease;
            }

            if(happiness <= 0)
            {
                health -= baseHealthDecrease;
            }

            //excluding entertainment here. Can put it in if we want

            if(fear <= 0)
            {
                health -= baseHealthDecrease;
            }

        }

        if(hungerTimer <= 0)
        {
            print("Changing Hunger");

            hungerTimer = hungerDecreaseTime;
            changeOccured = true;


            hunger -= baseHungerDecrease;
        }    

        if(thirstTimer <= 0)
        {
            thirstTimer = thirstDecreaseTime;
            changeOccured = true;

            thirst -= baseThirstDecrease;

            if(hunger <= 20)
            {
                thirst -= (0.5f * baseThirstDecrease);
            }
        }

        if(cleanlinessTimer <= 0)
        {
            cleanlinessTimer = cleanlinessDecreaseTime;
            changeOccured = true;

            cleanliness -= baseCleanlinessDecrease;

            if(energy >= 75 && entertainment >= 75)
            {
                cleanliness -= (baseCleanlinessDecrease * 0.25f);
            }
        }

        if(energyTimer <= 0)
        {
            energyTimer = energyChangeTime;
            changeOccured = true;

            energy -= baseEnergyDecrease;

            if(hunger <= 25)
            {
                energy -= baseEnergyDecrease;
            }

            if(thirst <= 20)
            {
                energy -= (baseEnergyDecrease * 0.5f);
            }
        }

        if (happinessTimer <= 0)
        {
            happinessTimer = happinessChangeTime;
            changeOccured = true;

            happiness = ((health + hunger + thirst + cleanliness + energy + entertainment) / 6) - fear;
        }

        if (entertainmentTimer <= 0)
        {
            entertainmentTimer = entertainmentChangeTime;
            changeOccured = true;

            entertainment -= baseEntertainmentDecrease;

            if(energy <= 25)
            {
                entertainment -= (baseEntertainmentDecrease * 0.75f);
            }
        }

        if (fearTimer <= 0)
        {
            fearTimer = fearChangeTime;
            changeOccured = true;

            if(health <= 20)
            {
                fear += (baseFearIncrease * 2);
            } else if (health <= 50)
            {
                fear += baseFearIncrease;
            }

            if (hunger <= 5)
            {
                fear += (baseFearIncrease * 0.75f);
            }
            else if (hunger <= 25)
            {
                fear += (baseFearIncrease * 0.5f);
            }

            if (thirst <= 5)
            {
                fear += (baseFearIncrease * 1f);
            }
            else if (thirst <= 25)
            {
                fear += (baseFearIncrease * 0.75f);
            }

            if (entertainment >= 75 && health >= 75 && hunger >= 50 && thirst >= 50 && cleanliness >= 25)
            {
                fear -= (baseFearIncrease * 0.75f);
            }

            if (health >= 90 && hunger >= 85 && thirst >= 85 && cleanliness >= 50)
            {
                fear -= (baseFearIncrease * 1.5f);
            }


        }

        if(changeOccured)
        {
            NotifyHumanSubscribers();
        }    

        //healthTimer
        //hungerTimer
        //thirstTimer
        //cleanlinessTimer
        //energyTimer
        //happinessTimer
        //entertainmentTimer
        //fearTimer
    }

    public void ChangeHumanState(HumanStateChange stateChange)
    {
        health += stateChange.healthChange;
        hunger += stateChange.hungerChange;
        thirst += stateChange.thirstChange;
        cleanliness += stateChange.cleanlinessChange;
        energy += stateChange.energyChange;
        happiness += stateChange.happinessChange;
        entertainment += stateChange.entertainmentChange;
        fear += stateChange.fearChange;

        if(health > 100) health = 100;
        if(hunger > 100) hunger = 100;
        if(thirst > 100) thirst = 100;
        if(cleanliness > 100) cleanliness = 100;
        if(energy > 100) energy = 100;
        if(happiness > 100) happiness = 100;
        if(entertainment > 100) entertainment = 100;
        if(fear > 100) fear = 100;

        NotifyHumanSubscribers();
    }


    public void SubscribeToHuman(IHumanSubscriber subscriber)
    {
        if(subscriber != null && !humanSubscribers.Contains(subscriber))
        {
            humanSubscribers.Add(subscriber);
        }
    }

    public void UnsubscribeFromHUman(IHumanSubscriber subscriber)
    {
        if(subscriber != null && humanSubscribers.Contains(subscriber))
        {
            humanSubscribers.Remove(subscriber);
        }
    }

    private void NotifyHumanSubscribers()
    {
        HumanState humanState = new HumanState();
        humanState.health = health;
        humanState.hunger = hunger;
        humanState.thirst = thirst;
        humanState.cleanliness = cleanliness;
        humanState.energy = energy;
        humanState.happiness = happiness;
        humanState.entertainment = entertainment;
        humanState.fear = fear;

        foreach (var subscriber in humanSubscribers)
        {
            subscriber.updateHuman(humanState);
        }
    }

    public HumanState RequestHumanState()
    {
        HumanState humanState = new HumanState();
        humanState.health = health;
        humanState.hunger = hunger;
        humanState.thirst = thirst;
        humanState.cleanliness = cleanliness;
        humanState.energy = energy;
        humanState.happiness = happiness;
        humanState.entertainment = entertainment;
        humanState.fear = fear;

        return humanState;
    }

    public void LoadData(GameData data)
    {
        if(data.HumanHealth >=0)
        {
            health = data.HumanHealth;
        }

        if(data.HumanHunger >= 0)
        {
            hunger = data.HumanHunger;
        }

        if(data.HumanThirst >= 0)
        {
            thirst = data.HumanThirst;
        }

        if(data.HumanCleanliness >= 0)
        {
            cleanliness = data.HumanCleanliness;
        }

        if(data.HumanEnergy >= 0)
        {
            energy = data.HumanEnergy;
        }

        if(data.HumanHappiness >= 0)
        {
            happiness = data.HumanHappiness;
        }

        if(data.HumanEntertainment >= 0)
        {
            entertainment = data.HumanEntertainment;
        }

        if(data.HumanFear >= 0)
        {
            fear = data.HumanFear;
        }

        if (health > 100) health = 100;
        if (hunger > 100) hunger = 100;
        if (thirst > 100) thirst = 100;
        if (cleanliness > 100) cleanliness = 100;
        if (energy > 100) energy = 100;
        if (happiness > 100) happiness = 100;
        if (entertainment > 100) entertainment = 100;
        if (fear > 100) fear = 100;

        NotifyHumanSubscribers();
    }

    public void SaveData(ref GameData data)
    {
        data.HumanHealth = health;
        data.HumanHunger = hunger;
        data.HumanThirst = thirst;
        data.HumanCleanliness = cleanliness;
        data.HumanEnergy = energy;
        data.HumanHappiness = happiness;
        data.HumanEntertainment = entertainment;
        data.HumanFear = fear;
    }

    public void Reset()
    {
        health = startingHealth;
        hunger = startingHunger;
        thirst = startingThirst;
        cleanliness = startingCleanliness;
        energy = startingEnergy;
        entertainment = startingEntertainment;
        fear = startingFear;
        happiness = ((health + hunger + thirst + cleanliness + energy + entertainment) / 6) - fear;

        NotifyHumanSubscribers();
    }
}
