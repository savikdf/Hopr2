using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.World.Platforms;

namespace SubManager.World
{
    public class WorldSubManager : BaseSubManager
    {
        public static WorldSubManager instance;

        #region Variables
        //data vars
        public Material plat_Y = null;
        public Material plat_N = null;
        public GameObject prefab_platform;


        //plaform vars
        short maxPlatformSpawnAmount = 30;
        float distanceAppart = 1.1f;
        int amountSpawned = 0;
        public enum PlatformTypes
        {
            Normal
        }


        List<Platform> platforms;
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
            SpawnInitialPlatforms();

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


        ////DEBUG METHODS
        //private void Update()
        //{
        //    if (GameManager.instance.debugMode)
        //    {
        //        if (Input.GetKeyDown(KeyCode.Space))
        //        {
        //            SpawnPlatforms();
        //        }
        //    }           
        //}
        #endregion

    }
}

