using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFlowHandler : MonoBehaviour {



    #region Main

    #endregion

    #region Death
    public void Retry()
    {
        //TODO: Make this better:
        //this is very hacky, need to create proper event flow
        GameManager.instance.ClearAndReloadScene();
    }

    #endregion

    #region Char

    #endregion


}
