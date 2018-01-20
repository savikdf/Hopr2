#region Using Directives
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;
using SubManager.Difficulty;
using System;
#endregion

namespace SubManager.World.Platforms
{
    public class Platform : MonoBehaviour
    {
        //hodler of side objects, and their bools
        public List<Side> sides;
        public WorldSubManager.PlatformTypes thisPlatformType;
        public DifficultySubManager.PlatformDifficulties thisPlatformDifficulty;
        public float thisPlatformSpinSpeed;
        public int platformIndex;

        private void Awake()
        {
            sides = new List<Side>();

            if(thisPlatformType != WorldSubManager.PlatformTypes.Test)
            {
                thisPlatformType = WorldSubManager.PlatformTypes.Normal;    
                thisPlatformDifficulty = DifficultySubManager.instance.GetPlatformDifficulty(this);
                thisPlatformSpinSpeed = DifficultySubManager.instance.GetPlatformSpinSpeed(thisPlatformDifficulty);
            }
            try
            {
                //finds sides and sets them in the sides[]
                for (int i = 0; i < 4; i++)
                {
                    //naming starts at S_01--> S_04
                    sides.Add(transform.Find(string.Format("S_0{0}", (i + 1).ToString())).GetComponent<Side>());
                }

                //sets the side colors
                for (int i = 0; i < sides.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        //true, green
                        sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_Y;
                        sides[i].isPassable = true;

                    }
                    else if (i % 2 != 0)
                    {
                        //false, red
                        sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_N;
                        sides[i].isPassable = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Platform.Awake(): " + ex.Message);
            }

        }

        void EstablishType(WorldSubManager.PlatformTypes type)
        {
            switch (type)
            {
                case WorldSubManager.PlatformTypes.Normal:

                    break;

                case WorldSubManager.PlatformTypes.Blinker:

                    break;
                case WorldSubManager.PlatformTypes.Dud:

                    break;
                case WorldSubManager.PlatformTypes.Rich:

                    break;
                case WorldSubManager.PlatformTypes.Special:

                    break;
                case WorldSubManager.PlatformTypes.Tri:

                    break;
                case WorldSubManager.PlatformTypes.Warp:

                    break;
                case WorldSubManager.PlatformTypes.Test:
                    EstablishType(WorldSubManager.PlatformTypes.Test);
                    break;
                default:
                    EstablishType(WorldSubManager.PlatformTypes.Normal);
                    break;

            }         
        }  

        public void OnIndexSet(int index)
        {
            platformIndex = index; 
            gameObject.name = string.Format("Platform#{0}", platformIndex.ToString());
            //update the difficulty settings of the platform
            thisPlatformDifficulty = DifficultySubManager.instance.GetPlatformDifficulty(this);
            thisPlatformSpinSpeed = DifficultySubManager.instance.GetPlatformSpinSpeed(thisPlatformDifficulty);
        }

        public void OnReposition(int index)
        {
            platformIndex = index;
            gameObject.name = string.Format("Platform#{0}", platformIndex.ToString());
            //update the difficulty settings of the platform
            thisPlatformDifficulty = DifficultySubManager.instance.GetPlatformDifficulty(this);
            thisPlatformSpinSpeed = DifficultySubManager.instance.GetPlatformSpinSpeed(thisPlatformDifficulty);
            gameObject.transform.position = WorldSubManager.instance.platforms[WorldSubManager.instance.platforms.Count - 2].transform.position + new Vector3(0, WorldSubManager.instance.distanceAppart, 0);
            

        }

    }

}
