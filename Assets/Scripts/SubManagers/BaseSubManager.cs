using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubManager
{
    public class BaseSubManager : MonoBehaviour
    {
        //Variables-------
        [HideInInspector]
        public GameManager.GameSubManagerTypes thisSubType = GameManager.GameSubManagerTypes.None;

        //Methods-------
        public void Awake()
        {
            GameManager.OnInitComplete += OnPostInit;

            InitializeSubManager();


        }

        //each sub manager will need to override these:

        //use this to set local data
        public virtual void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.None;
            Debug.Log(thisSubType.ToString() + " is not overriding the InitializeSubManager() method.");
        }

        //runs on the post init event from the gamemanager
        //use this to start communicating with other subManagers
        public virtual void OnPostInit()
        {
            Debug.Log("Some SubManager is running a default event (OnPostInit()), needs to override!");
        }


    }



}
