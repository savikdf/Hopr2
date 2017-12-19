using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Spawn;
using SubManager.World;
using System;

namespace SubManager.Player
{
    public class PlayerSubManager : BaseSubManager
    {
        #region Variables
        public static PlayerSubManager instance;
        bool isPlayerAlive = true;
        [HideInInspector]
        public bool isInvincible = false;

        //Data Vars
        public short playerSpawnIndex = 1;  //what platform they spawn on
        public int currentIndex;
        public Vector3 offsetVec3 = new Vector3(0, 0.6f, 0);

        //TODO: Real Player.
        private GameObject player_PH;
        public GameObject Player_PH
        {
            get { return player_PH; }
            set { Debug.Log("Cannot Set the Player Object This Way."); }
        }
        //----------------------------

        #endregion

        #region Properties



        #endregion

        #region Overrides
        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.Player;


            //PLACEHOLDER:

            player_PH = (player_PH == null) ?
                GameObject.Find("Player_PH") : player_PH;
            if (player_PH == null)
            {
                Debug.Log("NO PLAYER!");
            }
            //--------------

        }

        public override void OnPostInit()
        {

        }

        //spawn the player on the platforms
        public override void OnGameLoad()
        {
            currentIndex = playerSpawnIndex;
            SpawnSubManager.instance.SpawnPlayer("one");
            isPlayerAlive = true;
        }

        //begin input detection
        public override void OnGameStart()
        {

        }

        //player dies, this runs after
        public override void OnGameEnd()
        {
            isPlayerAlive = false;
        }

        #endregion

        #region Specific Methods 

        void OnPlayerJump(bool isUp)
        {
            //determine if they player CAN jump, if yes, go for it
            //if NO, the death sequence will need to be run
            try
            {


                if (isPlayerAlive && GameManager.instance.currentGameState == GameManager.GameStates.Intra)
                {
                    if (isUp && WorldSubManager.instance.IsPlatformAboveJumpable)
                    {
                        //moves player index up
                        SetPlayerOnPlatform(currentIndex + 1);
                        currentIndex++;
                        //tell the world manager that the player has jumped
                        WorldSubManager.instance.OnPlayerJumped();
                    }
                    else if (!isUp && WorldSubManager.instance.IsPlatformBelowJumpable)
                    {
                        //moves down... wont happen in vanilla.
                        //SetPlayerOnPlatform(currentIndex - 1);
                        //currentIndex--;           
                        Debug.LogWarning("Player Cannot Move Down Yet!");
                    }
                    else
                    {
                        //player just jumped into a red, they should die now.
                        OnPlayerDeath();
                    }
                }
                //if they jump when its the main screen it will start the game, but not jump them? yah. yah that sounds good.
                else if (GameManager.instance.currentGameState == GameManager.GameStates.Pre)
                {
                    GameManager.instance.StartEvent("OnGameStart");
                }

            }
            catch (Exception ex)
            {
                //kill them. They can reach this if you jump off the last platform (aka break things)
                OnPlayerDeath();
                Debug.Log("OnPlayerJump(): " + ex.Message);
            } 
        }

        //this is called when the the player SHOULD die, maybe they have an ex macina moment that saves them? hmm...
        void OnPlayerDeath()
        {
            if(!isInvincible || !GameManager.instance.debugMode)   
                GameManager.instance.StartEvent("OnGameEnd");
        }

        //TEMP, not linked with animation yet TODO: link
        public void SetPlayerOnPlatform(int platIndex)
        {
            //sets the parent of the player to platform
            PlayerSubManager.instance.Player_PH.transform.SetParent(
                         WorldSubManager.instance.platforms[platIndex].transform
                     );

            //puts them in the middle of the platform they spawn on
            PlayerSubManager.instance.Player_PH.transform.localPosition = Vector3.zero + offsetVec3;

        }

        #endregion

        #region Debug Commands

        //Press Up To move the player up
        //Press Down to move the player down   
        private void Update()
        {
            if (GameManager.instance.debugMode)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    OnPlayerJump(true);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    OnPlayerJump(false);
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    isInvincible = !isInvincible;
                }
            }
        }

        #endregion
    }

}
