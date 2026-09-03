using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IHumanSubscriber
{
    [SerializeField] AudioSource mainSource;
    [SerializeField] AudioSource lowSource;

    [SerializeField] AudioClip clickSFX;
    [SerializeField] AudioClip actionSFX;
    [SerializeField] AudioClip failSFX;
    [SerializeField] AudioClip activitySFX;
    [SerializeField] AudioClip purchaseSFX;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        HumanStateManager.Instance.SubscribeToHuman(this);
    }

    private void OnDestroy()
    {
        HumanStateManager.Instance.UnsubscribeFromHUman(this);
    }

    public void updateHuman(HumanState state)
    {
        if (
            state.health <= 0 || state.hunger <= 0 || state.thirst <= 0 || state.cleanliness <= 0 || 
            state.energy <= 0 || state.happiness <= 0 || state.entertainment <= 0 || state.fear >= 100
            )
        {
            if (!lowSource.isPlaying) lowSource.Play();
        }
        else lowSource.Stop();
    }

    public void PlayButtonClick()
    {
        mainSource.clip = clickSFX;
        mainSource.Play();
    }

    public void PlayActionClick()
    {
        mainSource.clip = actionSFX;
        mainSource.Play();
    }

    public void PlayFailSFX()
    {
        mainSource.clip = failSFX;
        mainSource.Play();
    }

    public void PlayPurchaseSFX()
    {
        mainSource.clip = purchaseSFX;
        mainSource.Play();
    }

    public void PlayActivityClick()
    {
        mainSource.clip = activitySFX;
        mainSource.Play();
    }
}
