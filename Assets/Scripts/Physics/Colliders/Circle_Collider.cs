using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Circle_Collider
{
    public float size;
    public Vector3 position;

    Vector3 OPosition;

    public GameObject Object;

    public Vector3 storedRefrenceRotation, storedRefrencePosition;

    public Circle_Collider(float _size, Vector3 _position, GameObject _object)
    {
        Object = _object;
        size = _size;

        storedRefrenceRotation = Object.transform.rotation.eulerAngles;
        storedRefrencePosition = Object.transform.position;

        OPosition = _position;

        if (Object.transform.parent != null)
            position = Object.transform.TransformPoint(OPosition);
        else
            position = Object.transform.TransformPoint(OPosition);
    }

    ///<summary>Drawing Circle Collider, Using Sphere Debug</summary> 
    public void DrawCircle()
    {
		Gizmos.color = new Color(1,1,1,0.25f);
        Gizmos.DrawSphere(position, size);
    }

    public void ApplyTransformMatrix()
    {
        if (Object.transform.parent != null)
            position = Object.transform.TransformPoint(OPosition);
        else
            position = Object.transform.TransformPoint(OPosition);
    }

    public void Update()
    {
        if (Object.transform.rotation.eulerAngles != storedRefrenceRotation || Object.transform.position != storedRefrencePosition)
        {
            //If we rotate the platform in real time, we gadda update the points
            //and normals to make sure the transform effects them too

            storedRefrenceRotation = Object.transform.rotation.eulerAngles;
            storedRefrencePosition = Object.transform.position;
            ApplyTransformMatrix();
        }
    }


}
