using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Player;
using SubManager.World;

namespace SubManager.Spawn
{
    public class SpawnSubManager : BaseSubManager
    {
        public static SpawnSubManager instance;

        Vector3 offsetVec3 = new Vector3(0, 0.6f, 0);

        #region Overrides
        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.Spawn;



        }

        public override void OnPostInit()
        {


        }

        public override void OnGameLoad()
        {


        }

        public override void OnGameStart()
        {


        }

        public override void OnGameEnd()
        {


        }

        #endregion

        #region Methods

        public void SpawnPlayer(string playerNum)
        {
            switch (playerNum)
            {
                case "one":
                    //TODO: actually SPAWN the player, not just relocate

                    //moves the player to their spawn platform
                    PlayerSubManager.instance.Player_PH.transform.SetParent(
                        WorldSubManager.instance.platforms[PlayerSubManager.instance.playerSpawnIndex].transform
                    );

                    //puts them in the middle of the platform they spawn on
                    PlayerSubManager.instance.Player_PH.transform.localPosition = Vector3.zero + offsetVec3;


                    break;
                case "two":

                    break;
            }

        }

        #endregion

    }
}

