﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.CharacterMan;
using System;

public class Player_Character : MonoBehaviour
{
    public static Player_Character instance;
    public float GRAVITY = -9.8f;

    public Character player_Character;
    public GameObject playerModelObject;
    public Model playerModel;

    private bool Animating;
    private bool Moving;
    private bool Grounded = true;
    private bool Jumping = false;
    private bool Landed = false;
    private bool Falling = false;

    private ParticleSystem puff;
    private TrailRenderer trail;

    Vector3 vector;
    Vector3 fallVel;

    public float jumpPower;
    float currentLerpTime;

    float lerpTime = 4.0f;

    [Range(0, 55)]
    public float jumpPowerMultiplier;


    [Range(0, 360)]
    public float TargetAngle;

    void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    void Start()
    {
        InitRender();

        player_Character.Effects[0].Set(this.transform);
        player_Character.Effects[2].Set(this.transform);

        InitEffects();
    }

    void InitEffects()
    {
   
        puff = Instantiate(player_Character.Effects[3].ps);
        puff.transform.parent = this.transform;
        puff.transform.localPosition = new Vector3(0, 0, 0);

        trail = Instantiate(player_Character.Effects[4].tr);
        trail.transform.parent = this.transform;
        trail.transform.localPosition = new Vector3(0, 0, 0);
        //trail.enabled = false;
    }

    #region Shite

    // void LateUpdate()
    // {
    //     //Jump();
    //     //Gravity();
    // }
    //
    // void Jump()
    // {
    //     if (Jumping)
    //     {
    //         Grounded = false;
    //         float yValue = 0;
    //
    //
    //         float dist = Mathf.Abs(transform.position.y - jumpPower);
    //
    //         if (dist > 5.0f)
    //         {
    //             if (jumpPower > 0)
    //             {
    //
    //                 if (jumpPower < 50)
    //                     jumpPower = 50;
    //
    //                 puff.Emit((int)jumpPower);
    //                 trail.enabled = true;
    //                 currentLerpTime += Time.deltaTime;
    //
    //                 if (currentLerpTime > lerpTime)
    //                 {
    //                     currentLerpTime = lerpTime;
    //                 }
    //  
    //                 float perc = currentLerpTime / lerpTime;
    //                 perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
    //
    //                 yValue = Mathf.Lerp(transform.position.y, jumpPower, perc);
    //                 float yValueNormal = Utils.Norm(yValue, 0, jumpPower - 5.0f);
    //                 //player_Character.Effects[2].Play(yValueNormal, 1, TargetAngle, 0, 0);
    //
    //           
    //                 
    //                 transform.position =  new Vector3(transform.position.x, yValue, transform.position.z);
    //             }
    //
    //
    //         }
    //         else
    //         {
    //             Jumping = false;
    //         }
    //
    //
    //     }
    //
    //     #region Effect
    //
    //         if (Jumping && !Grounded)
    //         {
    //             player_Character.Effects[1].Up(playerModel.Larm.transform, playerModel.Rarm.transform);
    //         }
    //         else if (Grounded)
    //         {
    //             player_Character.Effects[1].Reset(playerModel.Larm.transform, playerModel.Rarm.transform);
    //             //player_Character.Effects[2].Reset();
    //             trail.enabled = false;
    //         }
    // 
    //     #endregion
    // }
    //
    // void Gravity()
    // {
    //     if (transform.position.y > 0 && !Jumping)
    //     {
    //         fallVel += new Vector3(0, GRAVITY, 0) * Time.deltaTime * 1.2f;
    //
    //         transform.position += fallVel;
    //         Grounded = false;
    //         Falling = true;
    //     }
    //     else if (transform.position.y <= 0)
    //     {
    //         Falling = false;
    //         Grounded = true;
    //         fallVel = Vector3.zero;
    //         transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    //     }
    //
    //
    //     if (transform.position.y <= 0 && Falling)
    //     {
    //         Landed = true;
    //     }
    // }
    //
    // void JumpInputHandling(KeyCode jumpKey)
    // {
    //
    //     if (!Input.GetKey(jumpKey))
    //     {
    //         player_Character.Effects[0].Rewind(Time.deltaTime, 124);
    //         player_Character.Effects[2].Rewind(Time.deltaTime, 2);
    //
    //     }
    //
    //     if (Input.GetKeyUp(jumpKey))
    //     {
    //         Jumping = true;
    //
    //     }
    //
    //     if (Input.GetKeyDown(jumpKey))
    //     {
    //         jumpPower = 0;
    //         currentLerpTime = 0;
    //     }
    //
    //     if (Input.GetKey(jumpKey))
    //     {
    //         jumpPower += Time.deltaTime * jumpPowerMultiplier;
    //
    //         player_Character.Effects[0].Play(Time.deltaTime, 2);
    //     }
    // }
    //
    // void MovmentInputHandling()
    // {
    //     if (!Moving)
    //     {
    //         if (Input.GetKeyDown(KeyCode.W))
    //         {
    //             vector = transform.position + new Vector3(0, 0, 25);
    //
    //         }
    //
    //         if (Input.GetKeyDown(KeyCode.S))
    //         {
    //             vector = transform.position - new Vector3(0, 0, 25);
    //
    //         }
    //
    //         if (Input.GetKeyDown(KeyCode.D))
    //         {
    //             vector = transform.position + new Vector3(25, 0, 0);
    //
    //         }
    //
    //         if (Input.GetKeyDown(KeyCode.A))
    //         {
    //             vector = transform.position - new Vector3(25, 0, 0);
    //
    //         }
    //     }
    // }
    //
    void Update ()
     {
    
         if (!Jumping && Grounded)
         {
            //MovmentInputHandling();
            //JumpInputHandling(KeyCode.W);
            //LerpMove(vector, Time.deltaTime, 5);
            JumpAnimation();
         }
     }
    
    void JumpAnimation()
    {
        if(Input.GetKeyUp(KeyCode.W))
        puff.Emit(50);
    }

    // void LerpMove(Vector3 newPos, float delta, float speed)
    // {
    //     float dist = Vector3.Distance(newPos, transform.position);
    //
    //     if(dist > 0.4f)
    //     {
    //         Moving = true;
    //         transform.position = Vector3.Lerp(transform.position, newPos, delta * speed);
    //     }
    //     else
    //     {
    //         Moving = false;
    //         transform.position = newPos;
    //     }
    //
    // }

    #endregion

    public void InitRender()
    {
        try
        {
            player_Character = CharacterManager.instance.GetCurrentActiveCharacter();


            if (playerModelObject != null)
            {
                Destroy(playerModelObject);
            }

            playerModelObject = Instantiate(player_Character.Model.mainObject, this.transform.position, Quaternion.identity);
            playerModelObject.transform.parent = this.transform;
            playerModelObject.name = player_Character.name;

            //Instantiate(player_Character.Model.Body, this.transform.position, Quaternion.identity);



            playerModel.Body = (playerModelObject).transform.GetChild(0).gameObject;
            playerModel.Larm = (playerModelObject).transform.GetChild(1).gameObject;
            playerModel.Lleg = (playerModelObject).transform.GetChild(2).gameObject;
            playerModel.Rarm = (playerModelObject).transform.GetChild(3).gameObject;
            playerModel.Rleg = (playerModelObject).transform.GetChild(4).gameObject;

        }
        catch (Exception ex)
        {
            Debug.LogError("Player_Character.InitRender(): " + ex.Message);
        }

    }
}