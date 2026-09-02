using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnUnpauseClick()
    {
        GameManager.Instance.Unpause();
    }

    public void OnMenuClick()
    {
        GameManager.Instance.Unpause();
        GameManager.Instance.gameState = GameState.LOADMAINMENU;
    }

    public void OnSaveClick()
    {
        //print("Save Button Clicked. Not Yet Functional.");
        DataPersistenceManager.Instance.SaveGame();
    }
}
