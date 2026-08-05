using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

public enum Rooms
{
    LIVINGROOM,
    KITCHEN,
    BATHROOM,
    BEDROOM
}

public class GameManager : MonoBehaviour
{
    [HideInInspector] public GameState gameState = GameState.LOADTITLE;
    public static GameManager Instance { get; private set; }


    private bool sceneLoadingComplete = false;

    private bool titleSceneLoaded = false;
    private bool menuSceneLoaded = false;
    private bool gameSceneLoaded = false;

    private bool isLoadingTitle = false;
    private bool isLoadingMenu = false;
    private bool isLoadingGame = false;
    

    private bool isValidSaveFile = false;

    private bool isHovering = false;
    private IClickable clickableHovering = null;

    private Dictionary<Rooms, string> roomSceneNames = new Dictionary<Rooms, string>();
    private string currentSceneName = null;


    [SerializeField] private GameState startingState = GameState.LOADTITLE;

    [SerializeField] private string titleScreenSceneName;
    [SerializeField] private string menuScreenSceneName;
    [SerializeField] private string startingRoomSceneName;
    [SerializeField] private string LivingRoomSceneName;
    [SerializeField] private string KitchenSceneName;
    [SerializeField] private string BathroomSceneName;
    [SerializeField] private string BedroomSceneName;


    private void Awake()
    {
        roomSceneNames.Add(Rooms.LIVINGROOM, LivingRoomSceneName);
        roomSceneNames.Add(Rooms.BATHROOM, BathroomSceneName);
        roomSceneNames.Add(Rooms.BEDROOM, BedroomSceneName);
        roomSceneNames.Add(Rooms.KITCHEN, KitchenSceneName);
    }

    private void Start()
    {
        gameState = startingState;
    }



    private void Update()
    {
        switch (gameState)
        {
            case GameState.LOADTITLE:
                if(!isLoadingTitle)
                {
                    StartCoroutine(LoadTitleScene());
                    isLoadingTitle = true;
                }
                break;
            case GameState.TITLE:
                break;
            case GameState.LOADMAINMENU:
                if(!isLoadingMenu)
                {
                    StartCoroutine(LoadMenuScene());
                    isLoadingMenu = true;
                }
                break;
            case GameState.MAINMENU:
                break;
            case GameState.STARTGAME:
                if(!isLoadingGame)
                {
                    StartCoroutine(LoadGameScene());
                    isLoadingGame = true;
                }
                break;
            case GameState.PLAY:
                PlayGameUpdate();
                break;
            default:
                break;
        }
    }



    public void SetValidSaveFile(bool validSaveFile)
    {
        isValidSaveFile = validSaveFile;
    }

    public void swapRoom(Rooms room)
    {
        StartCoroutine(LoadSceneByName(roomSceneNames[room]));
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

    private IEnumerator LoadSceneByName(string roomName)
    {
        if(currentSceneName != null)
        {
            yield return StartCoroutine(UnloadScene(currentSceneName));
        }
        yield return StartCoroutine(LoadScene(roomName));
        currentSceneName = roomName;
    }



    #region GameStateUpdates

    /// <summary>
    /// Loads title screen scene and unloads all other non-base scenes
    /// </summary>
    private IEnumerator LoadTitleScene()
    {
        if (!sceneLoadingComplete)
        {
            //if (menuSceneLoaded)
            //{
            //    StartCoroutine(UnloadScene(menuScreenSceneName));
            //    menuSceneLoaded = false;
            //}
            //
            //if (gameSceneLoaded)
            //{
            //    StartCoroutine(UnloadScene(gameSceneName));
            //    gameSceneLoaded = false;
            //}
            //
            //if (!titleSceneLoaded)
            //{
            //    StartCoroutine(LoadScene(titleScreenSceneName));
            //    titleSceneLoaded = true;
            //}

            yield return LoadSceneByName(titleScreenSceneName);


            sceneLoadingComplete = true;
        }

        sceneLoadingComplete = false;
        isLoadingTitle = false;
    }

    /// <summary>
    /// Loads main menu screen scene and unloads all other non-base scenes
    /// </summary>
    private IEnumerator LoadMenuScene()
    {
        if (!sceneLoadingComplete)
        {
            //if (titleSceneLoaded)
            //{
            //    StartCoroutine(UnloadScene(titleScreenSceneName));
            //    titleSceneLoaded = false;
            //}
            //
            //if (gameSceneLoaded)
            //{
            //    StartCoroutine(UnloadScene(gameSceneName));
            //    gameSceneLoaded = false;
            //}
            //
            //if (!menuSceneLoaded)
            //{
            //    StartCoroutine(LoadScene(menuScreenSceneName));
            //    menuSceneLoaded = true;
            //}

            yield return StartCoroutine(LoadSceneByName(menuScreenSceneName));

            sceneLoadingComplete = true;
        }

        sceneLoadingComplete = false;
        isLoadingMenu = false;
    }

    /// <summary>
    /// Loads game scene, unloads all other non-base scenes, and starts game
    /// </summary>
    private IEnumerator LoadGameScene()
    {
        if (!sceneLoadingComplete)
        {
            //if (titleSceneLoaded)
            //{
            //    StartCoroutine(UnloadScene(titleScreenSceneName));
            //    titleSceneLoaded = false;
            //}
            //
            //if (menuSceneLoaded)
            //{
            //    StartCoroutine(UnloadScene(menuScreenSceneName));
            //    menuSceneLoaded = false;
            //}
            //
            //if (!gameSceneLoaded)
            //{
            //    StartCoroutine(LoadScene(gameSceneName));
            //    gameSceneLoaded = true;
            //}

            yield return StartCoroutine(LoadSceneByName(startingRoomSceneName));


            sceneLoadingComplete = true;
        }

        sceneLoadingComplete = false;
        isLoadingGame = false;
    }

    /// <summary>
    /// Runs all necessary update game manager functions for core gameplay
    /// </summary>
    private void PlayGameUpdate()
    {
        //ANY GAME MANAGER CRITICAL LOGIC GOES HERE!!!

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        IClickable clickable = ((Physics.Raycast(ray, out hit))) ?hit.collider.gameObject.GetComponent<IClickable>() : null;


        Vector3 mousePos = Vector3.zero;
        if(Mouse.current != null) mousePos = Mouse.current.position.ReadValue();

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        IClickable clickable2D = (hit2D.collider != null) ? hit2D.collider.gameObject.GetComponent<IClickable>() : null;

        Debug.DrawRay(ray.origin,Vector3.forward);

        if (clickable != null)
        {
            Debug.Log("Clickable");
        
            if (Input.GetMouseButtonDown(0))
            {
                clickable.OnClicked();
            }
            else
            {
                if(!isHovering || clickable != clickableHovering)
                {
                    clickableHovering = clickable;
                    isHovering = true;
                    clickable.OnHoverStart();
                }
            }
        }
        else
        {
            clickableHovering?.OnHoverStop();
            isHovering = false;
            clickableHovering = null;
        }

        if(clickable2D != null)
        {
            Debug.Log("Clickable2D");
        
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Clicked");
                clickable2D.OnClicked();
            }
            else
            {
                if(!isHovering || clickable2D != clickableHovering)
                {
                    clickableHovering = clickable2D;
                    isHovering = true;
                    clickable2D.OnHoverStart();
                }
            }
        }
        else
        {
            clickableHovering?.OnHoverStop();
            isHovering = false;
            clickableHovering = null;
        }
    }


    #endregion GameStateUpdates


}
