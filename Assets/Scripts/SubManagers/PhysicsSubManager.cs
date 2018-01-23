using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;
using SubManager.Player;
using SubManager.World.Platforms;

namespace SubManager.Physics
{
    public class PhysicsSubManager : BaseSubManager
    {
        public static PhysicsSubManager instance;

        public List<Vector3> intersections = new List<Vector3>();
        GameObject player, Arrow;
        //each sub manager will need to override these:
        Vector3 mousePos;
        bool isGettingReady, isGrounded, isApplyingGravity;
        public float buildup;
        float angle, time;
        public Vector3 Velocity;
        [Range(0, 10)]
        public int iterations = 2;
        Vector3 FuturePosition, PastPosition, m_ScreenMosPos;
        Vector3 OriginLeft, OriginRight, Direction;
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
            Arrow = (Arrow == null) ? GameObject.Find("Arrow") : Arrow;
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

        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;

            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            {
                CollisionCheck();
                Gravity();
                ApplyForce();
            }
        }

        void CollisionCheck()
        {
            OriginLeft = player.transform.position + VariableManager.P_Options.originLeft;
            OriginRight = player.transform.position + VariableManager.P_Options.originRight;
            Direction = Velocity.normalized * VariableManager.P_Options.CheckMultiplier;

            for (int i = 0; i < iterations; i++)
            {
                CollisionDetection();
            }
        }

        void PhysicsUpdate()
        {

            if (Input.GetMouseButtonDown(0))
            {
                //buildup = 0;
                isGettingReady = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isGettingReady = false;

                Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(getAngle() * Mathf.Deg2Rad), Mathf.Sin(getAngle() * Mathf.Deg2Rad), -1.0f));

                if (buildup < VariableManager.P_Options.TapRange)
                {
                    buildup = VariableManager.P_Options.TapRange;
                }

                if (isGrounded)
                    Velocity += new Vector3(direction.x, direction.y, 0.0f) * buildup;

                buildup = 0;
                isGrounded = false;
            }

            if (isGettingReady)
            {
                if (isGrounded)
                    BuilUp();
            }

            ResetPlayer();
            ArrowRotate();
        }

        void ResetPlayer()
        {

            if (player.transform.position.y < WorldSubManager.instance.platforms[0].transform.position.y - 10)
            {
                Vector3 PlatformPosition = new Vector3(WorldSubManager.instance.platforms[PlayerSubManager.instance.currentIndex].transform.position.x,
                 WorldSubManager.instance.platforms[PlayerSubManager.instance.currentIndex].transform.position.y,
                 0.5f);
                player.transform.position = PlatformPosition;//physicsOptions.physicsOptions.resetPosition;

                Velocity = Vector3.zero;
            }
        }

        void Update()
        {
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
                PhysicsUpdate();
        }

        void CollisionDetection()
        {
            for (int i = 0; i < WorldSubManager.instance.platforms.Count; i++)
            {
                Platform platform = WorldSubManager.instance.platforms[i];
                Platform_Collider platform_Collider = platform.GetComponent<Platform_Collider>();

                for (int j = 0; j < platform_Collider.sideColliders.Length; j++)
                {
                    Side_Collider side = platform_Collider.sideColliders[j];
                    CheckFace(side, platform);
                }
            }
        }

        void CheckFace(Side_Collider side, Platform platform)
        {

            float dotAngle = Vector3.Dot(side.face[0].normal.normalized, Velocity.normalized);

            if (Mathf.Sign(dotAngle) == -1)
            {
                Vector3 interesection = new Vector3();

                if(isColliding(side, interesection) || isCollidingFrameCheck(side, interesection))
                {
                    Debug.Log(platform.name + "on side: " + side.name);

                    player.transform.position = new Vector3(player.transform.position.x,
                    platform.transform.position.y,
                     0.5f);

                    isApplyingGravity = false;
                    isGrounded = true;
                    Velocity = Vector3.zero;
                    side.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else 
                {
                    isGrounded = false;
                }
            }
        }
        
        bool isColliding(Side_Collider side, Vector3 interesection)
        {
            return (Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginLeft)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginLeft, OriginLeft + Direction, ref interesection) ||
                Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginRight)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginRight, OriginRight + Direction, ref interesection));
        }

        bool isCollidingFrameCheck(Side_Collider side, Vector3 interesection)
        {
            return (Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginLeft)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginLeft, FuturePosition, ref interesection) ||
                Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginRight)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginRight, FuturePosition, ref interesection));
        }

        void BuilUp()
        {
            if (buildup < VariableManager.P_Options.cap)
                buildup += VariableManager.P_Options.force;
            else
                buildup = VariableManager.P_Options.cap;
        }

        void GetMousePos()
        {
            Vector3 MousePos = Input.mousePosition;
            MousePos.z = 3913;
            m_ScreenMosPos = (new Vector3(MousePos.x, MousePos.y, Camera.main.farClipPlane));
        }

        void ApplyForce()
        {
            PastPosition = player.transform.position;
            player.transform.position += (new Vector3(Velocity.x, Velocity.y, 0.0f) * Time.fixedDeltaTime) * 3.0f;
            FuturePosition = player.transform.position;
        }

        void Gravity()
        {
            if (!isGrounded)
            {
                Velocity += new Vector3(0, VariableManager.P_Options.GRAVITY * Time.fixedDeltaTime, 0.0f) * VariableManager.P_Options.SCALEFACTOR;
                isApplyingGravity = true;
            }
        }

        float getAngle()
        {
            GetMousePos();
            //Hyp Coords

            Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.farClipPlane));
            Vector3 MousePos = Input.mousePosition;
            Vector3 m_ScreenMosPos = Camera.main.ScreenToViewportPoint(new Vector3(MousePos.x, MousePos.y, Camera.main.farClipPlane));

            float dx = m_ScreenMosPos.x - screenPos.x;
            float dy = m_ScreenMosPos.y - screenPos.y;

            return Mathf.Atan2(dy, -dx) * Mathf.Rad2Deg;
        }

        void ArrowRotate()
        {

            Quaternion rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, getAngle())
            };

            Arrow.transform.rotation = rotation;
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying && VariableManager.P_Options.showDebugs)
            {
                // Gizmos.color = Color.green;
                //To mouse Direction
                //  Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(getAngle() * Mathf.Deg2Rad), Mathf.Sin(getAngle() * Mathf.Deg2Rad), -1.0f));
                // Gizmos.DrawRay(new Ray(player.transform.position, direction));
                // Gizmos.DrawSphere(direction + player.transform.position, .1f);

                Gizmos.color = Color.red;

                for (int i = 0; i < intersections.Count; i++)
                    Gizmos.DrawCube(intersections[i], new Vector3(0.1f, 0.1f, 0.1f));

                // Gizmos.DrawRay(CollisionRay.origin, CollisionRay.direction);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(OriginLeft, 0.1f);
                Gizmos.DrawSphere(OriginLeft + Direction, 0.05f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(OriginRight, 0.1f);
                Gizmos.DrawSphere(OriginRight + Direction, 0.05f);

            }

        }
    }
}
