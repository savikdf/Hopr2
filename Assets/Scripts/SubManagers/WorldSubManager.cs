using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Player;
using SubManager.World.Platforms;
using System;
using System.Linq;

namespace SubManager.World
{
    public class WorldSubManager : BaseSubManager
    {
        #region Variables
        //instance
        public static WorldSubManager instance;
        //data vars
        public Material plat_Y = null;
        public Material plat_N = null;
        public Material plat_G = null;
        public GameObject prefab_platform;
        short maxPlatformSpawnAmount = 5;
        int amountSpawned = 0;
        public float distanceAppart = 1.5f;

        //plaform vars    
        bool isSpinning;     //when is false, stops the spin coroutine
        bool isMoving;       //when is false, stops the move coroutine
        public enum PlatformTypes
        {
            Normal,
            Blinker,
            Dud,
            Special,
            Tri,
            Warp,
            Rich,
            Test
        }
        public List<Platform> platforms;
        Vector3 spawnVec3;
        GameObject trashObject;
        public GameObject platHolder;
        Platform cyclePlat;

        #endregion

        #region Properties

        Vector3 MoveSpeed
        {
            get
            {
                //TODO: get the movespeed from the difficulty sub manager
                //will be based on the amount spawned
                return new Vector3(0, 0.01f, 0) * -1;
            }
        }

        public bool IsPlatformAboveJumpable
        {
            //can they player go through the platform above them?
            get
            {
                try
                {
                    Platform plat = platforms[PlayerSubManager.instance.currentIndex + 1];
                    return plat.sides.MinObject(x => x.transform.position.z).isPassable;

                }
                catch (Exception ex)
                {
                    Debug.Log("IsPlatformAboveJumpable: " + ex.Message);
                    return false;
                }
            }
        }

        public bool IsPlatformBelowJumpable
        {
            //can the player fall through the platform they are standing on?
            get
            {
                try
                {
                    Platform plat = platforms[PlayerSubManager.instance.currentIndex];
                    return plat.sides.MinObject(x => x.transform.position.z).isPassable;
                }
                catch (Exception ex)
                {
                    Debug.Log("IsPlatformBelowJumpable: " + ex.Message);
                    return false;
                }
            }
        }

        #endregion

        #region Overrides
        //METHODS
        public override void InitializeSubManager()
        {
            //setting
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.World;

            //load the materials from resources
            plat_N = Resources.Load("Materials/Plat_N") as Material;  //red
            plat_Y = Resources.Load("Materials/Plat_Y") as Material;  //green 
            plat_G = Resources.Load("Materials/Plat_G") as Material;  //gray

            if (plat_N == null || plat_Y == null || plat_G == null)
            {
                Debug.LogError("resource material not loaded!");
            }

            //load the platform prefab 
            prefab_platform = Resources.Load("Prefabs/Platform") as GameObject;
            if (prefab_platform == null)
            {
                Debug.LogError("platform object not loaded!");
            }
        }


        public override void OnPostInit()
        {
            //Start routines
            SpawnInitialPlatforms();            //spawns
        }

        public override void OnGameLoad()
        {
            isSpinning = true;
            StartCoroutine(SpinPlatforms());    //spins 
        }

        public override void OnGameStart()
        {
            isMoving = true;
            StartCoroutine(MovePlatforms());    //moves    
        }

        public override void OnGameEnd()
        {
            //stop the move coroutine
            isMoving = false;
        }

        public override void OnGameReset()
        {
            ResetPlatforms();
            isSpinning = true;
            isMoving = false;
        }

        #endregion

        #region Specific Methods  

        void SpawnInitialPlatforms()
        {
            platHolder = new GameObject(name: "Platform_Holder");
            platforms = new List<Platform>();
            spawnVec3 = Vector3.zero;
            for (int i = 0; i < maxPlatformSpawnAmount; i++)
            {
                SpawnSingle();
                ApplyRandomSkew(platforms[i]);  //puts a random skew on the new platform
            }

            platforms[0].SwitchOff();
        }

        void SpawnSingle()
        {
            try
            {
                //spawn the platform and add it to the platform list
                //awake() in the platform will run and set itself up
                trashObject = Instantiate(prefab_platform, spawnVec3, Quaternion.identity) as GameObject;
                trashObject.transform.SetParent(platHolder.transform);
                trashObject.name = string.Format("Platform #{0}", amountSpawned.ToString());
                trashObject.GetComponent<Platform>().platformIndex = amountSpawned;
                platforms.Add(trashObject.GetComponent<Platform>());


                //moves the spawn postion up as they spawn
                amountSpawned++;
                spawnVec3.y = amountSpawned * distanceAppart;
            }
            catch (Exception ex)
            {
                Debug.Log("SpawnSingle(): " + ex.Message);
            }


        }

        void ApplyRandomSkew(Platform platToSkew)
        {
            platToSkew.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(-180f, 180f), 0));
        }

        IEnumerator SpinPlatforms()
        {
            while (isSpinning)
            {
                try
                {
                    //cycle through each platform and rotate it based on its speed value from the GetPlatformSpeed property
                    for (int i = 0; i < platforms.Count; i++)
                    {
                        if (platforms[i] != null && i != PlayerSubManager.instance.currentIndex)
                        {
                            platforms[i].transform.Rotate(
                             new Vector3(0, platforms[i].thisPlatformSpinSpeed, 0)
                             );
                        }

                        //Turn platforms normals off and On
                        if(i == PlayerSubManager.instance.currentIndex)
                        {
                            platforms[i].SwitchOff();
                        }
                        else
                        {
                            platforms[i].SwitchOn();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("SpinPlatforms(): " + ex.Message);
                }
                yield return null;
            }

        }

        IEnumerator MovePlatforms()
        {
            while (isMoving)
            {
                //cycle through each platform and rotate it based on its speed value from the GetPlatformSpeed property
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i] != null)
                    {
                        //platforms[i].transform.position += MoveSpeed;
                    }
                }
                yield return null;
            }
        }

        public void OnPlayerJumped()
        {
            //cycle the platform (bottom to top, like a modulus of sorts) 
            amountSpawned++;
            //PlayerSubManager.instance.currentIndex--;
            cyclePlat = platforms[0];
            platforms.RemoveAt(0);
            platforms.Insert(maxPlatformSpawnAmount - 1, cyclePlat);
            cyclePlat.OnReposition(amountSpawned - 1);
        }

        private void ResetPlatforms()
        {
            amountSpawned = 0;
            spawnVec3 = Vector3.zero;

            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].OnIndexSet(i);
                platforms[i].transform.position = spawnVec3;

                amountSpawned++;
                spawnVec3.y = amountSpawned * distanceAppart;
            }

        }

        #endregion 
    }
}

