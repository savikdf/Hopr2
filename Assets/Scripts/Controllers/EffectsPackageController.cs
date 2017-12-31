using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Spawn;
using SubManager.World;
using System;
using SubManager.CharacterMan;

public class EffectsPackageController : BaseSubManager
{
    //Effects 
    public ParticleSystem puff;
    public TrailRenderer trail;
    
    float time = 0;

    public bool JumpAnimationTriggered;
    public bool JumpAnimationEnded;

    void InitEffects()
    {
        if (puff != null)
            Destroy(puff.gameObject);


        puff = Instantiate(CharacterSubManager.ActiveCharacter.Effects[3].ps);
        puff.transform.parent = SubManager.Player.PlayerSubManager.instance.Player_Object.transform;
        puff.transform.localPosition = new Vector3(0, 0, 0);


        if (trail != null)
            Destroy(trail);


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
        InitEffects();
    }

    //spawn the player on the platforms
    public override void OnGameLoad()
    {

    }

    //begin input detection
    public override void OnGameStart()
    {
        //
    }

    //player dies, this runs after
    public override void OnGameEnd()
    {

    }

    //starting positions everyone!
    public override void OnGameReset()
    {
        InitEffects();
    }

    public void ResetEffects()
    {
        SubManager.Player.PlayerSubManager.player_Character.Effects[1].Reset(
        SubManager.Player.PlayerSubManager.playerModel.Larm.transform,
        SubManager.Player.PlayerSubManager.playerModel.Rarm.transform);
    }
    private void Update()
    {
        if (GameManager.instance != null)
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
                            puff.Clear();
                            if (trail != null) trail.enabled = true;


                        }
                        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                        {

                        }
                        if (Input.GetKeyDown(KeyCode.F2))
                        {
                        }
                        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
                        {
                            puff.Emit(100);
                        }
                    }


                    //  player_Character.Effects[1].Reset(playerModel.Larm.transform, playerModel.Rarm.transform);
                    if (JumpAnimationTriggered)
                    {
                        SubManager.Player.PlayerSubManager.player_Character.Effects[0].Play(time, 5f, ref JumpAnimationEnded);

                        if (JumpAnimationEnded)
                        {
                            SubManager.Player.PlayerSubManager.player_Character.Effects[0].Rewind(time, 5f);

                            SubManager.Player.PlayerSubManager.player_Character.Effects[1].Up(
                            SubManager.Player.PlayerSubManager.playerModel.Larm.transform,
                            SubManager.Player.PlayerSubManager.playerModel.Rarm.transform);

                        }
                    }

                }
            }

    }
}
