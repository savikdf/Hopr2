using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected enum GamesStates
    {
        Init,
        Load,
        Game,
        Post,
        Ad,
        Purchase
    }
    bool setupErroredOut;

    private void Awake()
    {
        //Instantiate all sub managers
        setupErroredOut = DeploySubManagers();

    }

    bool DeploySubManagers()
    {
        bool erroredOut = false;
        try
        {
            



        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            erroredOut = true;
        }          
        return erroredOut;
    }

}
