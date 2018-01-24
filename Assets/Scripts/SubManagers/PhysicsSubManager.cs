using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;
using SubManager.Player;
using SubManager.World.Platforms;
using SubManager.CameraMan;

namespace SubManager.Physics
{
    public class PhysicsSubManager : BaseSubManager
    {
        public static PhysicsSubManager instance;

        public List<Vector3> intersections = new List<Vector3>();
        GameObject player, Arrow;
        //each sub manager will need to override these:
        Vector3 mousePos;
        bool isGettingReady, isGrounded, isApplyingGravity, PassThrough;
        public float buildup, time;
        float angle;
        public Vector3 Velocity;
        [Range(0, 10)]
        public int iterations = 2;
        Vector3 OriginLeft, OriginRight, Direction;
        Vector3 OriginFutureLeft, OriginFutureRight;
        Vector3 OriginPastLeft, OriginPastRight;
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
            player.transform.position = WorldSubManager.instance.platforms[0].transform.position + new Vector3(0, 0.0f, 0.5f);
            isApplyingGravity = false;
            isGrounded = true;
            isGettingReady = false;
            Velocity = Vector3.zero;
            time = 0;

        }

        //runs on the game end event from the gamemanager
        //use this to end the process of the game
        public override void OnGameEnd()
        {
            isApplyingGravity = false;
            isGrounded = true;
            isGettingReady = false;
            Velocity = Vector3.zero;
            player.transform.position = WorldSubManager.instance.platforms[0].transform.position + new Vector3(0, 0.05f, 0);
        }

        //runs on the game reset event from the gamemanager
        //use this to reset the process of the game
        public override void OnGameReset()
        {
            player.transform.position = WorldSubManager.instance.platforms[0].transform.position + new Vector3(0, 0.05f, 0);
            isApplyingGravity = false;
            isGrounded = true;
            isGettingReady = false;
            Velocity = Vector3.zero;
        }

        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;

            CollisionCheck();
            Gravity();
            ApplyForce();
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

        void Update()
        {
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
                PhysicsUpdate();
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

                Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(GetAngle() * Mathf.Deg2Rad), Mathf.Sin(GetAngle() * Mathf.Deg2Rad), -1.0f));

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
                if (GameManager.instance.debugMode)
                {
                    Vector3 PlatformPosition = new Vector3(WorldSubManager.instance.platforms[PlayerSubManager.instance.currentIndex].transform.position.x,
                     WorldSubManager.instance.platforms[PlayerSubManager.instance.currentIndex].transform.position.y,
                     0.5f);
                    player.transform.position = PlatformPosition;//physicsOptions.physicsOptions.resetPosition;
                    Velocity = Vector3.zero;
                }
                else
                    Kill();
            }
        }

        void CollisionDetection()
        {

            //Gana change this to a proper search algo for the sake of speed
            for (int i = 0; i < WorldSubManager.instance.platforms.Count; i++)
            {
                Platform platform = WorldSubManager.instance.platforms[i];

                for (int j = 0; j < platform.sideColliders.Length; j++)
                {
                    Side_Collider side = platform.sideColliders[j];
                    CheckFace(side, platform);
                }
            }
        }

        void CheckFace(Side_Collider side, Platform platform)
        {
            //Face[0] is the front faces
            float dotAngle = Vector3.Dot(side.face[0].normal.normalized, Velocity.normalized);

            Vector3 intersection = new Vector3();

            if (Mathf.Sign(dotAngle) == -1)
            {

                if (isColliding(side, ref intersection) || isCollidingFrameCheck(side, ref intersection))
                {
                    Bounce(side.face[0].normal.normalized, intersection, side, platform);
                    //intersections.Add(intersection);
                }
                else
                {
                    isGrounded = false;
                }
            }
            else if (Mathf.Sign(dotAngle) == 1)
            {
                if (isColliding(side, ref intersection) || isCollidingFrameCheck(side, ref intersection))
                {
                    PassThrough = true;
                    Pass(intersection, side, platform);
                }
            }
        }

        //Juicy Mega Bool, 
        //Check #1, are they in the collider Traingle?
        //Check #2 Are they Near enough the triangle?
        //If yes too all, return true and a intersection in the XY,
        //Do this for Left and right Collision Rays
        bool isColliding(Side_Collider side, ref Vector3 intersection)
        {
            return (Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginLeft)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginLeft, OriginLeft + Direction, ref intersection) ||
                Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginRight)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginRight, OriginRight + Direction, ref intersection));
        }

        bool isCollidingFrameCheck(Side_Collider side, ref Vector3 intersection)
        {
            return (Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginLeft)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginPastLeft, OriginFutureLeft, ref intersection) ||
                Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, OriginRight)
                && Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, OriginPastRight, OriginFutureRight, ref intersection));
        }

        void Pass(Vector3 intersection, Side_Collider side, Platform platform)
        {
            if (side.GetComponent<Side>().isPassable && PassThrough)
            {
                    if (WorldSubManager.instance.GetIndex(platform) != 0)
                    {
                        WorldSubManager.instance.OnPlayerJumped();
                    }

                    PassThrough = false;
            }


        }

        void Bounce(Vector3 Normal, Vector3 intersection, Side_Collider side, Platform platform)
        {
            if (VariableManager.G_Option.killOnRed && !side.GetComponent<Side>().isPassable
            && platform.platformIndex != PlayerSubManager.instance.currentIndex)
            {
                //Kill Player if the option is checked
                Kill();
                return;
            }


            Vector3 result = Vector3.Reflect(Velocity, Normal);

            //If the reflection is greater then the rest time
            //then keep bouncing baby
            //else just chill untill the next jump
            if (result.magnitude > VariableManager.P_Options.RestTime)
            {
                Velocity = result;
                Velocity *= VariableManager.P_Options.BOUNCEDECAY;
            }
            else
            {
                ComeToRest(intersection, side, platform);
            }
        }

        void ComeToRest(Vector3 intersection, Side_Collider side, Platform platform)
        {
            player.transform.position = new Vector3(
            intersection.x,
            intersection.y,
            0.5f);

            isApplyingGravity = false;
            isGrounded = true;
            Velocity = Vector3.zero;

            if (WorldSubManager.instance.GetIndex(platform) != 0)
            {
                WorldSubManager.instance.OnPlayerJumped();
            }
        }

        void Kill()
        {
            if (!PlayerSubManager.instance.isInvincible || !GameManager.instance.debugMode)
                GameManager.instance.StartEvent("OnGameEnd");
        }
        void BuilUp()
        {
            if (buildup < VariableManager.P_Options.cap)
                buildup += VariableManager.P_Options.force;
            else
                buildup = VariableManager.P_Options.cap;
        }


        void ApplyForce()
        {
            OriginPastLeft = player.transform.position + VariableManager.P_Options.originLeft;
            OriginPastRight = player.transform.position + VariableManager.P_Options.originRight;

            player.transform.position += (new Vector3(Velocity.x, Velocity.y, 0.0f) * Time.fixedDeltaTime) * 3.0f;

            OriginFutureLeft = player.transform.position + VariableManager.P_Options.originLeft;
            OriginFutureRight = player.transform.position + VariableManager.P_Options.originRight;
        }

        void Gravity()
        {
            if (!isGrounded)
            {
                Velocity += new Vector3(0, VariableManager.P_Options.GRAVITY * Time.fixedDeltaTime, 0.0f) * VariableManager.P_Options.SCALEFACTOR;
                isApplyingGravity = true;
            }
        }

        float GetAngle()
        {

            Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.farClipPlane));
            float dx = CameraSubManager.instance.mouseScreenPosition.x - screenPos.x;
            float dy = CameraSubManager.instance.mouseScreenPosition.y - screenPos.y;

            return Mathf.Atan2(dy, -dx) * Mathf.Rad2Deg;
        }

        void ArrowRotate()
        {

            Quaternion rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, GetAngle())
            };

            Arrow.transform.rotation = rotation;
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying && VariableManager.P_Options.showDebugs)
            {
                // Gizmos.color = Color.green;
                //To mouse Direction
                //  Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(GetAngle() * Mathf.Deg2Rad), Mathf.Sin(GetAngle() * Mathf.Deg2Rad), -1.0f));
                // Gizmos.DrawRay(new Ray(player.transform.position, direction));
                // Gizmos.DrawSphere(direction + player.transform.position, .1f);

                Gizmos.color = Color.red;

                for (int i = 0; i < intersections.Count; i++)
                    Gizmos.DrawCube(intersections[i], new Vector3(0.1f, 0.1f, 0.1f));

                // Gizmos.DrawRay(CollisionRay.origin, CollisionRay.direction);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(OriginLeft, 0.05f);
                Gizmos.DrawSphere(OriginLeft + Direction, 0.05f);
                Gizmos.DrawLine(OriginLeft, OriginLeft + Direction);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(OriginRight, 0.05f);
                Gizmos.DrawSphere(OriginRight + Direction, 0.05f);
                Gizmos.DrawLine(OriginRight, OriginRight + Direction);

            }

        }
    }
}
