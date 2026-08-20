using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HUDScript : MonoBehaviour, IHumanSubscriber
{
    [SerializeField] public Slider healthSlider;
    [SerializeField] public Slider hungerSlider;
    [SerializeField] public Slider thirstSlider;
    [SerializeField] public Slider cleanlinessSlider;
    [SerializeField] public Slider energySlider;
    [SerializeField] public Slider happinessSlider;
    [SerializeField] public Slider entertainmentSlider;
    [SerializeField] public Slider fearSlider;

    
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

    private void OnDestroy()
    {
        HumanStateManager.Instance.UnsubscribeFromHUman(this);
    }


    public void updateHuman(HumanState state)
    {
        healthSlider.value = state.health;
        hungerSlider.value = state.hunger;
        thirstSlider.value = state.thirst;
        cleanlinessSlider.value = state.cleanliness;
        energySlider.value = state.energy;
        happinessSlider.value = state.happiness;
        entertainmentSlider.value = state.entertainment;
        fearSlider.value = state.fear;
    }

}
