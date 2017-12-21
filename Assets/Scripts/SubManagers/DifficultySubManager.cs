using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.World.Platforms;

namespace SubManager.Difficulty
{
    public class DifficultySubManager : BaseSubManager
    {
        #region Variables
        public static DifficultySubManager instance;
        public enum PlatformDifficulties
        {
            Easy,
            EasyMid,
            Mid,
            MidHard,
            Hard,
            HardGod,
            God
        }

        #endregion

        #region Properties






        #endregion

        #region Overrides
        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.Difficulty;

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

        #region Specific Methods

        //takes a platform and assigns its difficulty using its spawn number
        public PlatformDifficulties GetPlatformDifficulty(Platform plat)
        {
            int i = plat.platformIndex;
            //less than 10  :easy
            if (i < 10)
            {
                return PlatformDifficulties.Easy;
            }
            //11 - 20   :easy-mid
            if (i > 10 && i <= 20)
            {
                return PlatformDifficulties.EasyMid;
            }
            //21 - 30   :mid
            if (i > 20 && i <= 30)
            {
                return PlatformDifficulties.Mid;
            }
            //31 - 40   :mid-hard
            if (i > 30 && i <= 40)
            {
                return PlatformDifficulties.MidHard;
            }
            //41 - 50   :hard
            if (i > 40 && i <= 50)
            {
                return PlatformDifficulties.Hard;
            }
            //51 - 60   :hard-god
            if (i > 50 && i <= 60)
            {
                return PlatformDifficulties.HardGod;
            }
            //greater than 60   :god
            if (i > 60)
            {
                return PlatformDifficulties.God;
            }

            //default:
            return PlatformDifficulties.Easy;
        }

        //sets the spin speed based on the difficutly
        public float GetPlatformSpinSpeed(PlatformDifficulties dif)
        {
            switch (dif)
            {
                case PlatformDifficulties.Easy:
                    return 1.2f;

                case PlatformDifficulties.EasyMid:
                    return 1.4f;

                case PlatformDifficulties.Mid:
                    return 1.6f;

                case PlatformDifficulties.MidHard:
                    return 1.8f;

                case PlatformDifficulties.Hard:
                    return 2f;

                case PlatformDifficulties.HardGod:
                    return 2.2f;

                case PlatformDifficulties.God:
                    return 2.4f;

                default:
                    return 1.2f;
            }
        }  

        #endregion


    }
}

