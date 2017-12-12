using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region SubManager Using Directives
using SubManager;
using SubManager.Ad;
using SubManager.Camera;
using SubManager.Player;
using SubManager.Social;
using SubManager.Spawn;
using SubManager.World;

#endregion

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;
    public bool debugMode = false;
    bool setupErroredOut;


    //Types of states the game can exist in
    public enum GamesStates
    {
        Init,
        Load,
        Game,
        Post,
        Ad,
        Purchase,
    }

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
        Social
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

    //OnGameEnd event       aka.Player_Dead
    public delegate void GameEndAction();
    public static event GameEndAction OnGameEnd;

    #endregion

    #region Methods

    private void Awake()
    {
        //setting up the instance 
        instance = (instance == null) ? this : instance;
        //Instantiate all sub managers
        setupErroredOut = DeploySubManagers();

        if (setupErroredOut)
        {
            Debug.LogAssertion("GameManager Failed to Initialize.");

        }  
        else
        {
            //call the post init event
            OnInitComplete();

            //call the on game load event
            OnGameLoad();

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
                    Debug.Log("Purchases not setup yet.");

                    break;
                case GameSubManagerTypes.Social:
                    this.gameObject.AddComponent<SocialSubManager>();

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
                SceneManager.LoadScene(0);
            }
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        if (debugMode)
            GUILayout.Label("DEBUG MODE", style);
    }

    #endregion

}
