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
        public GameObject prefab_platform;
        short maxPlatformSpawnAmount = 30;
        float distanceAppart = 1.1f;
        int amountSpawned = 0;
        Vector3 moveSpeed = new Vector3(0, -0.1f, 0);

        //plaform vars    
        bool isSpinning = true;     //when is false, stops the spin coroutine
        bool isMoving = true;       //when is false, stops the move coroutine
        public enum PlatformTypes
        {
            Normal
        }
        public List<Platform> platforms;
        Vector3 spawnVec3;
        GameObject trashObject;

        #endregion

        #region Properties
        //PROPERTIES
        float GetPlatformSpeed(PlatformTypes type)
        {
            switch (type)
            {
                case PlatformTypes.Normal:
                    return 0.6f;
            }
            return 100f;
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
            if (plat_N == null || plat_Y == null)
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
            StartCoroutine(SpinPlatforms());    //spins
        }

        public override void OnGameLoad()
        {

        }

        public override void OnGameStart()
        {
            StartCoroutine(MovePlatforms());    //moves    
        }

        public override void OnGameEnd()
        {

        }

        #endregion

        #region Specific Methods  

        void SpawnInitialPlatforms()
        {
            platforms = new List<Platform>();
            spawnVec3 = Vector3.zero;
            for (int i = 0; i < maxPlatformSpawnAmount; i++)
            {
                SpawnSingle();
                ApplyRandomSkew(platforms[i]);
            }
        }

        void SpawnSingle()
        {
            try
            {
                //spawn the platform and add it to the platform list
                //awake() in the platform will run and set itself up
                trashObject = Instantiate(prefab_platform, spawnVec3, Quaternion.identity) as GameObject;
                trashObject.name = string.Format("Platform #{0}", amountSpawned.ToString());
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
                        if (platforms[i] != null)
                        {
                            platforms[i].transform.Rotate(
                             new Vector3(0, GetPlatformSpeed(platforms[i].thisPlatformType), 0)
                             );
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
            while (isSpinning)
            {
                //cycle through each platform and rotate it based on its speed value from the GetPlatformSpeed property
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i] != null)
                    {
                        platforms[i].transform.position += moveSpeed;
                    }
                }
                yield return null;
            }
        }

        //DEBUG COMMANDS
        private void Update()
        {
            if (GameManager.instance.debugMode)
            {


            }
        }

        #endregion



    }
}

