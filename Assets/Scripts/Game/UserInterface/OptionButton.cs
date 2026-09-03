using TMPro;
using UnityEngine;

public enum InteractionType
{
    PURCHASE,
    ACTIVITY,
    ACTION
}

public class OptionButton : MonoBehaviour
{ 
    [SerializeField] private GameObject moneys;
    [SerializeField] private GameObject hours;
    [SerializeField] private GameObject mood;

    [SerializeField] private TMP_Text optionText;
    [SerializeField] private TMP_Text moneysText;
    [SerializeField] private TMP_Text hoursText;

    // TEMPORARY should be randomized actions later
    [SerializeField] private string optionName;
    [SerializeField] private InteractionType interactionType;
    [SerializeField] private int cost;
    [SerializeField] private float healthChange;
    [SerializeField] private float hungerChange;
    [SerializeField] private float thirstChange;
    [SerializeField] private float cleanlinessChange;
    [SerializeField] private float energyChange;
    [SerializeField] private float happinessChange; //happiness to anger/sadness scale
    [SerializeField] private float entertainmentChange;
    [SerializeField] private float fearChange;

    void Start()
    {
        optionText.text = optionName;

        moneys.SetActive(false);
        hours.SetActive(false);
        mood.SetActive(false);

        switch (interactionType)
        {
            case InteractionType.PURCHASE: 
                moneys.SetActive(true);
                moneysText.text = cost.ToString();
                break;
            case InteractionType.ACTIVITY:
                hours.SetActive(true);
                hoursText.text = $"{cost} hrs";
                break;
            case InteractionType.ACTION:
                mood.SetActive(true);
                break;
        }
    }

    public void OnClick()
    {
        if (interactionType == InteractionType.PURCHASE)
        {
            if (GameManager.Instance.RequestTotalMoney() >= cost) GameManager.Instance.AddMoney(-cost);
            else return;
        }
        else if (interactionType == InteractionType.ACTIVITY) GameManager.Instance.AddTime(0, cost, 0);

        HumanStateChange stateChange = new();
        stateChange.healthChange = healthChange;
        stateChange.hungerChange = hungerChange;
        stateChange.thirstChange = thirstChange;
        stateChange.cleanlinessChange = cleanlinessChange;
        stateChange.energyChange = energyChange;
        stateChange.happinessChange = happinessChange;
        stateChange.entertainmentChange = entertainmentChange;
        stateChange.fearChange = fearChange;

        HumanStateManager.Instance.ChangeHumanState(stateChange);
    }
}
