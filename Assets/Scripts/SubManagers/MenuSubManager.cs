﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;
using System;
using System.Linq;
using SubManager.Player;
using SubManager.CharacterMan;
using SubManager.Inputs;


namespace SubManager.Menu
{
    public class MenuSubManager : BaseSubManager
    {
        #region Variables
        public static MenuSubManager instance;
        GameObject menuHolder;
        public GameObject ScoreObject, MultiObject, AltitudeObject, ChargeObject, AnchorObject, TracerObject;
        bool isMenuQued, trackingPlayer, trackingMouse, handleObjects;    //used to track the MenuQued() corroutine

        public enum MenuStates
        {
            Loading,
            Main,
            Intra,
            Death,
            Character,
            Setting
        }
        public MenuStates currentMenuState;

        List<Canvas> menus;

        #endregion

        #region Properties

        int CurrentMenuIndex
        {
            get { return (int)currentMenuState; }
        }

        #endregion

        #region Overrides

        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            thisSubType = GameManager.GameSubManagerTypes.Menu;

            menus = new List<Canvas>();
            try
            {
                menuHolder = Instantiate(Resources.Load("Prefabs/UI/Menus") as GameObject, Vector3.zero, Quaternion.identity);
                if (menuHolder)
                {
                    //cycle through each type of menu
                    for (int i = 0; i < System.Enum.GetNames(typeof(MenuStates)).Length; i++)
                    {
                        //find its coresponding canvas in the menuHolder object
                        //if the canvas doesn't exist, it wont be processed- no bothers
                        if (menuHolder.transform.Find(string.Format("Menu_{0}", (MenuStates)i)) != null)
                            menus.Add(menuHolder.transform.Find(string.Format("Menu_{0}", (MenuStates)i)).GetComponent<Canvas>());
                        //naming convention is type_Menu
                    }
                    if (menus != null && menus.Count > 0)
                    {
                        //sets up all the button events for each canvas
                        for (int i = 0; i < menus.Count; i++)
                        {
                            SetButtonEvents(menus[i]);

                        }
                    }
                    //Turn to the loading screen
                    SwitchMenu(MenuStates.Main);
                }
                else
                {
                    Debug.LogError("Menu Not Loaded!");
                }

                //Display Score 
                ScoreObject = menus[2].transform.Find("Score_Object").gameObject;
                //Display Number of Combo Multiplies
                MultiObject = menus[2].transform.Find("Multiplier_Object").gameObject;
                AltitudeObject = menus[2].transform.Find("Altitude_Object").gameObject;
                ChargeObject = menus[2].transform.Find("Charge_Object").gameObject;
                AnchorObject = menus[2].transform.Find("Anchor_Object").gameObject;
                TracerObject = menus[2].transform.Find("Tracer_Object").gameObject;
            }
            catch (Exception ex)
            {
                Debug.Log("Menu.InitializeSubManager():  " + ex.Message);
            }
        }


        float GetAngle(Vector3 vector)
        {
            Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(
                PlayerSubManager.instance.Player_Object.transform.position.x, 
                PlayerSubManager.instance.Player_Object.transform.position.y, 
                Camera.main.farClipPlane));

            float dx = vector.x - screenPos.x;
            float dy = vector.y - screenPos.y;

            return Mathf.Atan2(dy, -dx) * Mathf.Rad2Deg;
        }


        void ZAxisRotate(Vector3 vector, GameObject obj)
        {

            Quaternion rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, GetAngle(vector))
            };

            obj.transform.rotation = rotation;
        }


        public override void OnPostInit()
        {

        }

        public override void OnGameLoad()
        {
            //show the main menu, the place where everything converges
            SwitchMenu(MenuStates.Main);
        }

        public override void OnGameStart()
        {
            trackingPlayer = true;
            StartCoroutine(TrackPlayer());
            trackingMouse = true;
            StartCoroutine(TrackMouse());
            handleObjects = true;
            StartCoroutine(HandleMenuObjects());
            //show the ingame menu
            SwitchMenu(MenuStates.Intra);
        }

        public override void OnGameEnd()
        {
            trackingPlayer = false;
            trackingMouse = false;
            handleObjects = false;

            //enable the death screen
            SwitchMenu(MenuStates.Death);
        }

        public override void OnGameReset()
        {
            SwitchMenu(MenuStates.Main);
        }

        #endregion

        #region Specific Methods


        IEnumerator TrackPlayer()
        {
            while (trackingPlayer)
            {
                MultiObject.transform.position = Utils.WorldToScreen(PlayerSubManager.instance.Player_Object.transform.position
                + VariableManager.M_Options.MultiplierPositionOffset);

                ChargeObject.transform.position = Utils.WorldToScreen(PlayerSubManager.instance.Player_Object.transform.position
                + VariableManager.M_Options.ChargePositionOffset);

                yield return null;
            }
        }

        IEnumerator HandleMenuObjects()
        {
            while(handleObjects)
            {
                Vector3 vector = Camera.main.ViewportToScreenPoint (InputSubManager.instance.GetDirectionToTracker());
                ZAxisRotate(vector, ChargeObject);
                yield return null;
            }
        }
        IEnumerator TrackMouse()
        {
            while (trackingMouse)
            {
                if (InputSubManager.instance.MainDragging)
                {
                    AnchorObject.SetActive(true);
                    TracerObject.SetActive(true);

                    AnchorObject.transform.position = Utils.WorldToScreen(InputSubManager.instance.TouchAnchorPosition);
                    TracerObject.transform.position = Utils.WorldToScreen(InputSubManager.instance.TouchAnchorTrackPosition);
                }
                else
                {
                    AnchorObject.SetActive(false);
                    TracerObject.SetActive(false);

                }
                yield return null;
            }
        }
        public void SetButtonEvents(Canvas setCanvas)
        {
            try
            {
                Button[] buttons = setCanvas.GetComponentsInChildren<Button>();
                if (buttons != null && buttons.Length != 0)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].gameObject.AddComponent<DynamicButton>();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("SetButtonEvents(): " + ex.Message);
            }

        }

        public void OnButtonPress(string name)
        {
            //if (GameManager.instance.debugMode)
            //    Debug.Log("Button pressed: " + name);

            //change character buttons will hit this
            //Button_Character_Change_#. # will be the index that the character should update to
            int charIndex = -1;
            if (name.Contains("Character_Change_"))
            {
                charIndex = Int32.TryParse(name[name.Length - 1].ToString(), out charIndex) ? charIndex : -1;
                name = "Button_Character_Change";
            }

            switch (name)
            {
                #region Main
                case "Button_Main_Play":
                    GameManager.instance.StartEvent("OnGameStart");
                    break;
                case "Button_Main_Character":
                    SwitchMenu(MenuSubManager.MenuStates.Character);
                    break;

                #endregion

                #region Character

                case "Button_Character_Back":
                    SwitchMenu(MenuSubManager.MenuStates.Main);
                    break;
                case "Button_Character_Change":
                    if (charIndex != -1 && CharacterSubManager.instance != null && PlayerSubManager.instance != null)
                    {
                        //updates that character manager witht the new char index
                        CharacterSubManager.instance.index = (uint)charIndex;
                        PlayerSubManager.instance.InitRender();
                    }
                    break;

                #endregion

                #region Intra


                #endregion

                #region Death
                case "Button_Death_Retry":
                    GameManager.instance.StartEvent("OnGameReset");

                    break;

                    #endregion

            }

        }

        public void SwitchMenu(MenuStates toState)
        {
            //will always switch to loading menu if the Gamemanager is still loading
            //and then que the next menu transition while loading isn't complete
            if (GameManager.instance.isLoading && toState != MenuStates.Loading && !isMenuQued)
            {
                //if (GameManager.instance.debugMode)
                //    Debug.Log("Queing Menu of type: " + toState.ToString());
                isMenuQued = true;
                //override set the loading screen while other qued
                StartCoroutine(MenuQued(toState));
                toState = MenuStates.Loading;
            }
            //disables all the menus first, before turning on the specific one
            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].enabled = false;
            }
            menus.Single(x => x.name == string.Format("Menu_{0}", toState.ToString())).enabled = true;
        }

        //ques a menu if the gamemanager isn't done loading
        IEnumerator MenuQued(MenuStates queState)
        {
            while (GameManager.instance.isLoading)
            {
                yield return null;
            }

            //if (GameManager.instance.debugMode)
            //    Debug.Log("Un-Queing Menu of type: " + queState.ToString());

            isMenuQued = false;
            SwitchMenu(queState);
        }

        #endregion

        #region Debug Commands





        #endregion



    }
}


