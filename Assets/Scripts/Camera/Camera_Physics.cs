using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just Creating the main ray to use later for intersection checks
/// </summary>
public class Camera_Physics : MonoBehaviour
{
    [Range(0, 25)]
    public float rayLength;

    public static Vector3 point;

    [Range(0, 32)]
    public int gridSize;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);


        Debug.DrawRay(ray.origin, (ray.direction * rayLength), Color.green);
            
        if(Physics.Raycast(ray, out hit))
        {   

            //Super Buggy, Gadda Fix
            //TODO: 
            // FUX DIZ SHIZZ
            point.x = (Utils.roundNearest(hit.point.x , (hit.point.x * 2f)/(float)7)) + .5f;
            //point.y = Utils.roundNearest(point.y , (point.y * 2) / (float)2.5) + 1;
            point.z = (Utils.roundNearest(hit.point.z , (hit.point.z * 2f)/(float)7)) + .5f;

            //Debug.Log(point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(point, 0.5f);
    }
}
