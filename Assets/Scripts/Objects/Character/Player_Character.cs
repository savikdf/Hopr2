﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Character : MonoBehaviour
{
    public float GRAVITY = -9.8f;

    public Character player_Character;
    public GameObject playerModelObject;
    public Model playerModel;

    public bool Animating;
    public bool Moving;
    public bool Grounded = true;
    public bool Jumping = false;

    public ParticleSystem puff;
    public TrailRenderer trail;

    Vector3 vector;
    Vector3 fallVel;

    public float jumpPower;
    float currentLerpTime;

    float lerpTime = 4.0f;


    [Range(0, 55)]
    public float jumpPowerMultiplier;

    void Awake()
    {

    
    }

    void Start ()
    {
        player_Character = CharacterManager.ActiveCharacter;

        InitRender();
    

        player_Character.Effects[0].Set(this.transform);

        InitEffects();
    }
	

    void InitEffects()
    {

        puff = Instantiate(player_Character.Effects[2].ps);
        puff.transform.parent = this.transform;

        trail = Instantiate(player_Character.Effects[3].tr);
        trail.transform.parent = this.transform;
    }

    void LateUpdate()
    {
        Jump();
        Gravity();
    }

    void Jump()
    {
        if (Jumping)
        {
            Grounded = false;
            float yValue = 0;


            float dist = Mathf.Abs(transform.position.y - jumpPower);

            if (dist > 5.0f)
            {
                if (jumpPower > 0)
                {

                    if(jumpPower > 50)
                    puff.Emit(400);

                    currentLerpTime += Time.deltaTime;

                    if (currentLerpTime > lerpTime)
                    {
                        currentLerpTime = lerpTime;
                    }
     
                    float perc = currentLerpTime / lerpTime;
                    perc = Mathf.Sin(perc * Mathf.PI * 0.5f);

                    yValue = Mathf.Lerp(transform.position.y, jumpPower, perc);
                    
                    transform.position =  new Vector3(transform.position.x, yValue, transform.position.z);
                }


            }
            else
            {
                Jumping = false;
            }


        }

        #region Effect

            if (Jumping && !Grounded)
            {
                player_Character.Effects[1].Up(playerModel.Larm.transform, playerModel.Rarm.transform);
            }
            else if (Grounded)
            {
                player_Character.Effects[1].Reset(playerModel.Larm.transform, playerModel.Rarm.transform);
            }

        #endregion
    }

    void Gravity()
    {
        if (transform.position.y > 0 && !Jumping)
        {
            fallVel += new Vector3(0, GRAVITY, 0) * Time.deltaTime * 1.2f;

            transform.position += fallVel;
            Grounded = false;
        }
        else if (transform.position.y <= 0)
        {
            Grounded = true;
            fallVel = Vector3.zero;
        }
    }

	void Update ()
    {


        if (!Input.GetKey(KeyCode.Space))
        {
            player_Character.Effects[0].Rewind(Time.deltaTime, 124);

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jumping = true;
         
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPower = 0;
            currentLerpTime = 0;
        }

        if (!Jumping && Grounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                jumpPower += Time.deltaTime * jumpPowerMultiplier;

                player_Character.Effects[0].Play(Time.deltaTime, 2);
            }

            if (!Moving)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    vector = transform.position + new Vector3(0, 0, 25);
    
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    vector = transform.position - new Vector3(0, 0, 25);

                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    vector = transform.position + new Vector3(25, 0, 0);
          
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    vector = transform.position - new Vector3(25, 0, 0);

                }
            }

            LerpMove(vector, Time.deltaTime, 5);
        }
    }

    void LerpMove(Vector3 newPos, float delta, float speed)
    {
        float dist = Vector3.Distance(newPos, transform.position);

        if(dist > 0.4f)
        {
            Moving = true;
            transform.position = Vector3.Lerp(transform.position, newPos, delta * speed);
        }
        else
        {
            Moving = false;
            transform.position = newPos;
        }

    }

    void InitRender()
    {
        playerModelObject = Instantiate(player_Character.Model.mainObject, this.transform.position, Quaternion.identity);
        playerModelObject.transform.parent = this.transform;
        playerModelObject.name = player_Character.name;


        playerModel.Body = playerModelObject.transform.GetChild(0).gameObject;//Instantiate(player_Character.Model.Body, this.transform.position, Quaternion.identity);
        playerModel.Larm = playerModelObject.transform.GetChild(1).gameObject;
        playerModel.Lleg = playerModelObject.transform.GetChild(2).gameObject;
        playerModel.Rarm = playerModelObject.transform.GetChild(3).gameObject;
        playerModel.Rleg = playerModelObject.transform.GetChild(4).gameObject;
    }
}
