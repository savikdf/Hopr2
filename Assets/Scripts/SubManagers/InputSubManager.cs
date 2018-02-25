using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.Player;

namespace SubManager.Inputs
{
    public class InputSubManager : BaseSubManager
    {
        public bool MainUp, MainDown, MainDragging;
        public bool trackingInput, trackMouse;
        public Vector3 TouchCurrentPosition, TouchAnchorPosition, ReflectedTouchAnchorPosition, TouchAnchorTrackPosition;
        public static InputSubManager instance;
        //use this to set local data

        bool anchored;
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
        }

        //runs on the game start event from the gamemanager
        //use this to begin the process of the game
        public override void OnGameStart()
        {
            trackingInput = true;
            StartCoroutine(InputCheck());
            trackMouse = true;
            StartCoroutine(trackMousePosition());
        }

        //runs on the game end event from the gamemanager
        //use this to end the process of the game
        public override void OnGameEnd()
        {
            trackingInput = false;
            trackMouse = false;
        }

        //runs on the game reset event from the gamemanager
        //use this to reset the process of the game
        public override void OnGameReset()
        {
        }

        public Vector3 GetDirection()
        {
            return (ReflectedTouchAnchorPosition - TouchAnchorPosition).normalized;
        }

        public float GetDistance()
        {
            return Vector3.Distance(TouchAnchorPosition, ReflectedTouchAnchorPosition);
        }

        IEnumerator InputCheck()
        {
            while (trackingInput)
            {
                if (Input.GetKeyDown(VariableManager.I_Options.MainKey))
                {
                    anchored = true;
                    MainDragging = true;
                    if (anchored)
                    {
                        TouchAnchorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
                        TouchAnchorTrackPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
                        anchored = false;
                    }
                    MainDown = true;
                }
                else
                {
                    MainDown = false;
                }


                if (Input.GetKeyUp(VariableManager.I_Options.MainKey))
                {
                    MainUp = true;
                    if (MainDragging) MainDragging = false;
                }
                else
                    MainUp = false;


                if (Input.GetKey(VariableManager.I_Options.MainKey))
                {
                    //Debug.Log(Vector3.Distance(TouchAnchorPosition, TouchAnchorTrackPosition));
					TouchAnchorTrackPosition = 
                    Camera.main.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));

                    Vector3 diffrenceVector = TouchAnchorPosition - TouchAnchorTrackPosition;

                    ReflectedTouchAnchorPosition = diffrenceVector + TouchAnchorPosition;
                }

                yield return null;
            }
        }


        IEnumerator trackMousePosition()
        {
            while (trackMouse)
            {
                //Z doesnt matter for this so its set to the farplane z
                TouchCurrentPosition = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
                yield return null;
            }
        }
        
        Vector3 ScreenToView(Vector3 v)
        {
            return Camera.main.ScreenToViewportPoint(v);
        }

        void MovementCheck()
        {

        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying && VariableManager.P_Options.showDebugs)
            {
                Gizmos.DrawSphere(TouchAnchorPosition, .1f);
                Gizmos.DrawSphere(TouchAnchorTrackPosition, .1f);
                Gizmos.DrawSphere(ReflectedTouchAnchorPosition, .1f);
            }
        }
    }
}
