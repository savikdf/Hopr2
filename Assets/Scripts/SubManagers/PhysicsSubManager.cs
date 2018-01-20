﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;
using SubManager.Player;
using SubManager.World.Platforms;

public class pointsGrabber
{
    Vector3 p0, p1, intersection;

    public pointsGrabber(Vector3 _p0, Vector3 _p1, Vector3 _i)
    {
        p0 = _p0;
        p1 = _p1;
        intersection = _i;
    }

    public void DrawPoints()
    {
        Gizmos.DrawLine(intersection, p0);
        Gizmos.DrawLine(intersection, p1);
    }
}

namespace SubManager.Physics
{
    public class PhysicsSubManager : BaseSubManager
    {
        public List<pointsGrabber> pointsgrab = new List<pointsGrabber>();
        public VariableManager physicsOptions;
        public Platform_Collider[] colliderFaces;
        public GameObject player, Arrow;
        //each sub manager will need to override these:
        Vector3 mousePos;
        bool isGettingReady, isGrounded, isApplyingGravity, isColliding;
        public float buildup;
        float angle, time;
        public Vector3 Velocity;
        [Range(0, 10)]
        public int iterations = 1;
        Ray CollisionRay;
        Vector3 CollisionRayVector, FuturePosition, PastPosition, m_ScreenMosPos;

        public List<Vector3> intersections = new List<Vector3>();
        //use this to set local data
        public override void InitializeSubManager()
        {
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
            physicsOptions = GameObject.Find("GameManager").GetComponent<VariableManager>();
            player = GameObject.Find("Player_Object");
            Arrow = GameObject.Find("Arrow");
            colliderFaces = WorldSubManager.instance.platHolder.GetComponentsInChildren<Platform_Collider>();//player.transform.Find("Platform_Holder").gameObject;
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
            CollisionRay = new Ray(player.transform.position, Velocity.normalized);
            CollisionRayVector = (player.transform.position
            + physicsOptions.physicsOptions.rayCheckOffset)
            + (CollisionRay.direction * physicsOptions.physicsOptions.CheckMultiplier);

            //GroundCheck();
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

                if (buildup < physicsOptions.physicsOptions.TapRange)
                {
                    buildup = physicsOptions.physicsOptions.TapRange;
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

            if (player.transform.position.y < physicsOptions.physicsOptions.resetPoint.y)
            {
                player.transform.position = WorldSubManager.instance.platforms[PlayerSubManager.instance.currentIndex].transform.position;//physicsOptions.physicsOptions.resetPosition;

                Velocity = Vector3.zero;
            }
            ArrowRotate();
        }

        void Update()
        {
            if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
                PhysicsUpdate();
        }

        void NearestPlatform(Face face)
        {

            if ((Mathf.Sign(Vector3.Dot(Velocity.normalized, face.normal)) == -1))
            {
                Vector3 intersection = new Vector3();

                if (Utils.IsSegmentIntersection(face.p0, face.p1, player.transform.position, CollisionRayVector, ref intersection))
                {
     
                        intersections.Add(intersection);
                        Velocity = ReflectionTest(Velocity, Vector3.up, intersection, face.Object.transform.parent.GetComponent<Platform>().platformIndex) * physicsOptions.physicsOptions.BOUNCEDECAY;
                        //Debug.Log(face.Object.transform.parent.name);
                        pointsgrab.Add(new pointsGrabber(face.p0, face.p1, intersection));
                }
                else
                    PastandFutureCheck(face.p0, face.p1, Vector3.up, face.Object.transform.parent.GetComponent<Platform>().platformIndex);
            }
        }


        void PastandFutureCheck(Vector3 p0, Vector3 p1, Vector3 n, int index)
        {
            if ((Mathf.Sign(Vector3.Dot(Velocity.normalized, n.normalized)) == -1))
            {

                Vector3 intersection = new Vector3();

                if (Utils.IsSegmentIntersection(p0, p1, PastPosition, FuturePosition, ref intersection))
                {
                    //intersections.Add(intersection);
                        intersections.Add(intersection);
                        Velocity = ReflectionTest(Velocity, n.normalized, intersection, index) * physicsOptions.physicsOptions.BOUNCEDECAY;
                        pointsgrab.Add(new pointsGrabber(p0, p1, intersection));
                }
                else
                {
                }
            }
        }

        Vector3 ReflectionTest(Vector3 Velocity, Vector3 n, Vector3 intersectionPoint, int index)
        {
            Vector3 Result = Vector3.Reflect(Velocity, n) * physicsOptions.physicsOptions.BOUNCEDECAY;

            if (Result.magnitude < physicsOptions.physicsOptions.RestTime)
            {
                isGrounded = true;
                Result = Vector3.zero;
                isApplyingGravity = false;
                player.transform.position = intersectionPoint;
                PlayerSubManager.instance.currentIndex = index;
            }
            else if (Result.magnitude > physicsOptions.physicsOptions.RestTime)
            {
                isGrounded = false;
            }

            return Result;
        }

        void CollisionDetection()
        {
            for (int j = 0; j < colliderFaces.Length; j++)
            {
                for (int k = 0; k < colliderFaces[j].CollisionFaces.Length; k++)
                {
                    NearestPlatform(colliderFaces[j].CollisionFaces[k]);
                }
            }

            //  if (ToView(player.transform.position).x < 0.0f)
            //  {
            //      player.transform.position = new Vector3(toWorld(ToView(player.transform.position)).x - .1f, toWorld(ToView(player.transform.position)).y, player.transform.position.z);
            //      //Velocity.x += -Velocity.x * 0.27f;
            //      Velocity = Vector3.Reflect(Velocity, Vector3.right) * BOUNCEDECAY;
            //  }
            //  if (ToView(player.transform.position).x > 1.0f)
            //  {
            //      player.transform.position = new Vector3(toWorld(ToView(player.transform.position)).x + .1f, toWorld(ToView(player.transform.position)).y, player.transform.position.z);
            //      //Velocity.x += -Velocity.x * 0.27f;
            //      Velocity = Vector3.Reflect(Velocity, Vector3.left) * BOUNCEDECAY;
            //  }

            //GroundCheck();
        }

        void BuilUp()
        {
            if (buildup < physicsOptions.physicsOptions.cap)
                buildup += physicsOptions.physicsOptions.force;
            else
                buildup = physicsOptions.physicsOptions.cap;
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
                Velocity += new Vector3(0, physicsOptions.physicsOptions.GRAVITY * Time.fixedDeltaTime, 0.0f) * physicsOptions.physicsOptions.SCALEFACTOR;
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
            if (Application.isPlaying && physicsOptions.physicsOptions.showDebugs)
            {
                Gizmos.color = Color.green;
                for (int j = 0; j < colliderFaces.Length; j++)
                {
                    for (int k = 0; k < colliderFaces[j].CollisionFaces.Length; k++)
                    {
                        Gizmos.DrawSphere(colliderFaces[j].CollisionFaces[k].p0, 0.1f);
                        Gizmos.DrawSphere(colliderFaces[j].CollisionFaces[k].p1, 0.1f);
                    }
                }

                for (int i = 0; i < pointsgrab.Count; i++)
                {
                    pointsgrab[i].DrawPoints();
                }

                Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(getAngle() * Mathf.Deg2Rad), Mathf.Sin(getAngle() * Mathf.Deg2Rad), -1.0f));
                Gizmos.DrawSphere(direction + player.transform.position, .1f);

                Ray debugRay = new Ray(player.transform.position, direction);
                Gizmos.DrawRay(debugRay);

                // Gizmos.DrawSphere(new Vector3(-3.0f, 0, 0), 0.05f);
                // Gizmos.DrawSphere(new Vector3(3.0f, 0, 0), 0.05f);

                Gizmos.color = Color.green;
                Gizmos.DrawRay(player.transform.position, CollisionRay.direction * physicsOptions.physicsOptions.CheckMultiplier);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(CollisionRayVector, 0.06f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(player.transform.position, 0.07f);

                for (int i = 0; i < intersections.Count; i++)
                    Gizmos.DrawCube(intersections[i], new Vector3(0.1f, 0.1f, 0.1f));
            }

        }
    }
}