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
            InitializeSubManager();
            if (GameManager.instance.debugMode)
            {
                Debug.Log(thisSubType.ToString() + " has Initialized.");
            }
        }

        //each sub manager will need to override this:
        public virtual void InitializeSubManager()
        {                                                                     
            thisSubType = GameManager.GameSubManagerTypes.None;
            Debug.Log(thisSubType.ToString() + " is not overriding the Init method.");
        } 
        



    } 
    


}
