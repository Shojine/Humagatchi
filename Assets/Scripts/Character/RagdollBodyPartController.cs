using UnityEngine;
using UnityEngine.EventSystems;

public class RagdollBodyPartController : MonoBehaviour, IClickable, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked bitch");
        parent.SetRagdollState(RagdollState.Slapped);
    }
}
