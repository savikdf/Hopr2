using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Spawn;
using SubManager.World;
using System;
using SubManager.CharacterMan;

public class EffectsPackadgeManager : BaseSubManager
{
    //Effects 
    private ParticleSystem puff;
    private TrailRenderer trail;
    
    float time = 0;

    public bool JumpAnimationTriggered;
    public bool JumpAnimationEnded;

    void InitEffects()
    {
        puff = Instantiate(CharacterSubManager.ActiveCharacter.Effects[3].ps);
        puff.transform.parent = SubManager.Player.PlayerSubManager.instance.Player_Object.transform;
        puff.transform.localPosition = new Vector3(0, 0, 0);

        trail = Instantiate(CharacterSubManager.ActiveCharacter.Effects[4].tr);
        trail.transform.parent = SubManager.Player.PlayerSubManager.instance.Player_Object.transform;
        trail.transform.localPosition = new Vector3(0, 0, 0);
        trail.enabled = false;
    }

    public override void InitializeSubManager()
    {
    }

    public override void OnPostInit()
    {
    }

    //spawn the player on the platforms
    public override void OnGameLoad()
    {
        InitEffects();
    }

    //begin input detection
    public override void OnGameStart()
    { 
    }

    //player dies, this runs after
    public override void OnGameEnd()
    {

    }

    //starting positions everyone!
    public override void OnGameReset()
    {

    }


    private void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameStates.Intra)
        {
            time += Time.deltaTime;

            if (GameManager.instance.debugMode)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    time = 0;
                    JumpAnimationTriggered = true;
                    JumpAnimationEnded = false;
                    trail.enabled = true;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                }
            }

            if (JumpAnimationTriggered)
            {
                SubManager.Player.PlayerSubManager.player_Character.Effects[0].Play(time, 5f, ref JumpAnimationEnded);

                if (JumpAnimationEnded)
                {
                    //trail.enabled = false;
                    SubManager.Player.PlayerSubManager.player_Character.Effects[0].Rewind(time, 5f);
                }
            }

        }

    }
}
