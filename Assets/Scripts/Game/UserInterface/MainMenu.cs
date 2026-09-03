using UnityEngine;

public class MainMenu : MonoBehaviour
{
    

    public void OnPlayClick()
    {
        AudioManager.Instance.PlayButtonClick();
        GameManager.Instance.isNotLoading = true;
        GameManager.Instance.Reset();
        HumanStateManager.Instance.Reset();
        GameManager.Instance.gameState = GameState.STARTGAME;
    }

    public void OnContinueClick()
    {
        AudioManager.Instance.PlayButtonClick();
        GameManager.Instance.isNotLoading = false;
        DataPersistenceManager.Instance.LoadGame();
        GameManager.Instance.gameState = GameState.STARTGAME;
    }

    public void OnQuitClick()
    {
        AudioManager.Instance.PlayButtonClick();
        Application.Quit();
    }
}
