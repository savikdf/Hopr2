using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Spawn;

namespace SubManager.Player
{
    public class PlayerSubManager : BaseSubManager
    {
        #region Variables
        public static PlayerSubManager instance;

        //Data Vars
        public short playerSpawnIndex = 2;  //what platform they spawn on
        public int currentIndex;

        //TODO: Real Player.
        private GameObject player_PH;
        public GameObject Player_PH
        {
            get { return player_PH; }
            set { Debug.Log("Cannot Set the Player Object This Way."); }
        }
        //----------------------------

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
        void PlayerJumped(bool isUp)
        {
            if (isUp)
            {
                //moves up


            }
            else
            {

                

                //moves down... wont happen in vanilla.
                Debug.LogWarning("Player Cannot Move Down Yet!");
            }

        }



        #endregion
    }

}
