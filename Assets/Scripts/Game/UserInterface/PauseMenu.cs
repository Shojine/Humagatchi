using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnUnpauseClick()
    {
        AudioManager.Instance.PlayButtonClick();
        GameManager.Instance.Unpause();
    }

    public void OnMenuClick()
    {
        AudioManager.Instance.PlayButtonClick();
        GameManager.Instance.Unpause();
        GameManager.Instance.gameState = GameState.LOADMAINMENU;
    }

    public void OnSaveClick()
    {
        AudioManager.Instance.PlayButtonClick();
        //print("Save Button Clicked. Not Yet Functional.");
        DataPersistenceManager.Instance.SaveGame();
    }
}
