﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Player;

namespace SubManager.CameraMan
{
    public class CameraSubManager : BaseSubManager
    {
        #region Variables
        bool followPlayer = false;
        float damping = 0.9f;
        Camera mainCamera;
        Vector3 offsetVec3;

        #endregion


        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.Camera;
            mainCamera = GameObject.FindObjectOfType<Camera>();
            offsetVec3 = new Vector3(0, 2.8f, -10f);
        }

        public override void OnPostInit()
        {

        }

        public override void OnGameLoad()
        {
            //start the camera looking at the right place
            mainCamera.transform.position = PlayerSubManager.instance.Player_PH.transform.position + offsetVec3;

        }

        public override void OnGameStart()
        {
            if (mainCamera)
            {
                followPlayer = true;
                StartCoroutine(FollowPlayer());
            }
        }

        public override void OnGameEnd()
        {
            followPlayer = false;
        }

        #region Specific Methods
        IEnumerator FollowPlayer()
        {
            while (followPlayer)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                    PlayerSubManager.instance.Player_PH.transform.position + offsetVec3, Time.deltaTime * damping);

                yield return null;
            }
            if (GameManager.instance.debugMode)
                Debug.Log("Camera.FollowPlayer() has ended.");
        }

        #endregion


    }
}

