using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public enum RagdollState
{
    Standing,
    Standing_Waiting,
    Fallen,
    Slapped,
    Dead
}
public class RagdollController : MonoBehaviour
{
    [SerializeField] private int randomFallingPercentage = 90;

    public RagdollState currentState = RagdollState.Standing;
    private Rigidbody2D[] childRB2D;
    private Vector3[] standingPosition;
    private Vector3[] standingRotation;
    private bool isStanding = true;
    private bool setupComplete = false;
    private bool startStanding = false;
    private float standingCooldown = 5;

    void Start()
    {
        childRB2D = transform.GetComponentsInChildren<Rigidbody2D>();

        standingPosition = new Vector3[transform.childCount];
        standingRotation = new Vector3[transform.childCount];
        StartCoroutine(Setup());

    }

    // Update is called once per frame
    void Update()
    {
        if (startStanding && standingCooldown > 0)
        {
            standingCooldown -= 1 * Time.deltaTime;
        }else if(standingCooldown <= 0)
        {
            startStanding = false;
            standingCooldown = 5;
            SetRagdollState(RagdollState.Standing);
        }
    }

    

    public void SetRagdollState(RagdollState newState)
    {
        currentState = newState;
        UpdateRagdollState();
    }

    private void UpdateRagdollState()
    {
        if (!setupComplete) return;
        switch (currentState)
        {
            case RagdollState.Standing:
                if (!isStanding)
                {
                    isStanding = true;
                    EnableBones(false);

                    for(int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).position = standingPosition[i];
                        transform.GetChild(i).rotation = Quaternion.Euler(standingRotation[i]);
                        Debug.DrawRay(standingPosition[i], Vector3.forward, Color.red, 5f);
                    }
                }
                currentState = RagdollState.Standing_Waiting;
                break;
            case RagdollState.Standing_Waiting:
                if (UnityEngine.Random.Range(0, 100) < randomFallingPercentage)
                {
                    currentState = RagdollState.Fallen;
                    UpdateRagdollState();
                    break;
                }

                break;
            case RagdollState.Fallen:
                EnableBones(true);
                isStanding = false;
                startStanding = true;
                break;
            case RagdollState.Slapped:
                standingCooldown = 5;
                EnableBones(true);
                Vector3 mouseScreenPoz = Mouse.current.position.ReadValue();
                mouseScreenPoz.z = Camera.main.WorldToScreenPoint(transform.position).z;
                Vector3 mouseWorldPoz = Camera.main.ScreenToWorldPoint(mouseScreenPoz);
                mouseWorldPoz.z = 0;
                float force = 10;

                Vector2 direction = (transform.position - mouseWorldPoz).normalized;
                foreach (Rigidbody2D rigidbody2D in GetComponentsInChildren<Rigidbody2D>())
                {
                    rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
                }
                SetRagdollState(RagdollState.Fallen);
                break;
            case RagdollState.Dead:
                EnableBones(true);
                GetComponent<RagdollController>().enabled = false;
                break;
        }
    }

    private void EnableBones(bool isEnabled)
    {
        foreach (Rigidbody2D rb2D in childRB2D)
        {
            if (rb2D.CompareTag("LooseLimb")) continue;
            rb2D.bodyType = isEnabled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }
    }

    private IEnumerator StateRoutine()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("State Routine Triggered");
        UpdateRagdollState();
        StartCoroutine(StateRoutine());
    }

    private IEnumerator Setup()
    {
        EnableBones(false);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < transform.childCount; i++)
        {

            standingPosition[i] = transform.GetChild(i).position;
            standingRotation[i] = transform.GetChild(i).rotation.eulerAngles;
        }
        SetRagdollState(currentState);
        StartCoroutine(StateRoutine());
        setupComplete = true;
    }

}