using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] GameObject bodyPart;
  
    private Rigidbody[] childRB;
    void Start()
    {
        childRB = transform.GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisableBones()
    {
        for (int i = 0; i < childRB.Length; i++)
        {
            childRB[i].isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        bodyPart.GetComponent<Renderer>().material.color = Color.red;
    }
}