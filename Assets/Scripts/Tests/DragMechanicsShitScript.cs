using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMechanicsShitScript : MonoBehaviour {

    GameObject playertest;
    Vector2 mousePos;
    public static float GRAVITY = -9.8f;
    public static float SCALEFACTOR = 0.95f;

    [Range(0, 100)]
    public float force;
    float time;
    public bool isGettingReady, isGrounded, isApplyingGravity; 
    public float buildup, cap, angle;
    public Vector2 Velocity;

    bool Impulse;

    public GameObject Arrow;
    public Vector3 m_ScreenMosPos;

    void Start ()
    {
    }

	void Update ()
    {
        GroundCheck();
        if (Input.GetMouseButtonDown(0))
        {
            isGettingReady = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            isGettingReady = false;
            Velocity += (new Vector2(0, 1f)) * force;
        }

        Gravity();
        ApplyForce();
        ArrowRotate();
    }

    void GetMousePos()
    {
        Vector3 MousePos = Input.mousePosition;
        MousePos.z = 3913;
        m_ScreenMosPos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Camera.main.farClipPlane));
    }

    void ApplyForce()
    {
        transform.position += (new Vector3(Velocity.x, Velocity.y, 0.0f) * Time.deltaTime) * 2.0f;
    }

    void Gravity()
    {
        if(!isGrounded)
        {
            Velocity += new Vector2(0, GRAVITY * Time.deltaTime) * SCALEFACTOR;
            isApplyingGravity = true;
        }
    }

    void GroundCheck()
    {
        if (transform.position.y < 0.1f)
        {
            isGrounded = true;
            Velocity.y = 0;
            isApplyingGravity = false;
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
        else
        {
            isGrounded = false;
        }
    }

    void getAngle()
    {
        GetMousePos();
        //Hyp Coords
        float dx = m_ScreenMosPos.x;
        float dy = m_ScreenMosPos.y;

        angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
    }

    void ArrowRotate()
    {
        getAngle();

        Quaternion rotation = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, angle)
        };
        Arrow.transform.rotation = rotation;
    }

}





