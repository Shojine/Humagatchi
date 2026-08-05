using UnityEngine;

public class RoomSwapClickableObject : MonoBehaviour, IClickable
{
    [SerializeField] private Rooms room;

    //[SerializeField] private MeshRenderer renderer;
    //[SerializeField] private Material baseMaterial;
    //[SerializeField] private Material hoverMaterial;

    public void OnClicked()
    {
        GameManager.Instance.swapRoom(room);
    }

    public void OnHoverStart()
    {
        //renderer.material = hoverMaterial;
    }

    public void OnHoverStop()
    {
        //print("Is Unhovering");
        //renderer.material = baseMaterial;
    }
}
