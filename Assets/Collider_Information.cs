using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_Information : MonoBehaviour
{
	public bool isColliding;
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Ass Enter");
		isColliding = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Ass Stay");
		isColliding = true;
    }

	void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Ass Enter");
		isColliding = false;
    }
}
