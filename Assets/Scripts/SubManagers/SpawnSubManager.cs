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
                    PlayerSubManager.instance.SetPlayerOnPlatform(PlayerSubManager.instance.playerSpawnIndex);

                    break;
                case "two":

                    break;
            }

        }

        #endregion

    }
}

