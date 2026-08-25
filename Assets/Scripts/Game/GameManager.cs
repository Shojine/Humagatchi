using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;


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

public enum AmPm
{
    AM,
    PM
}

public class GameManager : MonoBehaviour, IDataPersistence
{
    [HideInInspector] public GameState gameState = GameState.LOADTITLE;
    // public static GameManager Instance { get; private set; }
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private List<IGameSubscriber> gameSubscribers = new List<IGameSubscriber>();

    private int totalMoney = 500;

    private bool sceneLoadingComplete = false;

    private bool titleSceneLoaded = false;
    private bool menuSceneLoaded = false;
    private bool gameSceneLoaded = false;

    private bool isLoadingTitle = false;
    private bool isLoadingMenu = false;
    private bool isLoadingGame = false;

    private bool isActivelyLoading = false;
    

    private bool isValidSaveFile = false;

    private bool isHovering = false;
    private IClickable clickableHovering = null;

    private Dictionary<Rooms, string> roomSceneNames = new Dictionary<Rooms, string>();
    private string currentSceneName = null;

    private bool isPaused = false;
    public bool isNotLoading = true;

    private float timeTimer;
    private int currentHour;
    private int currentMinute;
    private AmPm timeAmPm;
    private int currentDay = 0;

    [SerializeField] private GameState startingState = GameState.LOADTITLE;
    [SerializeField] private Rooms startingRoom = Rooms.LIVINGROOM;

    [SerializeField] private string titleScreenSceneName;
    [SerializeField] private string menuScreenSceneName;
    //[SerializeField] private string startingRoomSceneName;
    [SerializeField] private string LivingRoomSceneName;
    [SerializeField] private string KitchenSceneName;
    [SerializeField] private string BathroomSceneName;
    [SerializeField] private string BedroomSceneName;

    [SerializeField] private GameObject pausePanel;

    [SerializeField] private float TimeChangeTimer = 5.0f;
    [SerializeField] private int TimeChangeIncrement = 5;
    [SerializeField] private int defaultStartHour = 12;
    [SerializeField] private int defaultStartMinute = 30;
    [SerializeField] private AmPm defaultStartAmPm = AmPm.PM;

    [SerializeField] private int startingMoney = 500;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        roomSceneNames.Add(Rooms.LIVINGROOM, LivingRoomSceneName);
        roomSceneNames.Add(Rooms.BATHROOM, BathroomSceneName);
        roomSceneNames.Add(Rooms.BEDROOM, BedroomSceneName);
        roomSceneNames.Add(Rooms.KITCHEN, KitchenSceneName);

        pausePanel.SetActive(false);
        isPaused = false;

        timeTimer = TimeChangeTimer;
        currentHour = defaultStartHour;
        currentMinute = defaultStartMinute;
        timeAmPm = defaultStartAmPm;

        totalMoney = startingMoney;
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
       isHovering = false;
       clickableHovering = null;
        pausePanel.SetActive(false);
        isPaused = false;

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
        isActivelyLoading = true;
        if(currentSceneName != null)
        {
            yield return StartCoroutine(UnloadScene(currentSceneName));
        }
        yield return StartCoroutine(LoadScene(roomName));
        currentSceneName = roomName;
        isActivelyLoading = false;
    }



    #region GameStateUpdates

    /// <summary>
    /// Loads title screen scene and unloads all other non-base scenes
    /// </summary>
    private IEnumerator LoadTitleScene()
    {
        pausePanel.SetActive(false);
        isPaused = false;

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

        gameState = GameState.TITLE;
        sceneLoadingComplete = false;
        isLoadingTitle = false;
    }

    /// <summary>
    /// Loads main menu screen scene and unloads all other non-base scenes
    /// </summary>
    private IEnumerator LoadMenuScene()
    {
        pausePanel.SetActive(false);
        isPaused = false;

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

        gameState = GameState.MAINMENU;
        sceneLoadingComplete = false;
        isLoadingMenu = false;
    }

    /// <summary>
    /// Loads game scene, unloads all other non-base scenes, and starts game
    /// </summary>
    private IEnumerator LoadGameScene()
    {
        pausePanel.SetActive(false);
        isPaused= false;

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

            yield return StartCoroutine(LoadSceneByName(roomSceneNames[startingRoom]));


            sceneLoadingComplete = true;
        }

        gameState = GameState.PLAY;
        sceneLoadingComplete = false;
        isLoadingGame = false;
    }

    /// <summary>
    /// Runs all necessary update game manager functions for core gameplay
    /// </summary>
    private void PlayGameUpdate()
    {
        //ANY GAME MANAGER CRITICAL LOGIC GOES HERE!!!

        if(isActivelyLoading) return;

        //pause menu check
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            isPaused = true;
        }


        //Time changing
        timeTimer -= Time.deltaTime;
        if(timeTimer <= 0)
        {
            timeTimer = TimeChangeTimer;
            currentMinute += TimeChangeIncrement;

            if(currentMinute >= 60)
            {
                currentHour++;
                currentMinute %= 60;

                if(currentHour > 12)
                {
                    currentHour = 1;

                } else if(currentHour == 12)
                {
                    timeAmPm = (timeAmPm == AmPm.AM)? AmPm.PM : AmPm.AM;
                    if(timeAmPm == AmPm.AM) currentDay++;
                }

            }
            NotifyGameSubscribersTimeChange();
        }


        //Hovering and clicking things in the scene
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
        } else if (clickable2D != null)
        {
            Debug.Log("Clickable2D");

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Clicked");
                clickable2D.OnClicked();
            }
            else
            {
                if (!isHovering || clickable2D != clickableHovering)
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


       //if(clickable2D != null)
       //{
       //    Debug.Log("Clickable2D");
       //
       //    if (Mouse.current.leftButton.wasPressedThisFrame)
       //    {
       //        Debug.Log("Clicked");
       //        clickable2D.OnClicked();
       //    }
       //    else
       //    {
       //        if(!isHovering || clickable2D != clickableHovering)
       //        {
       //            clickableHovering = clickable2D;
       //            isHovering = true;
       //            clickable2D.OnHoverStart();
       //        }
       //    }
       //}
       //else
       //{
       //    clickableHovering?.OnHoverStop();
       //    isHovering = false;
       //    clickableHovering = null;
       //}
    }


    #endregion GameStateUpdates




    public void Unpause()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPaused = false;
    }

    /// <summary>
    /// Adds an amount of money to the total amount. (Use negative numbers to subtract money)
    /// </summary>
    /// <param name="amount">How much money being added (or removed if negative)</param>
    /// <returns>false if money removed exceeds total funds. True otherwise.</returns>
    public bool AddMoney(int amount)
    {
        if (amount < 0)
        {
            if((amount * -1) > totalMoney)
                return false;
        }

        totalMoney += amount;
        NotifyGameSubscribersMoneyChange();

        return true;
    }

    public int RequestTotalMoney()
    {
        return totalMoney;
    }

    public int RequestHour()
    {
        return currentHour;
    }

    public int RequestMinute()
    {
        return currentMinute;
    }

    public AmPm requestAmPm()
    {
        return timeAmPm;
    }

    public int requestDay()
    {
        return currentDay;
    }


    public void SubscribeToGame(IGameSubscriber subscriber)
    {
        if (subscriber != null && !gameSubscribers.Contains(subscriber))
        {
            gameSubscribers.Add(subscriber);
        }
    }

    public void UnsubscribeFromGame(IGameSubscriber subscriber)
    {
        if (subscriber != null && gameSubscribers.Contains(subscriber))
        {
            gameSubscribers.Remove(subscriber);
        }
    }
    private void NotifyGameSubscribersMoneyChange()
    {
        foreach (var subscriber in gameSubscribers)
        {
            subscriber.updateMoney(totalMoney);
        }
    }

    private void NotifyGameSubscribersTimeChange()
    {
        foreach(var subscriber in gameSubscribers)
        {
            subscriber.updateTime(currentHour, currentMinute, currentDay, timeAmPm);
        }
    }

    public void LoadData(GameData data)
    {
        if(data.totalFunds >=0)
        {
            totalMoney = data.totalFunds;
        }

        if(data.hour >= 0)
        {
            currentHour = data.hour;
            currentMinute = data.minute;
            timeTimer = data.timeTimer;
            timeAmPm = data.timeAmPm;
            currentDay = data.day;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.totalFunds = totalMoney;
        data.hour = currentHour;
        data.minute = currentMinute;
        data.timeTimer = timeTimer;
        data.timeAmPm = timeAmPm;
        data.day = currentDay;
    }


    public void Reset()
    {
        totalMoney = startingMoney;

        timeTimer = TimeChangeTimer;
        currentHour = defaultStartHour;
        currentMinute = defaultStartMinute;
        timeAmPm = defaultStartAmPm;

        currentDay = 0;
    }

    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame();
    }
}
