using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Character : MonoBehaviour
{
    public float GRAVITY = -9.8f;

    public Character player_Character;
    public bool Animating;
    public bool Moving;
    public bool Grounded;
    public bool Jumping;

    Vector3 vector;
    Vector3 fallVel;

    public float jumpPower;

    [Range(0, 55)]
    public float jumpPowerMultiplier;

    void Awake()
    {

    
    }

    void Start ()
    {
        player_Character = CharacterManager.ActiveCharacter;

        InitRender();
        player_Character.Effects[0].SetTarget(this.transform);
    }
	

    void LateUpdate()
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
            
        }


        if(Grounded)
        {
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, 0, 0);

            player_Character.Model.Larm.transform.rotation = rot;
            player_Character.Model.Rarm.transform.rotation = rot;

        }
        else
        {
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(180, 0, 0);

            player_Character.Model.Larm.transform.rotation = rot;
            player_Character.Model.Rarm.transform.rotation = rot;

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


        if(Jumping)
        {
            float yValue = 0;


            float dist = Mathf.Abs(transform.position.y - jumpPower);

            if (dist > 2.7f)
            {
                if(jumpPower > 0)
                    yValue = Mathf.Lerp(transform.position.y, jumpPower, Time.deltaTime * (jumpPower/25));

                transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
            }
            else
            {
                Jumping = false;
            }


        }

        Debug.Log(Jumping);
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
        GameObject model = Instantiate(player_Character.Model.mainObject, this.transform.position, Quaternion.identity);
        model.transform.parent = this.transform;
        model.name = player_Character.name;
    }
}
