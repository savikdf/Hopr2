using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.Ad
{
    public class AdSubManager : BaseSubManager
    {
        public static AdSubManager instance;

        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.Ad;              
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

        public override void OnGameReset()
        {

        }

    }
}

