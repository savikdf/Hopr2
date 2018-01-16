using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;

namespace SubManager.Physics
{

    public class PhysicsSubManager : BaseSubManager
    {
        public static float GRAVITY = -9.8f;
        public static float SCALEFACTOR = 0.95f;
        public static float BOUNCEDECAY = 0.55f;
        public Platform_Collider[] colliderFaces;
        public GameObject player;
        //each sub manager will need to override these:
        Vector2 mousePos;

        [Range(0, 100)]
        public float force = 0.2f;
        public bool isGettingReady, isGrounded, isApplyingGravity, Impulse;
        public float buildup;
        public float cap = 25;
        public float angle;
        public float time;
        public Vector2 Velocity;
        public bool isColliding;

        [Range(0, 10)]
        public float TapRange = 5f;
        public GameObject Arrow;
        public Vector3 m_ScreenMosPos;

        [Range(0, 10)]
        public int iterations = 2;


        [Range(0, 10)]
        public float CheckMultiplier = 0.15f;

        public Vector3 rayCheckOffset = new Vector3(0, 0.2f, 0);

        Ray CollisionRay;
        Vector3 CollisionRayVector, FuturePosition, PastPosition;

        [Range(0, 5)]
        public float RestTime = 1.4f;

        [Range(0, 1)]
        public float CollisionDistance = .25f;

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

            if(GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            CollisionCheck();
        }

        void CollisionCheck()
        {
            CollisionRay = new Ray(player.transform.position, Velocity.normalized);
            CollisionRayVector = (player.transform.position + rayCheckOffset) + CollisionRay.direction * CheckMultiplier;
            //GroundCheck();
            for (int i = 0; i < iterations; i++)
            {
                CollisionDetection();
            }
            ApplyForce();
        }

        void PhysicsUpdate()
        {
            if (Velocity.magnitude < 0.1f)
                //isGrounded = true;

                if (Input.GetMouseButtonDown(0))
                {
                    //buildup = 0;

                    isGettingReady = true;
                }

            if (Input.GetMouseButtonUp(0))
            {
                isGettingReady = false;
                getAngle();
                Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -1.0f));

                if (buildup < TapRange)
                {
                    buildup = TapRange;
                }

                if (isGrounded)
                    Velocity += new Vector2(direction.x, direction.y) * buildup;

                buildup = 0;
                isGrounded = false;
            }

            if (isGettingReady)
            {
                if (isGrounded)
                    BuilUp();
            }

            Gravity();
            ArrowRotate();
        }
        void Update()
        {
            if(GameManager.instance.currentGameState == GameManager.GameStates.Intra)
            PhysicsUpdate();
        }

        void NearestPlatform(Face face)
        {
            float dotAngle = Vector2.Dot(Velocity.normalized, Vector3.up);

            Vector3 intersectionPoint = Utils.SegmentIntersection(face.p0, face.p1, CollisionRayVector, player.transform.position, false);

            if ((Mathf.Sign(dotAngle) == -1))
            {
                if (Utils.IsSegmentIntersection(face.p0, face.p1, CollisionRayVector, player.transform.position))
                {
                    Velocity = ReflectionTest(Velocity, face.normal, intersectionPoint, dotAngle, face.isDynamic) * BOUNCEDECAY;
                }
            }

            PastandFutureCheck(face.p0, face.p1, face.isDynamic, Vector3.up);
        }

        void GroundCheck()
        {

            float dotAngle = Vector2.Dot(Velocity.normalized, Vector3.up);

            Vector3 leftSide = new Vector3(-3.0f, 0, 0);
            Vector3 rightSide = new Vector3(3.0f, 0, 0);

            Vector3 intersectionPoint = Utils.SegmentIntersection(leftSide, rightSide, CollisionRayVector, player.transform.position, false);

            if ((Mathf.Sign(dotAngle) == -1))
            {

                if (Utils.IsSegmentIntersection(leftSide, rightSide, CollisionRayVector, player.transform.position))
                {
                    Velocity = ReflectionTest(Velocity, Vector2.up, intersectionPoint, dotAngle, false) * BOUNCEDECAY;

                }
            }

            PastandFutureCheck(leftSide, rightSide, false, Vector3.up);
        }

        void PastandFutureCheck(Vector3 p0, Vector3 p1, bool isDynamic, Vector3 n)
        {
            float dotAngle = Vector2.Dot(Velocity.normalized, n);

            Vector3 intersectionPoint = Utils.SegmentIntersection(p0, p1, PastPosition, FuturePosition, false);

            if ((Mathf.Sign(dotAngle) == -1))
            {
                if (Utils.IsSegmentIntersection(p0, p1, PastPosition, FuturePosition))
                {
                    Velocity = ReflectionTest(Velocity, n, intersectionPoint, dotAngle, isDynamic) * BOUNCEDECAY;
                }
            }
        }

        Vector2 ReflectionTest(Vector2 Velocity, Vector3 direction, Vector3 intersectionPoint, float angle, bool isDynamic)
        {
            Vector2 Result = Vector2.Reflect(Velocity, direction);
            //
            angle = Mathf.Abs(angle);

            if (Result.magnitude < RestTime && !isDynamic)
            {
                isGrounded = true;
                Result = Vector2.zero;
                isApplyingGravity = false;
                player.transform.position = intersectionPoint;
                //Debug.Log("stuck");
            }
            else if (Result.magnitude > RestTime)
            {
                isGrounded = false;
            }

            return Result;
        }

        bool LineIntresection()
        {
            return true;
        }

        void CollisionDetection()
        {
            for (int j = 0; j < colliderFaces.Length; j++)
            {
                for (int k = 0; k < colliderFaces[j].CollisionFaces.Length; k++)
                    NearestPlatform(colliderFaces[j].CollisionFaces[k]);
            }

          //  if (ToView(player.transform.position).x < 0.0f)
          //  {
          //      player.transform.position = new Vector3(toWorld(ToView(player.transform.position)).x - .1f, toWorld(ToView(player.transform.position)).y, player.transform.position.z);
          //      //Velocity.x += -Velocity.x * 0.27f;
          //      Velocity = Vector2.Reflect(Velocity, Vector2.right) * BOUNCEDECAY;
          //  }
          //  if (ToView(player.transform.position).x > 1.0f)
          //  {
          //      player.transform.position = new Vector3(toWorld(ToView(player.transform.position)).x + .1f, toWorld(ToView(player.transform.position)).y, player.transform.position.z);
          //      //Velocity.x += -Velocity.x * 0.27f;
          //      Velocity = Vector2.Reflect(Velocity, Vector2.left) * BOUNCEDECAY;
          //  }
        //GroundCheck();
        }

        void BuilUp()
        {
            if (buildup < cap)
                buildup += force;
            else
                buildup = cap;
        }


        void GetMousePos()
        {
            Vector3 MousePos = Input.mousePosition;
            MousePos.z = 3913;
            m_ScreenMosPos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Camera.main.farClipPlane));
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
                Velocity += new Vector2(0, GRAVITY * Time.fixedDeltaTime) * SCALEFACTOR;
                isApplyingGravity = true;
            }
        }

        void getAngle()
        {
            GetMousePos();
            //Hyp Coords

            Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.farClipPlane));

            float dx = m_ScreenMosPos.x + screenPos.x;
            float dy = m_ScreenMosPos.y + screenPos.x;

            angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        }

        void ArrowRotate()
        {
            getAngle();

            Quaternion rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, angle)
            };
            Arrow.transform.rotation = rotation;
        }


        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {

                Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -1.0f));
                Gizmos.DrawSphere(direction + player.transform.position, .05f);

                Ray debugRay = new Ray(player.transform.position, direction);
                Gizmos.DrawRay(debugRay);

                Gizmos.DrawSphere(new Vector3(-3.0f, 0, 0), 0.05f);
                Gizmos.DrawSphere(new Vector3(3.0f, 0, 0), 0.05f);

                Gizmos.color = Color.green;
                Gizmos.DrawRay(player.transform.position, CollisionRay.direction * CheckMultiplier);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(CollisionRayVector, 0.06f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(player.transform.position, 0.07f);
            }

        }
    }
}
