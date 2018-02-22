using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;
using SubManager.Player;
using SubManager.World.Platforms;
using SubManager.CameraMan;
using SubManager.Score;
using SubManager.Inputs;

namespace SubManager.Physics
{
    public class PhysicsSubManager : BaseSubManager
    {
        public static PhysicsSubManager instance;
        public List<Vector3> intersections = new List<Vector3>();
        public List<Platform> trackers = new List<Platform>();
        GameObject player, Arrow;
        //each sub manager will need to override these:
        Vector3 mousePos;
        bool isGettingReady, isGrounded, isApplyingGravity, Tracking;
        public float buildup, time;
        float angle;
        public Vector3 Velocity;
        [Range(0, 10)]
        public int iterations = 2;
        Vector3 OriginFrontLeft, OriginFrontRight, OriginBackLeft, OriginBackRight, Direction;
        Vector3 OriginFrontFutureLeft, OriginFrontFutureRight;
        Vector3 OriginBackFutureLeft, OriginBackFutureRight;
        Vector3 OriginFrontPastLeft, OriginFrontPastRight;
        Vector3 OriginBackPastLeft, OriginBackPastRight, directionToPlayer;
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
            player.transform.position = WorldSubManager.instance.platforms[0].transform.position + new Vector3(0, 0.05f, 0.5f);
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
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            {
                CollisionCheck();
                Gravity();
                TrackerClean();
                ApplyForce();
            }
        }

        //Check and Remove when there isnt any collisions
        //When player Passes through and above
        void TrackerClean()
        {
            if (trackers.Count > 0)
            {
                for (int i = 0; i < trackers.Count; i++)
                {
                    CleanPool(trackers[i]);
                }
            }
        }

        void CollisionCheck()
        {
            OriginBackLeft = player.transform.position + VariableManager.P_Options.originBackLeft;
            OriginBackRight = player.transform.position + VariableManager.P_Options.originBackRight;

            OriginFrontLeft = player.transform.position + VariableManager.P_Options.originFrontLeft;
            OriginFrontRight = player.transform.position + VariableManager.P_Options.originFrontRight;

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

            if (InputSubManager.instance.MainDown)
            {
                isGettingReady = true;
            }

            if (!VariableManager.G_Option.RunAway)
            {
                if (InputSubManager.instance.MainUp)
                    FireCharacter();
            }
            else
            {
                if (!InputSubManager.instance.MainDown)
                    FireCharacter();
            }

            if (isGettingReady)
            {
                if (isGrounded)
                    BuilUp();
            }

            ResetPlayer();
            ArrowRotate();
        }


        void FireCharacter()
        {
            isGettingReady = false;

            Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(GetAngle() * Mathf.Deg2Rad), Mathf.Sin(GetAngle() * Mathf.Deg2Rad), -1.0f));

            if (buildup < VariableManager.P_Options.TapRange)
            {
                if (isGrounded)
                {
                    if (VariableManager.G_Option.tapInDir)
                        Velocity += new Vector3(direction.x, direction.y, 0.0f).normalized * VariableManager.P_Options.TapRange;
                    else
                        Velocity += new Vector3(0.0f, 1.0f, 0.0f).normalized * VariableManager.P_Options.TapRange;
                }
            }
            else
            {
                if (isGrounded)
                {
                    Velocity += new Vector3(direction.x, direction.y, 0.0f).normalized * buildup;

                }
            }

            buildup = 0;
            isGrounded = false;
        }
        void ResetPlayer()
        {
            //Death/Reset
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

            //Gana Pass Through and Add to a tracker pool, turning off as soon as the player has passed
            if (isColliding(side, ref intersection) || isCollidingFrameCheck(side, ref intersection))
            {
                if (side.GetComponent<Side>().isPassable && !platform.SwitchedOff)
                    if (!trackers.Contains(platform)) trackers.Add(platform);
            }

            //Bounce Off If its Directactly Facing the Player
            if (Mathf.Sign(dotAngle) == -1)
            {

                if (isColliding(side, ref intersection) || isCollidingFrameCheck(side, ref intersection))
                {
                    Bounce(side.face[0].normal.normalized, intersection, side, platform);
                    //intersections.Add(intersection);
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
            return
             //Back
             isCollidingWithFace(side, ref intersection, OriginBackLeft, OriginBackLeft, OriginBackLeft + Direction) ||
             isCollidingWithFace(side, ref intersection, OriginBackRight, OriginBackRight, OriginBackRight + Direction) ||
             //Front
             isCollidingWithFace(side, ref intersection, OriginFrontLeft, OriginFrontLeft, OriginFrontLeft + Direction) ||
             isCollidingWithFace(side, ref intersection, OriginFrontRight, OriginFrontRight, OriginFrontRight + Direction);
        }

        bool isInterSecting(Side_Collider side, ref Vector3 intersection)
        {
            return
            //Back
            isIntersectingWithFace(side, ref intersection, OriginBackLeft, OriginBackLeft + Direction) ||
            isIntersectingWithFace(side, ref intersection, OriginBackRight, OriginBackRight + Direction) ||
            //Front
            isIntersectingWithFace(side, ref intersection, OriginFrontLeft, OriginFrontLeft + Direction) ||
            isIntersectingWithFace(side, ref intersection, OriginFrontRight, OriginFrontRight + Direction);
        }

        bool isCollidingFrameCheck(Side_Collider side, ref Vector3 intersection)
        {
            return
           //Back
           isCollidingWithFace(side, ref intersection, OriginBackLeft, OriginBackPastLeft, OriginBackFutureLeft) ||
           isCollidingWithFace(side, ref intersection, OriginBackRight, OriginBackPastRight, OriginBackFutureRight) ||
           //Front
           isCollidingWithFace(side, ref intersection, OriginFrontLeft, OriginFrontPastLeft, OriginFrontFutureLeft) ||
           isCollidingWithFace(side, ref intersection, OriginFrontRight, OriginFrontPastRight, OriginFrontFutureRight); ;
        }

        bool isIntersectingWithFace(Side_Collider side, ref Vector3 intersection, Vector3 p0, Vector3 p1)
        {
            return
                //Main
                Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, p0, p1, ref intersection) ||

                Utils.IsSegmentIntersection(side.face[3].p1,
                side.face[3].p0, p0, p1, ref intersection) ||

                Utils.IsSegmentIntersection(side.face[6].p1,
                side.face[6].p0, p0, p1, ref intersection) ||

                Utils.IsSegmentIntersection(side.face[9].p1,
                side.face[9].p0, p0, p1, ref intersection);
        }

        bool isCollidingWithFace(Side_Collider side, ref Vector3 intersection, Vector3 tri_p0, Vector3 p0, Vector3 p1)
        {
            return
                //Main
                Utils.PointInTriangle(side.face[0].p0,
                side.face[0].p1, side.face[1].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.face[0].p1,
                side.face[0].p0, p0, p1, ref intersection) ||

                //Front
                Utils.PointInTriangle(side.face[3].p0,
                side.face[3].p1, side.face[4].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.face[3].p1,
                side.face[3].p0, p0, p1, ref intersection) ||

                //Front Left
                Utils.PointInTriangle(side.face[6].p0,
                side.face[6].p1, side.face[7].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.face[6].p1,
                side.face[6].p0, p0, p1, ref intersection) ||

                //Front Right
                Utils.PointInTriangle(side.face[9].p0,
                side.face[9].p1, side.face[10].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.face[9].p1,
                side.face[9].p0, p0, p1, ref intersection);
        }

        void CleanPool(Platform platform)
        {
            if (!platform.SwitchedOff)
            {
                //Checking based the up vector for the player and the side face normal 

                //     |   <--- player up Vector, above the platform
                //
                //
                //-----|-------// <--- platform point up, below the player               

                directionToPlayer = platform.sideColliders[0].face[0].c - player.transform.position;

                float dotAngle = Vector3.Dot(platform.sideColliders[0].face[0].normal.normalized, player.transform.up);

                Vector3 intersection = new Vector3();

                bool passed = true;

                for (int i = 0; i < platform.sideColliders.Length; i++)
                {
                    if (
                    isColliding(platform.sideColliders[i], ref intersection)
                    && (Mathf.Sign(dotAngle) == 1)
                    && !AbovePlatform(platform.sideColliders[i].face[0].c)
                    && platform.sides[i].isPassable)
                    {
                        Debug.Log("Isnt Above");
                        //passed = false;
                        return;
                    }
                    if (
                   !isColliding(platform.sideColliders[i], ref intersection)
                   && (Mathf.Sign(dotAngle) == 1)
                   && !AbovePlatform(platform.sideColliders[i].face[0].c))
                    {
                        //Debug.Log("Isnt Above");
                        //passed = false;
                        return;
                    }
                }

                if (passed)
                {
                    // Debug.Log("Is Above");
                    //side.GetComponentInParent<Platform>().SwitchOff();
                    WorldSubManager.instance.OnPlayerJumped();

                    //And Clear the current side if it isnt already been removed
                    if (trackers.Contains(platform)) trackers.Remove(platform);
                    //trackers.Clear();
                }
            }
        }

        bool AbovePlatform(Vector3 c)
        {
            if ((player.transform.position.y + 0.05f) > c.y)
                return true;

            return false;
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
                isGrounded = false;
            }
            else
            {
                //Have to Make sure it intersects 
                ComeToRest(intersection, side, platform);
            }
        }

        void ComeToRest(Vector3 intersection, Side_Collider side, Platform platform)
        {
            if (
                isColliding(side, ref intersection) && AbovePlatform(side.face[0].c)
             || isCollidingFrameCheck(side, ref intersection) && AbovePlatform(side.face[0].c))
            {
                player.transform.position = new Vector3(
                player.transform.position.x,
                platform.transform.position.y,
                0.5f);

                isApplyingGravity = false;
                isGrounded = true;
                Velocity = Vector3.zero;
                trackers.Clear();
                Recycle(WorldSubManager.instance.GetIndex(platform));
                if (!platform.SwitchedOff) platform.SwitchOff();
            }
            else
                Debug.Log("Trying to Rest On A higher Platform");
        }

        void Recycle(int index)
        {
            if (index != 0)
            {
                for (int i = 0; i < index; i++)
                {
                    WorldSubManager.instance.OnPlayerJumped();
                }

                Debug.Log("Clean up time baby");
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
            OriginBackPastLeft = player.transform.position + VariableManager.P_Options.originBackLeft;
            OriginBackPastRight = player.transform.position + VariableManager.P_Options.originBackRight;

            OriginFrontPastLeft = player.transform.position + VariableManager.P_Options.originFrontLeft;
            OriginFrontPastRight = player.transform.position + VariableManager.P_Options.originFrontRight;

            player.transform.position += (new Vector3(Velocity.x, Velocity.y, 0.0f) * Time.fixedDeltaTime) * 3.0f;

            OriginBackFutureLeft = player.transform.position + VariableManager.P_Options.originBackLeft;
            OriginBackFutureRight = player.transform.position + VariableManager.P_Options.originBackRight;

            OriginFrontFutureLeft = player.transform.position + VariableManager.P_Options.originFrontLeft;
            OriginFrontFutureRight = player.transform.position + VariableManager.P_Options.originFrontRight;
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
            float dx = InputSubManager.instance.TouchCurrentPosition.x - screenPos.x;
            float dy = InputSubManager.instance.TouchCurrentPosition.y - screenPos.y;

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

                Gizmos.DrawSphere(directionToPlayer, 0.1f);
                // Gizmos.DrawRay(CollisionRay.origin, CollisionRay.direction);

                //Back
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(OriginBackLeft, 0.05f);
                Gizmos.DrawSphere(OriginBackLeft + Direction, 0.05f);
                Gizmos.DrawLine(OriginBackLeft, OriginBackLeft + Direction);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(OriginBackRight, 0.05f);
                Gizmos.DrawSphere(OriginBackRight + Direction, 0.05f);
                Gizmos.DrawLine(OriginBackRight, OriginBackRight + Direction);

                //Front
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(OriginFrontLeft, 0.05f);
                Gizmos.DrawSphere(OriginFrontLeft + Direction, 0.05f);
                Gizmos.DrawLine(OriginFrontLeft, OriginFrontLeft + Direction);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(OriginFrontRight, 0.05f);
                Gizmos.DrawSphere(OriginFrontRight + Direction, 0.05f);
                Gizmos.DrawLine(OriginFrontRight, OriginFrontRight + Direction);

            }

        }
    }
}
