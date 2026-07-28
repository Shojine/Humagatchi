using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum RagdollState
{
    Standing, 
    Fallen, 
    Slapped,
    Dead
}
public class RagdollController : MonoBehaviour
{
    [SerializeField] GameObject bodyPart;

    public RagdollState currentState = RagdollState.Standing;
    private Rigidbody[] childRB;
    private Rigidbody2D[] childRB2D;
    private int randomFallingPercentage = 3;
    void Start()
    {
        childRB = transform.GetComponentsInChildren<Rigidbody>();
        SetRagdollState(currentState);
        childRB2D = transform.GetComponentsInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            SetRagdollState(RagdollState.Slapped);
        }

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
                EnableBones(false);
                if (UnityEngine.Random.Range(0, 100) < randomFallingPercentage)
                {
                    currentState = RagdollState.Fallen;
                    break;
                }
                    break;
            case RagdollState.Fallen:
                EnableBones(true);
                break;
            case RagdollState.Slapped:
                EnableBones(true);
                Vector3 mouseScreenPoz = Mouse.current.position.ReadValue();
                mouseScreenPoz.z = Camera.main.WorldToScreenPoint(transform.position).z;
                Vector3 mouseWorldPoz = Camera.main.ScreenToWorldPoint(mouseScreenPoz);
                mouseWorldPoz.z = 0;
                float force = 10;

                Vector2 direction = (transform.position - mouseWorldPoz).normalized;
                foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
                {
                    rb.AddForce(direction * force, ForceMode.Impulse);
                }
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
        foreach (Rigidbody rb in childRB)
        {
            rb.isKinematic = !isEnabled;
        }
        //foreach (Rigidbody2D rb2D in childRB2D)
        //{
        //    rb2D.bodyType = isEnabled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        //}
    }


}