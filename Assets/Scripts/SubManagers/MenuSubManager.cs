using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;
using System;
using System.Linq;
using SubManager.Player;
using SubManager.CharacterMan;

namespace SubManager.Menu
{
    public class MenuSubManager : BaseSubManager
    {
        #region Variables
        public static MenuSubManager instance;
        GameObject menuHolder;
        bool isMenuQued;    //used to track the MenuQued() corroutine

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

            }
            catch (Exception ex)
            {
                Debug.Log("Menu.InitializeSubManager():  " + ex.Message);
            }
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
            //show the ingame menu
            SwitchMenu(MenuStates.Intra);
        }

        public override void OnGameEnd()
        {
            //enable the death screen
            SwitchMenu(MenuStates.Death);
        }

        public override void OnGameReset()
        {

        }

        #endregion

        #region Specific Methods

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
                Debug.LogError("SetBUttonEvents(): " + ex.Message);
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
                    if (charIndex != -1 && CharacterManager.instance != null && PlayerSubManager.instance != null)
                    {
                        //updates that character manager witht the new char index
                        CharacterManager.instance.index = (uint)charIndex;
                        PlayerSubManager.instance.InitRender();
                    }
                    break;
           
                #endregion

                #region Intra


                #endregion

                #region Death
                case "Button_Death_Retry":
                    GameManager.instance.ClearAndReloadScene();

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


