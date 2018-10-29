using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.World.Platforms;
using SubManager.Player;
using SubManager.World;
using SubManager.Physics;

namespace SubManager.Physics.Collision
{
    public class CollisionSubManager : BaseSubManager
    {
        public static CollisionSubManager instance;
        GameObject player;
        //each sub manager will need to override these:
        Vector3 mousePos;
        bool isGrounded, isApplyingGravity, Tracking;
        public float time;
        [Range(0, 10)]
        public int iterations = 2;
        //use this to set local data
        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
        }

        //runs on the post init event from the gamemanager
        //use this to start communicating with other subManagers
        public override void OnPostInit()
        {

        }

        //runs on the game load event from the gamemanager
        //use this to begin the setup of the game
        public override void OnGameLoad()
        {
            player = PlayerSubManager.instance.Player_Object;
        }

        //runs on the game start event from the gamemanager
        //use this to begin the process of the game
        public override void OnGameStart()
        {
        }

        //runs on the game end event from the gamemanager
        //use this to end the process of the game
        public override void OnGameEnd()
        {
        }

        //runs on the game reset event from the gamemanager
        //use this to reset the process of the game
        public override void OnGameReset()
        {
        }

        void Update()
        {
            time += Time.fixedDeltaTime;
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            {
                CollisionCheck();
                //TrackerClean();

            }
        }
        
        void CollisionCheck()
        {
            //for (int i = 0; i < iterations; i++)
            //{
            CollisionDetection();
            

            if (player.transform.position.y < WorldSubManager.instance.platforms[0].transform.position.y - 10)
            {
                //Debug.Log("Being a Willy");
                //Move this to World SubmanangerLater
                PhysicsSubManager.instance.ResetPlayer();
            }
        }


        void CollisionDetection()
        {
        }

        void CheckFace(Side_Collider side, Platform platform)
        {

        }

        float GetAngle(Vector3 vector)
        {
            Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(
                player.transform.position.x, player.transform.position.y, Camera.main.farClipPlane));

            float dx = vector.x - screenPos.x;
            float dy = vector.y - screenPos.y;

            return Mathf.Atan2(dy, -dx) * Mathf.Rad2Deg;
        }

        void OnDrawGizmos()
        {
            // && VariableManager.P_Options.showDebugs
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;

              //  for (int i = 0; i < intersections.Count; i++)
              //      Gizmos.DrawCube(intersections[i], new Vector3(0.1f, 0.1f, 0.1f));

            }

        }
    }
}
