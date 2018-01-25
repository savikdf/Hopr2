using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicsOptions
{
    public float GRAVITY = -9.8f;
    public float SCALEFACTOR = 0.95f;
    public float BOUNCEDECAY = 0.55f;
    [Range(0, 100)]
    public float force = 0.2f;
    public float cap = 25;
    [Range(0, 10)]
    public float TapRange = 5f;
    [Range(0, 10)]
    public float CheckMultiplier = 0.15f;
    public Vector3 originLeft = new Vector3(0, 0.2f, 0);
    public Vector3 originRight = new Vector3(0, 0.2f, 0);
    [Range(0, 5)]
    public float RestTime = 1.4f;
    public bool showDebugs;
}
[System.Serializable]
public class GameOptions
{
    public bool killOnRed = false, MuteSound = false;
}

[System.Serializable]
public class ScoreOptions
{
    ///<summary>Rate at which the score multiply per combo</summary> 
    public float ScoreMultipler = 2.5f;

    public float ComboWindow = .1f;
}

public class VariableManager : MonoBehaviour
{
    public static PhysicsOptions P_Options;
    public static GameOptions G_Option;
    public static ScoreOptions S_Option;
    public PhysicsOptions physicsoptions;
    public GameOptions gameOptions;
    public ScoreOptions scoreOptions;
    public void OnValidate()
    {
        P_Options = physicsoptions;
        G_Option = gameOptions;
        S_Option = scoreOptions;
    }
}

