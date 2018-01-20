using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Spawn;
using SubManager.World;
using System;
using SubManager.CharacterMan;

namespace SubManager.Player
{
    public class PlayerSubManager : BaseSubManager
    {

        //Character Model Rendering

        public static Character player_Character;

        [HideInInspector]
        public static GameObject playerModelObject;
        public static Model playerModel;

        public EffectsPackageController playerEffectsManager;

        #region Variables
        public static PlayerSubManager instance;
        bool isPlayerAlive = true;
        [HideInInspector]
        public bool isInvincible = false;
        //Data Vars
        public short playerSpawnIndex = 1;
        public int currentIndex;
        public Vector3 offsetVec3 = new Vector3(0, 0.08f, 0);


        //TODO: Real Player.
        private GameObject player_Object;

        public GameObject Player_Object
        {
            get { return player_Object; }
            set { Debug.Log("Cannot Set the Player Object This Way."); }
        }
        //----------------------------

        #endregion

        #region Properties
        #endregion

        #region Overrides
        public override void InitializeSubManager()
        {
            instance = instance ?? (this);
            thisSubType = GameManager.GameSubManagerTypes.Player;

            //PLACEHOLDER:
            player_Object = (player_Object == null) ?
                GameObject.Find("Player_Object") : player_Object;
            if (player_Object == null)
            {
                Debug.Log("NO PLAYER!");
            }
            //--------------

        }

        public override void OnPostInit()
        {
            InitRender();

        }

        //spawn the player on the platforms
        public override void OnGameLoad()
        {
            playerEffectsManager = Player_Object.GetComponent<EffectsPackageController>();
            currentIndex = playerSpawnIndex;
            SpawnSubManager.instance.SpawnPlayer("one");
            isPlayerAlive = true;
        }

        //begin input detection
        public override void OnGameStart()
        {

            if (Player_Object != null) player_Character.Effects[0].Set(Player_Object.transform);
            //player_Character.Effects[2].Set(player_Object.transform);
        }

        //player dies, this runs after
        public override void OnGameEnd()
        {
            isPlayerAlive = false;
        }

        //starting positions everyone!
        public override void OnGameReset()
        {
            //reseting eveything
            currentIndex = playerSpawnIndex;
            SetPlayerOnPlatform(playerSpawnIndex);
            isPlayerAlive = true;

        }

        #endregion

        #region Specific Methods 

        public void InitRender()
        {

            player_Character = CharacterSubManager.instance.GetCurrentActiveCharacter();


            if (playerModelObject != null)
            {
                Destroy(playerModelObject);
            }

            //playerModelObject = new GameObject();

            playerModelObject = Instantiate(player_Character.Model.mainObject, player_Object.transform.position, Quaternion.identity);
            playerModelObject.transform.parent = player_Object.transform;

            playerModelObject.name = player_Character.name;

            playerModel.Body = (playerModelObject).transform.GetChild(0).gameObject;
            playerModel.Larm = (playerModelObject).transform.GetChild(1).gameObject;
            playerModel.Lleg = (playerModelObject).transform.GetChild(2).gameObject;
            playerModel.Rarm = (playerModelObject).transform.GetChild(3).gameObject;
            playerModel.Rleg = (playerModelObject).transform.GetChild(4).gameObject;
        }

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

                        //Call clear on effects here if needed
                        playerEffectsManager.ResetEffects();

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
            if (!isInvincible || !GameManager.instance.debugMode)
                GameManager.instance.StartEvent("OnGameEnd");
        }

        //TEMP, not linked with animation yet TODO: link
        public void SetPlayerOnPlatform(int platIndex)
        {
            try
            {
                ////sets the parent of the player to platform
                //PlayerSubManager.instance.Player_Object.transform.SetParent(
                //             WorldSubManager.instance.platforms[platIndex].transform
                //         );
//
                ////puts them in the middle of the platform they spawn on
                //PlayerSubManager.instance.Player_Object.transform.localPosition = Vector3.zero + offsetVec3;

            }
            catch (Exception ex)
            {
                Debug.LogError("SetPlayerOnPlatform(): " + ex.Message);
            }
          
        }

        #endregion

        #region Debug Commands

        //Press Up To move the player up
        //Press Down to move the player down   
        private void Update()
        {
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            {
                if (GameManager.instance.debugMode)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                    {

                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                    {
                        OnPlayerJump(false);
                    }
                    if (Input.GetKeyDown(KeyCode.F2))
                    {
                        isInvincible = !isInvincible;
                    }

                    if (playerEffectsManager.JumpAnimationTriggered)
                    {
                        if (playerEffectsManager.JumpAnimationEnded)
                        {
                            OnPlayerJump(true);
                            playerEffectsManager.JumpAnimationEnded = false;
                            playerEffectsManager.JumpAnimationTriggered = false;

                        }
                    }

                }
            }
        }

        #endregion
    }

}
