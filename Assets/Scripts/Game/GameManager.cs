using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    LOADTITLE,
    TITLE,
    LOADMAINMENU,
    MAINMENU,
    STARTGAME,
    PLAY
}


public class GameManager : MonoBehaviour
{
    public GameState gameState = GameState.LOADTITLE;
    public static GameManager Instance { get; private set; }


    private bool sceneIsLoaded = false;

    [SerializeField] private string TitleScreenSceneName;




    private void Update()
    {
        switch (gameState)
        {
            case GameState.LOADTITLE:
                if(!sceneIsLoaded)
                {
                    StartCoroutine(LoadScene(TitleScreenSceneName));

                    sceneIsLoaded = true;
                }

                sceneIsLoaded = false;
                break;
            case GameState.TITLE:
                break;
            case GameState.LOADMAINMENU:
                break;
            case GameState.MAINMENU:
                break;
            case GameState.STARTGAME:
                break;
            case GameState.PLAY:
                break;
            default:
                break;
        }
    }



    #region Load and Unload Scenes

    /// <summary>
    /// Loads a scene asynchronously
    /// </summary>
    /// <param name="sceneName">The name of the scene being loaded (MUST be the exact name of the scene (case insensitve) or the file path name if two scenes of the same name exist)</param>
    /// <returns>Yield return for Coroutine</returns>
    private IEnumerator LoadScene(string sceneName)
    {
        //Only loads scene if scene is not already loaded
        if (!SceneManager.GetSceneByName(sceneName).IsValid())
        {
            //Loads specified scene Async (for better performance) and additive so that the base scene remains present
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// Unloads a scene asynchronously
    /// </summary>
    /// <param name="sceneName">The name of the scene being loaded (MUST be the exact name of the scene (case insensitve) or the file path name if two scenes of the same name exist)</param>
    /// <returns>Yield return for Coroutine</returns>
    private IEnumerator UnloadScene(string sceneName)
    {
        //Only unloads scene if scene is already loaded
        if (SceneManager.GetSceneByName(sceneName).IsValid())
        {
            //Unloads specified scene Async (for better performance)
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    #endregion Load and Unload Scenes
}
