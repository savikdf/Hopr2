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

        //Data Vars
        public short playerSpawnIndex = 2;  //what platform they spawn on
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

        }

        //begin input detection
        public override void OnGameStart()
        {
                    
        }

        //player dies, this runs after
        public override void OnGameEnd()
        {
                     
        }

        #endregion

        #region Specific Methods 

        void OnPlayerJump(bool isUp)
        {
            try
            {
                if (isUp && Player_PH != null)
                {
                    //moves player index up
                    currentIndex++;
                    SetPlayerOnPlatform(currentIndex);
                }
                else if (!isUp && Player_PH != null)
                {
                    //moves down... wont happen in vanilla.
                    currentIndex--;
                    SetPlayerOnPlatform(currentIndex);
                    Debug.LogWarning("Player Cannot Move Down Yet!");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

        }

        //TEMP
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
            }
        }

        #endregion
    }

}
