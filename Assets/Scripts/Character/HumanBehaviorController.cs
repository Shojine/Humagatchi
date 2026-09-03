using Hairibar.Ragdoll;
using Hairibar.Ragdoll.Animation;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviorController : MonoBehaviour
{
    [Header("References")]
    public RagdollSettings ragdollSettings;   // on the generated "Ragdoll" object
    public RagdollAnimator ragdollAnimator;   // on the "Target" object (alongside Animator)

    [Header("Power profiles")]
    public RagdollPowerProfile kinematicProfile; // every bone = Kinematic
    public RagdollPowerProfile ragdollProfile;   // every bone = Unpowered

    [Header("Sprites")]
    [SerializeField] public Sprite neutralSprite;
    [SerializeField] public Sprite hungrySprite;
    [SerializeField] public Sprite thirstySprite;
    [SerializeField] public Sprite uncleanSprite;
    [SerializeField] public Sprite lowEnergySprite;
    [SerializeField] public Sprite happySprite;
    [SerializeField] public Sprite boredSprite;
    [SerializeField] public Sprite fearfulSprite;
    [SerializeField] public Sprite multinegativesSprite;

    public KeyCode ragdollKey = KeyCode.Mouse1;
    
    private NavMeshAgent agent;
    private HumanAI human;
    private bool isRagdolled;
    private SpriteRenderer expressionSprite;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        human = GetComponent<HumanAI>();
    }

    private void Start()
    {
        SetRagdollState(false);
        var headSprite = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in headSprite)
        {
            if (sprite.tag == "Expression")
            {
                expressionSprite = sprite;
                return;
            }
        }
    }

    void Update()
    {
        int negativeCount = 0;
        if (Input.GetKeyDown(ragdollKey))
            SetRagdollState(!isRagdolled);

        var sprites = GetComponentsInChildren<HumanBodyPart>();
        foreach (var sprite in sprites)
        {
            sprite.transform.rotation = Quaternion.LookRotation(-sprite.targetCamera.transform.forward);
        }

        var currentState = HumanStateManager.Instance.RequestHumanState();

        if (expressionSprite != null) {
            if (currentState.fear >= 80) 
            {
                expressionSprite.sprite = fearfulSprite;
                negativeCount++;
            }
            if (currentState.thirst >= 45) 
            {
                expressionSprite.sprite = thirstySprite;
                negativeCount++;
            }
            if (currentState.hunger >= 50) 
            {
                expressionSprite.sprite = hungrySprite;
                negativeCount++;
            }
            if (currentState.cleanliness < 45)            
            {
                expressionSprite.sprite = uncleanSprite;
                negativeCount++;
            }
            if (currentState.energy < 30) 
            {
                expressionSprite.sprite = lowEnergySprite;
                negativeCount++;
            }
            if (currentState.entertainment < 60) 
            {
                expressionSprite.sprite = boredSprite;
                negativeCount++;
            }
            if (currentState.happiness >= 75) 
            {
                expressionSprite.sprite = happySprite;
                negativeCount--;
            }
            if (negativeCount >= 3)
            {
                expressionSprite.sprite = multinegativesSprite;
            }
            if (negativeCount <= 0)
            {
                expressionSprite.sprite = neutralSprite;
            }
        }
    }

    void SetRagdollState(bool ragdollActive)
    {
        isRagdolled = ragdollActive;

        // Swap the per-bone power profile, then re-apply
        ragdollSettings.PowerProfile = ragdollActive ? ragdollProfile : kinematicProfile;
        ragdollSettings.ApplySettings(); // required after changing settings from code

        agent.enabled = !ragdollActive;
        human.enabled = !ragdollActive;
    }
}
