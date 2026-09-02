using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HumanAI : MonoBehaviour
{
    //Wandering
    public float wanderRadius = 15.0f;
    public float minWait = 1f, maxWait = 5f;

    //Internal
    private NavMeshAgent humanAgent;
    private Animator humanAnim;
    private float waitTimer;

    void Awake()
    {
        humanAgent = GetComponent<NavMeshAgent>();
        humanAnim = GetComponent<Animator>();
    }

    private void OnEnable() => PickNewDest();

    // Update is called once per frame
    void Update()
    {
        humanAnim.SetFloat("Speed", humanAgent.velocity.magnitude);

        if (!humanAgent.pathPending && humanAgent.remainingDistance <= humanAgent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= Random.Range(minWait, maxWait))
            {
                PickNewDest();
                waitTimer = 0f;
            }
        }
    }

    void PickNewDest()
    {
        Vector3 randDirection = Random.insideUnitSphere;
        if (NavMesh.SamplePosition(randDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            humanAgent.SetDestination(hit.position);
        }
    }
}
