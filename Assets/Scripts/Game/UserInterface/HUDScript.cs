using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HUDScript : MonoBehaviour, IHumanSubscriber
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
    }

    private void Start()
    {
        HumanState state = HumanStateManager.Instance.RequestHumanState();
        updateHuman(state);
    }

    private void OnDestroy()
    {
        HumanStateManager.Instance.UnsubscribeFromHUman(this);
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

}
