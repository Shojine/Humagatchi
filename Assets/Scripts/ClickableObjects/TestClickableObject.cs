using UnityEngine;



public class TestClickableObject : MonoBehaviour, IClickable
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material hoverMaterial;

    public void OnClicked()
    {
        Debug.Log("Clicked");
        GameManager.Instance.swapRoom(Rooms.KITCHEN);
        //GameManager.Instance.gameState = GameState.PLAY;
    }

    public void OnHoverStart()
    {
        renderer.material = hoverMaterial;
    }

    public void OnHoverStop()
    {
        print("Is Unhovering");
        renderer.material = baseMaterial;
    }
}
