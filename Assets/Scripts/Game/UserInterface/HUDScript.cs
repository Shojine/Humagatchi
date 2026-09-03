using System.Text;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HUDScript : MonoBehaviour, IHumanSubscriber, IGameSubscriber
{
    [SerializeField] public Image testSlider;
    [SerializeField] public Image healthSlider;
    [SerializeField] public Image hungerSlider;
    [SerializeField] public Image thirstSlider;
    [SerializeField] public Image cleanlinessSlider;
    [SerializeField] public Image energySlider;
    [SerializeField] public Image happinessSlider;
    [SerializeField] public Image entertainmentSlider;
    [SerializeField] public Image fearSlider;

    [SerializeField] public TMPro.TMP_Text moneyText;
    [SerializeField] public TMPro.TMP_Text timeText;
    [SerializeField] public TMPro.TMP_Text dayText;

    
    //[SerializeField] private string titleScreenSceneName;

    //healthTimer
    //hungerTimer
    //thirstTimer
    //cleanlinessTimer
    //energyTimer
    //happinessTimer
    //entertainmentTimer
    //fearTimer



    private void Awake()
    {
        HumanStateManager.Instance.SubscribeToHuman(this);
        GameManager.Instance.SubscribeToGame(this);
    }

    private void Start()
    {
        HumanState state = HumanStateManager.Instance.RequestHumanState();
        updateHuman(state);

        updateMoney(GameManager.Instance.RequestTotalMoney());
        updateTime(GameManager.Instance.RequestHour(), GameManager.Instance.RequestMinute(), GameManager.Instance.requestDay(), GameManager.Instance.requestAmPm());
    }

    private void OnDestroy()
    {
        HumanStateManager.Instance.UnsubscribeFromHUman(this);
        GameManager.Instance.UnsubscribeFromGame(this);
    }


    public void updateHuman(HumanState state)
    {
        if(testSlider == null)
        {
            print("Slider Is Null!!!");
        }

        //testSlider.fillAmount = (state.hunger/100.0f);
        healthSlider.fillAmount = (state.health/100.0f);
        hungerSlider.fillAmount = (state.hunger/100.0f);
        thirstSlider.fillAmount = (state.thirst/100.0f);
        cleanlinessSlider.fillAmount = (state.cleanliness/100.0f);
        energySlider.fillAmount = (state.energy/100.0f);
        happinessSlider.fillAmount = (state.happiness/100.0f);
        entertainmentSlider.fillAmount = (state.entertainment/100.0f);
        fearSlider.fillAmount = (state.fear/100.0f);
    }

    public void updateMoney(int currentFunds)
    {
        moneyText.text = currentFunds.ToString();
    }

    public void updateTime(int hour, int minute, int day, AmPm amPm)
    {
        StringBuilder timeSb = new StringBuilder();

        timeSb.Append(hour.ToString());
        timeSb.Append(":");
        if (minute < 10) timeSb.Append("0");
        timeSb.Append(minute.ToString());
        if (amPm == AmPm.AM)
        {
            timeSb.Append("am");
        }
        else
        {
            timeSb.Append("pm");
        }

        string timeString = timeSb.ToString();
        timeText.text = timeString;

        StringBuilder daySb = new StringBuilder();

        dayText.text = "Day " + day.ToString();
    }
}
