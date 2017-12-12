﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Player;
using SubManager.World.Platforms;

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

        public override void OnGameStart()
        {
            StartCoroutine(MovePlatforms());    //moves    
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
            //spawn the platform and add it to the platform list
            //awake() in the platform will run and set itself up
            trashObject = Instantiate(prefab_platform, spawnVec3, Quaternion.identity) as GameObject;
            trashObject.name = string.Format("Platform #{0}", amountSpawned.ToString());
            platforms.Add(trashObject.GetComponent<Platform>());


            //moves the spawn postion up as they spawn
            amountSpawned++;
            spawnVec3.y = amountSpawned * distanceAppart;

        }

        void ApplyRandomSkew(Platform platToSkew)
        {
            platToSkew.transform.Rotate(new Vector3(0, Random.Range(-180f, 180f), 0));   
        }

        IEnumerator SpinPlatforms()
        {
            while (isSpinning)
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

        //DEBUG
        private void Update()
        {
            if (GameManager.instance.debugMode)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    //move player index up
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    //move player index down
                }
            }
        }

        #endregion



    }
}
