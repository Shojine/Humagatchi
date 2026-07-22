using UnityEngine;



public class TestClickableObject : MonoBehaviour, IClickable
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material hoverMaterial;

    public void OnClicked()
    {
        Debug.Log("Clicked");
    }

    public void OnHoverStart()
    {
        renderer.material = hoverMaterial;
    }

    public void OnHoverStop()
    {
        renderer.material = baseMaterial;
    }
}
