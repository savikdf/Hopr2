using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubManager
{
    public class BaseSubManager : MonoBehaviour
    {
        #region Variables

        [HideInInspector]
        public GameManager.GameSubManagerTypes thisSubType = GameManager.GameSubManagerTypes.None;

        #endregion

        #region Methods

        //Methods-------
        public void Awake()
        {
            //subscring to the gamemanger events
            GameManager.OnInitComplete += OnPostInit;
            GameManager.OnGameLoad += OnGameLoad;
            GameManager.OnGameStart += OnGameStart;
            GameManager.OnGameEnd += OnGameEnd; 
                                                      
            //init
            InitializeSubManager();             
        }

        #endregion

        #region Virtual Methods

        //each sub manager will need to override these:

        //use this to set local data
        public virtual void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.None;
            Debug.Log(thisSubType.ToString() + " is not overriding the InitializeSubManager() method.");
        }

        //runs on the post init event from the gamemanager
        //use this to start communicating with other subManagers
        public virtual void OnPostInit()
        {
            Debug.Log("Some SubManager is running a default event (OnPostInit()), needs to override!");
        }

        //runs on the game load event from the gamemanager
        //use this to begin the setup of the game
        public virtual void OnGameLoad()
        {
            Debug.Log("Some SubManager is running a default event (OnGameLoad()), needs to override!");
        }

        //runs on the game start event from the gamemanager
        //use this to begin the process of the game
        public virtual void OnGameStart()
        {
            Debug.Log("Some SubManager is running a default event (OnGameStart()), needs to override!");
        }

        //runs on the game start event from the gamemanager
        //use this to begin the process of the game
        public virtual void OnGameEnd()
        {
            Debug.Log("Some SubManager is running a default event (OnGameEnd()), needs to override!");
        }




        #endregion  

        #region Event Notes
        /*
        PLATFORMS are spawned and spun during POST_INIT --> want these spawned by the time the player is placed
        PLATFORMS are moved on the GAME_START

        PLAYER is spawned on the GAME_LOAD  --> ensures platforms exist to be placed on
        PLAYER controls activate on GAME_START




        */
        #endregion

    }   
}
