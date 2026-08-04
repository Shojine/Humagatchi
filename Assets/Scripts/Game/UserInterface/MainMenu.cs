using UnityEngine;

public class MainMenu : MonoBehaviour
{
    

    public void OnPlayClick()
    {
        GameManager.Instance.gameState = GameState.STARTGAME;
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
