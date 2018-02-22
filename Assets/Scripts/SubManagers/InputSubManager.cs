﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubManager.Inputs
{
    public class InputSubManager : BaseSubManager
    {
        public bool MainUp, MainDown, MainDragging;
		public bool trackingInput, trackMouse;
        public Vector3 TouchCurrentPosition, TouchAnchorPosition;
        public static InputSubManager instance;
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

		IEnumerator InputCheck()
		{
			while(trackingInput)
			{
				if(Input.GetKeyDown(VariableManager.I_Option.MainKey))
					MainDown =  true;
				else
					MainDown = false;

				if(Input.GetKeyUp(VariableManager.I_Option.MainKey))
				{
					MainUp =  true;
					if(MainDragging) MainDragging = false;
				}
				else
					MainUp = false;


				yield return null;
			}
		}

		
        IEnumerator trackMousePosition()
        {
            while(trackMouse)
            {
                //Z doesnt matter for this so its set to the farplane z
                TouchCurrentPosition = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
                yield return null;
            }
        }

		void OnDrawGizmos()
		{
			if(Application.isPlaying)
			{

			}
		}
    }
}
