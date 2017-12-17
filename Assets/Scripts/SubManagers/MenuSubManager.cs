using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;
using System;

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
            None,
            Loading,
            Main,
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
            menus = new List<Canvas>();
            try
            {
                menuHolder = Instantiate(Resources.Load("Prefabs/Menus") as GameObject, Vector3.zero, Quaternion.identity);
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

        }

        public override void OnGameStart()
        {

        }

        public override void OnGameEnd()
        {
                   
        }

        #endregion

        #region Specific Methods

        public void SwitchMenu(MenuStates toState)
        {
            //will always switch to loading menu if the Gamemanager is still loading
            //and then que the next menu transition while loading isn't complete
            if (GameManager.instance.isLoading && toState != MenuStates.Loading && !isMenuQued)
            {
                isMenuQued = true;
                StartCoroutine(MenuQued(toState));
                toState = MenuStates.Loading;
            }
            //disables all the menus first, before turning on the specific one
            for(int i = 0; i < menus.Count; i++)
            {
                if (i == (int)toState)   
                {
                    //this is the menu being switch to
                    menus[i].enabled = true;
                    continue;
                }
                menus[i].enabled = false; 
            }              
        }

        //ques a menu if the gamemanager isn't done loading
        IEnumerator MenuQued(MenuStates queState)
        {
            while (GameManager.instance.isLoading)
            {     
                yield return null;
            }
            isMenuQued = false;
            SwitchMenu(queState);
        }                        
        
        #endregion

        #region Debug Commands





        #endregion



    }
}


