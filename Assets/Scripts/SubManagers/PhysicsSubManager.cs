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
        public Vector3 OriginFrontLeft, OriginFrontRight, OriginBackLeft, OriginBackRight, Direction;
        public Vector3 OriginFrontFutureLeft, OriginFrontFutureRight;
        public Vector3 OriginBackFutureLeft, OriginBackFutureRight;
        public Vector3 OriginFrontPastLeft, OriginFrontPastRight;
        public Vector3 OriginBackPastLeft, OriginBackPastRight, directionToPlayer;
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
                OriginBackLeft = player.transform.position + VariableManager.P_Options.originBackLeft;
                OriginBackRight = player.transform.position + VariableManager.P_Options.originBackRight;

                OriginFrontLeft = player.transform.position + VariableManager.P_Options.originFrontLeft;
                OriginFrontRight = player.transform.position + VariableManager.P_Options.originFrontRight;

                Direction = Velocity.normalized * VariableManager.P_Options.CheckMultiplier;


                PhysicsUpdate();
                Gravity();
                ApplyForce();
            }
        }

        void PhysicsUpdate()
        {

            if (InputSubManager.instance.MainDown)
            {
                isGettingReady = true;
            }

            if (!VariableManager.G_Options.RunAway)
            {
                if (InputSubManager.instance.MainUp)
                {

                    FireCharacter();
                }

            }
            else
            {
                if (!InputSubManager.instance.MainDown)
                    FireCharacter();
            }

            if (InputSubManager.instance.MainDragging)
            {
                Vector3 vector = Camera.main.ViewportToScreenPoint(InputSubManager.instance.GetDirection());
                if (InputSubManager.instance.GetDistance() > .1f)
                    ArrowRotate(new Vector3(-vector.x, vector.y, vector.z));
            }
            else
                ArrowRotate(InputSubManager.instance.TouchCurrentPosition);

            if (isGettingReady)
            {
                if (isGrounded)
                    BuilUp();
            }
        }


        void FireCharacter()
        {
            isGettingReady = false;

            Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(GetAngle(InputSubManager.instance.TouchCurrentPosition)
             * Mathf.Deg2Rad), Mathf.Sin(GetAngle(InputSubManager.instance.TouchCurrentPosition) * Mathf.Deg2Rad), -1.0f));

            if (buildup < VariableManager.P_Options.TapRange)
            {
                if (isGrounded)
                {
                    if (VariableManager.G_Options.tapInDir)
                        Velocity += new Vector3(direction.x, direction.y, 0.0f).normalized * VariableManager.P_Options.TapRange;
                    else
                        Velocity += new Vector3(0.0f, 1.0f, 0.0f).normalized * VariableManager.P_Options.TapRange;
                }
            }
            else
            {
                if (isGrounded)
                {
                    Vector3 inputDirecetion = InputSubManager.instance.GetDirection();
                    Velocity += new Vector3(inputDirecetion.x, inputDirecetion.y, 0.0f).normalized * buildup;
                }
            }

            buildup = 0;
            isGrounded = false;
        }
        public void ResetPlayer()
        {
            //Death/Reset

            if (GameManager.instance.debugMode)
            {
                Vector3 PlatformPosition = new Vector3(
                    WorldSubManager.instance.platforms[0].transform.position.x,
                    WorldSubManager.instance.platforms[0].transform.position.y + 0.05f,
                    0.5f);
                player.transform.position = PlatformPosition;//physicsOptions.physicsOptions.resetPosition;
                Velocity = Vector3.zero;
            }
            else
                Kill();
        }


        public void Rest(Vector3 intersection, Side_Collider side, Platform platform)
        {
            //need to fix way repositions, taking into account the intersection and the platform
            //else it only pushes a little past halfway
            player.transform.position = new Vector3(
            player.transform.position.x,
            platform.transform.position.y + .05f,
            0.5f);

            isApplyingGravity = false;
            isGrounded = true;
            Velocity = Vector3.zero;

            Recycle(WorldSubManager.instance.GetIndex(platform));
            if (!platform.SwitchedOff) platform.SwitchOff();

        }

        public bool Reflect(Vector3 Normal)
        {
            Vector3 result = Vector3.Reflect(Velocity, Normal);

            if (result.magnitude > VariableManager.P_Options.RestTime)
            {
                Velocity = result;
                Velocity *= VariableManager.P_Options.BOUNCEDECAY;
                isGrounded = false;

                return true;
            }

            return false;
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

        public void Kill()
        {
            if (!PlayerSubManager.instance.isInvincible || !GameManager.instance.debugMode)
                GameManager.instance.StartEvent("OnGameEnd");
        }
        void BuilUp()
        {
            if (buildup < VariableManager.P_Options.cap)
            {
                buildup = (float)InputSubManager.instance.GetDistance() * VariableManager.P_Options.force;
                //buildup += VariableManager.P_Options.force;
            }
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

        float GetAngle(Vector3 vector)
        {
            Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(
                player.transform.position.x, player.transform.position.y, Camera.main.farClipPlane));

            float dx = vector.x - screenPos.x;
            float dy = vector.y - screenPos.y;

            return Mathf.Atan2(dy, -dx) * Mathf.Rad2Deg;
        }

        void ArrowRotate(Vector3 vector)
        {

            Quaternion rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, GetAngle(vector))
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
