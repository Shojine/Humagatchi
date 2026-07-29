using System;
using System.Collections;
using Unity.VisualScripting;
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

    void Start()
    {

        childRB2D = transform.GetComponentsInChildren<Rigidbody2D>();
        SetRagdollState(currentState);

        standingPosition = new Vector3[transform.childCount];
        standingRotation = new Vector3[transform.childCount];
        Debug.Log("Child Count: " + transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {

            standingPosition[i] = transform.GetChild(i).position;
            standingRotation[i] = transform.GetChild(i).rotation.eulerAngles;

        }

        StartCoroutine(stateRoutine());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    

    public void SetRagdollState(RagdollState newState)
    {
        currentState = newState;
        UpdateRagdollState();
    }

    private void UpdateRagdollState()
    {
        switch(currentState)
        {
            case RagdollState.Standing:
                for(int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).position = standingPosition[i];
                    transform.GetChild(i).rotation = Quaternion.Euler(standingRotation[i]);
                    Debug.DrawRay(standingPosition[i], Vector3.forward, Color.red, 5f);
                }
                EnableBones(false);
                currentState = RagdollState.Standing_Waiting;
                break;
            case RagdollState.Standing_Waiting:
                EnableBones(false);
                if (UnityEngine.Random.Range(0, 100) < randomFallingPercentage)
                {
                    currentState = RagdollState.Fallen;
                    UpdateRagdollState();
                    break;
                }

                break;
            case RagdollState.Fallen:
                EnableBones(true);
                StartCoroutine(standUp());
                break;
            case RagdollState.Slapped:
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
            rb2D.bodyType = isEnabled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }
    }

    private IEnumerator stateRoutine()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("State Routine Triggered");
        UpdateRagdollState();
        StartCoroutine(stateRoutine());
    }

    private IEnumerator standUp()
    {
        yield return new WaitForSeconds(5f);
        currentState = RagdollState.Standing;
        UpdateRagdollState();
    }


}