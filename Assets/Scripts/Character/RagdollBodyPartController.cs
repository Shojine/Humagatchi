using UnityEngine;
using UnityEngine.EventSystems;

public class RagdollBodyPartController : MonoBehaviour, IClickable
{
    private RagdollController parent;


    void Start()
    {
        parent = transform.parent.GetComponent<RagdollController>();
    }
    public void OnClicked()
    {
        parent.SetRagdollState(RagdollState.Slapped);
    }

    public void OnHoverStart()
    {
       Debug.Log("Hovering over " + gameObject.name);
    }

    public void OnHoverStop()
    {
        
    }

}
