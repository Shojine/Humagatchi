using UnityEngine;

public class MainMenu : MonoBehaviour
{
    

    public void OnPlayClick()
    {
        GameManager.Instance.isNotLoading = true;
        GameManager.Instance.Reset();
        HumanStateManager.Instance.Reset();
        GameManager.Instance.gameState = GameState.STARTGAME;
    }

    public void OnContinueClick()
    {
        GameManager.Instance.isNotLoading = false;
        DataPersistenceManager.Instance.LoadGame();
        GameManager.Instance.gameState = GameState.STARTGAME;
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
