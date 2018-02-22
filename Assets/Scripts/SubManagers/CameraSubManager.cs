using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Player;

namespace SubManager.CameraMan
{
    public class CameraSubManager : BaseSubManager
    {
        #region Variables
        public static CameraSubManager instance;
        bool followPlayer = false;

        float damping = 20.6f;
        Camera mainCamera;
        public Vector3 offsetVec3;
        #endregion

        #region Overrides
        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.Camera;
            mainCamera = Camera.main;//GameObject.FindObjectOfType<Camera>();
            offsetVec3 = new Vector3(0, 3.6f, 10f);
        }

        public override void OnPostInit()
        {

        }

        public override void OnGameLoad()
        {
            //start the camera looking at the right place
            SetCameraOnPlayer();
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

        public override void OnGameReset()
        {
            
        }

        #endregion

        #region Specific Methods
        public void SetCameraOnPlayer()
        {
            //Debug.Log(PlayerSubManager.instance.Player_Object.transform.position.ToString());
            mainCamera.transform.position = PlayerSubManager.instance.Player_Object.transform.position + offsetVec3;
            //Debug.Log(mainCamera.transform.position.ToString());

        }

        IEnumerator FollowPlayer()
        {
            while (followPlayer)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                    PlayerSubManager.instance.Player_Object.transform.position + offsetVec3, Time.deltaTime * damping);

                yield return null;
            }

        }

        #endregion


    }
}

