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
        public Side_Collider[] sideColliders;
        public bool SwitchedOff;
        private void Awake()
        {
            sides = new List<Side>();

            if (thisPlatformType != WorldSubManager.PlatformTypes.Test)
            {
                thisPlatformType = WorldSubManager.PlatformTypes.Normal;
                thisPlatformDifficulty = DifficultySubManager.instance.GetPlatformDifficulty(this);
                thisPlatformSpinSpeed = DifficultySubManager.instance.GetPlatformSpinSpeed(thisPlatformDifficulty);
            }
            try
            {
                //If we forget to setup the prefabs, since it will be  #ofplats * 4
                if (sides.Count <= 0)
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

                if (sideColliders.Length <= 0)
                {
                    sideColliders = new Side_Collider[4];
                    sideColliders[0] = transform.GetChild(1).GetComponent<Side_Collider>();
                    sideColliders[1] = transform.GetChild(2).GetComponent<Side_Collider>();
                    sideColliders[2] = transform.GetChild(3).GetComponent<Side_Collider>();
                    sideColliders[3] = transform.GetChild(4).GetComponent<Side_Collider>();
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
            SwitchOn();
        }

        public void SwitchOff()
        {
            SwitchedOff = true;
            
            for (int i = 0; i < sideColliders.Length; i++)
            {
                sideColliders[i].GetComponent<MeshRenderer>().material = WorldSubManager.instance.plat_G;
                sideColliders[i].face[0].normal = new Vector3(0, 1, 0);
                sideColliders[i].face[1].normal = new Vector3(0, 1, 0);
                sideColliders[i].face[2].normal = new Vector3(0, 1, 0);
            }
        }

        public void SwitchOn()
        {
            //If we forget to setup the prefabs, since it will be  #ofplats * 4
            //sets the side colors
            SwitchedOff = false;
            for (int i = 0; i < sides.Count; i++)
            {
                if (i % 2 == 0)
                {
                    //true, green
                    sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_Y;
                    sides[i].isPassable = true;
                    sides[i].GetComponent<Side_Collider>().face[0].normal = new Vector3(0, 1, 0);

                }
                else if (i % 2 != 0)
                {
                    //false, red
                    sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_N;
                    sides[i].isPassable = false;
                    sides[i].GetComponent<Side_Collider>().face[0].normal = new Vector3(0, -1, 0);
                }
            }

        }

    }
}
