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

    public KeyCode ragdollKey = KeyCode.Mouse1;
    
    private NavMeshAgent agent;
    private HumanAI human;
    private bool isRagdolled;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        human = GetComponent<HumanAI>();
    }

    private void Start()
    {
        SetRagdollState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(ragdollKey))
            SetRagdollState(!isRagdolled);

        var sprites = GetComponentsInChildren<HumanBodyPart>();
        foreach (var sprite in  sprites)
        {
            sprite.transform.rotation = Quaternion.LookRotation(-sprite.targetCamera.transform.forward);
        }
    }

    void SetRagdollState(bool ragdollActive)
    {
        isRagdolled = ragdollActive;

        // Swap the per-bone power profile, then re-apply
        ragdollSettings.PowerProfile = ragdollActive ? ragdollProfile : kinematicProfile;
        ragdollSettings.ApplySettings(); // required after changing settings from code

        // Drop animation-matching so the flop isn't fighting invisible springs
        ragdollAnimator.MasterAlpha = ragdollActive ? 0f : 1f;

        agent.enabled = !ragdollActive;
        human.enabled = !ragdollActive;
    }
}
