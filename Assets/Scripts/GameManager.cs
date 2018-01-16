using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region SubManager Using Directives
using SubManager;
using SubManager.Ad;
using SubManager.CameraMan;
using SubManager.Player;
using SubManager.Social;
using SubManager.Spawn;
using SubManager.World;
using SubManager.Menu;
using SubManager.Difficulty;
using SubManager.CharacterMan;
using SubManager.Physics;
#endregion

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;
    public bool debugMode = false;
    bool setupErroredOut;
    public bool isLoading; //menu submanager tracks this for its loading UI    
    public float globaltimer;

    //states of the game
    public enum GameStates
    {
        Setup,
        Pre,
        Intra,
        Post
    }
    public GameStates currentGameState;

    //Types for sub managers
    public enum GameSubManagerTypes
    {
        None,
        Player,
        Spawn,
        Camera,
        World,
        Ad,
        Purchase,
        Social,
        Menu,
        Character,
        Difficulty,
        Physics
    }

    #endregion

    #region Properties

    //nothing here yet

    #endregion

    #region Events

    //Sub Managers init complete event
    public delegate void InitCompleteAction();
    public static event InitCompleteAction OnInitComplete;

    //OnGameLoad event
    public delegate void GameLoadAction();
    public static event GameLoadAction OnGameLoad;

    //OnGameStart event
    public delegate void GameStartAction();
    public static event GameStartAction OnGameStart;

    //OnGameEnd event       aka.Player_Died_Event
    public delegate void GameEndAction();
    public static event GameEndAction OnGameEnd;

    //OnGameReset event
    public delegate void GameResetAction();
    public static event GameResetAction OnGameReset;

    #endregion

    #region Methods

    private void Awake()
    {
        //setting up the instance 
        instance = (instance == null) ? this : instance;
        isLoading = true;
        currentGameState = GameStates.Setup;
        //Instantiate all sub managers
        setupErroredOut = DeploySubManagers();

        if (setupErroredOut)
        {
            Debug.LogAssertion("GameManager Failed to Initialize.");
        }
        else
        {
            //call the post init event
            StartEvent("OnInitComplete");

            //call the on game load event
            OnGameLoad();
            isLoading = false;
            currentGameState = GameStates.Pre;
        }

    }

    bool DeploySubManagers()
    {
        bool erroredOut = false;
        try
        {
            //loop through all of the gamestates, and
            for (int i = 0; i < Enum.GetNames(typeof(GameSubManagerTypes)).Length; i++)
            {
                AddSubManagerSuccess((GameSubManagerTypes)i);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            erroredOut = true;
        }
        return erroredOut;
    }

    bool AddSubManagerSuccess(GameSubManagerTypes subtype)
    {
        try
        {
            //The Awake() function is used in each sub type for the init proccess.
            //Awake() will run directly after the component is added
            switch (subtype)
            {
                case GameSubManagerTypes.Player:
                    this.gameObject.AddComponent<PlayerSubManager>();

                    break;
                case GameSubManagerTypes.Spawn:
                    this.gameObject.AddComponent<SpawnSubManager>();

                    break;
                case GameSubManagerTypes.Camera:
                    this.gameObject.AddComponent<CameraSubManager>();

                    break;
                case GameSubManagerTypes.World:
                    this.gameObject.AddComponent<WorldSubManager>();

                    break;
                case GameSubManagerTypes.Ad:
                    this.gameObject.AddComponent<AdSubManager>();

                    break;
                case GameSubManagerTypes.Purchase:
                    //NOT SETUP YET

                    break;
                case GameSubManagerTypes.Social:
                    this.gameObject.AddComponent<SocialSubManager>();

                    break;
                case GameSubManagerTypes.Menu:
                    this.gameObject.AddComponent<MenuSubManager>();

                    break;
                case GameSubManagerTypes.Character:
                    this.gameObject.AddComponent<CharacterSubManager>();

                    break;
                case GameSubManagerTypes.Difficulty:
                    this.gameObject.AddComponent<DifficultySubManager>();
                    break;
                case GameSubManagerTypes.Physics:
                    this.gameObject.AddComponent<PhysicsSubManager>();
                    break;
                case GameSubManagerTypes.None:
                    //nothing needs to happen hear. this is used for catching errors in the
                    //sub manager init proccess
                    break;


                default:
                    Debug.Log("GameManager hit a default for " + subtype.ToString());
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }

        return true;
    }

    public void StartEvent(string eventName)
    {
        //Debug.Log(eventName + " is about to kick off");
        switch (eventName)
        {
            case "OnInitComplete":
                //this is just for the submanagers to finalize things  
                OnInitComplete();
                break;

            case "OnGameLoad":
                currentGameState = GameStates.Pre;
                OnGameLoad();
                break;

            case "OnGameStart":
                currentGameState = GameStates.Intra;
                OnGameStart();
                StartCoroutine(TrackTime());
                break;

            case "OnGameEnd":
                currentGameState = GameStates.Post;
                OnGameEnd();
                break;

            case "OnGameReset":
                isLoading = true;
                currentGameState = GameStates.Pre;
                OnGameReset();
                CameraSubManager.instance.SetCameraOnPlayer();
                globaltimer = 0;
                isLoading = false;
                break;
        }
    }


    public void ClearAndReloadScene()
    {
        //unsub all from the events of the game
        if (OnInitComplete != null)
        {
            foreach (var d in OnInitComplete.GetInvocationList())
            {
                OnInitComplete -= (d as InitCompleteAction);
            }
        }

        if (OnGameLoad != null)
        {
            foreach (var d in OnGameLoad.GetInvocationList())
            {
                OnGameLoad -= (d as GameLoadAction);
            }
        }

        if (OnGameStart != null)
        {
            foreach (var d in OnGameStart.GetInvocationList())
            {
                OnGameStart -= (d as GameStartAction);
            }
        }

        if (OnGameEnd != null)
        {
            foreach (var d in OnGameEnd.GetInvocationList())
            {
                OnGameEnd -= (d as GameEndAction);
            }
        }

        SceneManager.LoadScene(0);

    }

    IEnumerator TrackTime()
    {
        while(currentGameState == GameStates.Intra)
        {
            globaltimer += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #region Debug

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugMode = !debugMode;
        }

        if (debugMode)
        {
            //WILL RELOAD THE SCENE
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearAndReloadScene();
            }
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        if (debugMode)
        {
            GUILayout.Label("DEBUG MODE", style);
            if (PlayerSubManager.instance != null)
                GUILayout.Label("Invincibility_F2: " + PlayerSubManager.instance.isInvincible.ToString(), style);
        }

    }

    #endregion

}
