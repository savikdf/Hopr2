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
        public List<Vector3> intersections = new List<Vector3>();
        public List<Platform> trackers = new List<Platform>();
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

        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            {
                CollisionCheck();
                TrackerClean();
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
            for (int i = 0; i < iterations; i++)
            {
                CollisionDetection();
            }

            if (player.transform.position.y < WorldSubManager.instance.platforms[0].transform.position.y - 10)
            {
                Debug.Log("Being a Willy");
                //Move this to World SubmanangerLater
                PhysicsSubManager.instance.ResetPlayer();
            }
        }


        void CollisionDetection()
        {
            //Gana change this to a proper search algo for the sake of speed
            for (int i = 0; i < 1; i++)
            {
                Platform platform = WorldSubManager.instance.platforms[i];

                for (int j = 0; j < 1; j++)
                {
                    Side_Collider side = platform.sideColliders[j];
                    CheckFace(side, platform);
                }
            }
        }

        void CheckFace(Side_Collider side, Platform platform)
        {

			Vector3 direction = player.transform.position - side.transform.position;
			
            //Segment[0] is the front faces
            float dotAngle = Vector3.Dot(direction.normalized, side.segment[0].normal.normalized);
			Debug.Log(Mathf.Sign(dotAngle));
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
                    ResolveCollision(side.segment[0].normal.normalized, intersection, side, platform);
                    //intersections.Add(intersection);
                }
            }
        }


        public void ResolveCollision(Vector3 Normal, Vector3 intersection, Side_Collider side, Platform platform)
        {
            //if (VariableManager.G_Options.killOnRed && !side.GetComponent<Side>().isPassable
            //&& platform.platformIndex != PlayerSubManager.instance.currentIndex)
            //{
            //    //Kill Player if the option is checked
            //    PhysicsSubManager.instance.Kill();
            //    return;
            //}

            //If the reflection is greater then the rest time
            //then keep bouncing baby
            //else just chill untill the next jump
            if (PhysicsSubManager.instance.Reflect(Normal))
            {
                Debug.Log("Totaly Reflecting");
            }
            else
            {
                if (isColliding(side, ref intersection) || isCollidingFrameCheck(side, ref intersection))
                {
                    Debug.Log("Being Wierd");
                    //Have to Make sure it intersects 
                    trackers.Clear();
                    PhysicsSubManager.instance.Rest(intersection, side, platform);
                }
                else
                    Debug.Log("Trying to Rest On A higher Platform");
            }

        }

        //Juicy Mega Bool, 
        //Check #1, are they in the collider Traingle?
        //Check #2 Are they Near enough the triangle?
        //If yes too all, return true and a intersection in the XY,
        //Do this for Left and right Collision Rays
        bool isColliding(Side_Collider side, ref Vector3 intersection)
        {
            //return
            // //Back
            // isCollidingWithFace(side, ref intersection, OriginBackLeft, OriginBackLeft, OriginBackLeft + Direction) ||
            // isCollidingWithFace(side, ref intersection, OriginBackRight, OriginBackRight, OriginBackRight + Direction) ||
            // //Front
            // isCollidingWithFace(side, ref intersection, OriginFrontLeft, OriginFrontLeft, OriginFrontLeft + Direction) ||
            // isCollidingWithFace(side, ref intersection, OriginFrontRight, OriginFrontRight, OriginFrontRight + Direction);

            Character_Collider c = PlayerSubManager.instance.c_Collider;

            for (int i = 0; i < c.faces.Length; i++)
            {
                for (int j = 0; j < c.faces[i].segments.Length; j++)
                {
                    if (isCollidingWithFace(side, ref intersection,
                        c.faces[i].segments[j].p0,
                        c.faces[i].segments[j].p0,
                        c.faces[i].segments[j].p1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool isInterSecting(Side_Collider side, ref Vector3 intersection)
        {
            //return
            ////Back
            //isIntersectingWithFace(side, ref intersection, OriginBackLeft, OriginBackLeft + Direction) ||
            //isIntersectingWithFace(side, ref intersection, OriginBackRight, OriginBackRight + Direction) ||
            ////Front
            //isIntersectingWithFace(side, ref intersection, OriginFrontLeft, OriginFrontLeft + Direction) ||
            //isIntersectingWithFace(side, ref intersection, OriginFrontRight, OriginFrontRight + Direction);

            Character_Collider c = PlayerSubManager.instance.c_Collider;

            for (int i = 0; i < c.faces.Length; i++)
            {
                for (int j = 0; j < c.faces[i].segments.Length; j++)
                {
                    if (isIntersectingWithFace(side, ref intersection,
                        c.faces[i].segments[j].p0,
                        c.faces[i].segments[j].p1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool isCollidingFrameCheck(Side_Collider side, ref Vector3 intersection)
        {
            // Vector3 OriginBackLeft = PhysicsSubManager.instance.OriginBackLeft;
            //Vector3 OriginBackPastLeft = PhysicsSubManager.instance.OriginBackPastLeft;
            //Vector3 OriginBackFutureLeft = PhysicsSubManager.instance.OriginBackFutureLeft;
            //Vector3 OriginBackRight = PhysicsSubManager.instance.OriginBackRight;
            //Vector3 OriginBackPastRight = PhysicsSubManager.instance.OriginBackPastRight;
            //Vector3 OriginBackFutureRight = PhysicsSubManager.instance.OriginBackFutureRight;
            //
            //Vector3 OriginFrontLeft = PhysicsSubManager.instance.OriginFrontLeft;
            //Vector3 OriginFrontPastLeft = PhysicsSubManager.instance.OriginFrontPastLeft;
            //Vector3 OriginFrontFutureLeft = PhysicsSubManager.instance.OriginFrontFutureLeft;
            //Vector3 OriginFrontRight = PhysicsSubManager.instance.OriginFrontRight;
            Vector3 OriginFrontPastRight = PhysicsSubManager.instance.OriginFrontPastRight;
            Vector3 OriginFrontFutureRight = PhysicsSubManager.instance.OriginFrontFutureRight;
            //
            // return
            ////Back
            //isCollidingWithFace(side, ref intersection, OriginBackLeft, OriginBackPastLeft, OriginBackFutureLeft) ||
            //isCollidingWithFace(side, ref intersection, OriginBackRight, OriginBackPastRight, OriginBackFutureRight) ||
            ////Front
            //isCollidingWithFace(side, ref intersection, OriginFrontLeft, OriginFrontPastLeft, OriginFrontFutureLeft) ||
            //isCollidingWithFace(side, ref intersection, OriginFrontRight, OriginFrontPastRight, OriginFrontFutureRight); 

            Character_Collider c = PlayerSubManager.instance.c_Collider;

            for (int i = 0; i < c.faces.Length; i++)
            {
                for (int j = 0; j < c.faces[i].segments.Length; j++)
                {
                    if (isCollidingWithFace(side, ref intersection,
                        c.faces[i].segments[j].p0,
                        c.faces[i].segments[j].p0 + OriginFrontPastRight,
                        c.faces[i].segments[j].p1 + OriginFrontFutureRight))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool isIntersectingWithFace(Side_Collider side, ref Vector3 intersection, Vector3 p0, Vector3 p1)
        {
            return
                //Main
                Utils.IsSegmentIntersection(side.segment[0].p1,
                side.segment[0].p0, p0, p1, ref intersection) ||

                Utils.IsSegmentIntersection(side.segment[3].p1,
                side.segment[3].p0, p0, p1, ref intersection) ||

                Utils.IsSegmentIntersection(side.segment[6].p1,
                side.segment[6].p0, p0, p1, ref intersection) ||

                Utils.IsSegmentIntersection(side.segment[9].p1,
                side.segment[9].p0, p0, p1, ref intersection);
        }

        bool isCollidingWithFace(Side_Collider side, ref Vector3 intersection, Vector3 tri_p0, Vector3 p0, Vector3 p1)
        {
            return
                //Main
                Utils.PointInTriangle(side.segment[0].p0,
                side.segment[0].p1, side.segment[1].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.segment[0].p1,
                side.segment[0].p0, p0, p1, ref intersection) ||

                //Front
                Utils.PointInTriangle(side.segment[3].p0,
                side.segment[3].p1, side.segment[4].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.segment[3].p1,
                side.segment[3].p0, p0, p1, ref intersection) ||

                //Front Left
                Utils.PointInTriangle(side.segment[6].p0,
                side.segment[6].p1, side.segment[7].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.segment[6].p1,
                side.segment[6].p0, p0, p1, ref intersection) ||

                //Front Right
                Utils.PointInTriangle(side.segment[9].p0,
                side.segment[9].p1, side.segment[10].p1, tri_p0)
                &&
                Utils.IsSegmentIntersection(side.segment[9].p1,
                side.segment[9].p0, p0, p1, ref intersection);
        }

        void CleanPool(Platform platform)
        {
            if (!platform.SwitchedOff)
            {
                //Checking based the up vector for the player and the side segment normal 

                //     |   <--- player up Vector, above the platform
                //
                //
                //-----|-------// <--- platform point up, below the player               

                Vector3 direction = player.transform.position - platform.sideColliders[0].transform.position;

                float dotAngle = Vector3.Dot(direction.normalized, platform.sideColliders[0].segment[0].normal.normalized);

                Vector3 intersection = new Vector3();

                bool passed = true;

                for (int i = 0; i < platform.sideColliders.Length; i++)
                {
                    if (isColliding(platform.sideColliders[i], ref intersection)
                    && (Mathf.Sign(dotAngle) == 1)
                    && !AbovePlatform(platform.sideColliders[i].segment[0].c)
                    && platform.sides[i].isPassable)
                    {
                        Debug.Log("Isnt Above");
                        //passed = false;
                        return;
                    }
                    if (!isColliding(platform.sideColliders[i], ref intersection)
                   && (Mathf.Sign(dotAngle) == 1)
                   && !AbovePlatform(platform.sideColliders[i].segment[0].c))
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

            }

        }
    }
}
