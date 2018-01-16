using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Face
{
    public Vector3 p0, p0pos;
    public Vector3 p1, p1pos;
    public Vector2 c;
    public Vector3 normal;
    public GameObject Object;
    //Probs better to just store the transform itself *shrugs*
    public Vector3 storedRefrenceRotation, storedRefrencePosition;

    public bool isDynamic;

    public Face(Vector3 _p0, Vector3 _p1, Vector3 pos, string name, bool _isDynamic, GameObject _object = null)
    {
        Object = (_object == null) ?  new GameObject(name) : _object;

		if((_object == null))
		{
			 Object.transform.position = pos;
			 Object.transform.rotation = new Quaternion() { eulerAngles = new Vector3(90, 0, 0) };
		}
        storedRefrenceRotation = Object.transform.rotation.eulerAngles;
        storedRefrencePosition = Object.transform.position;
        isDynamic = _isDynamic;
        p0pos = _p0;
        p1pos = _p1;

        Vector3 pM0 = Object.transform.localToWorldMatrix * (_p0);
        p0 = (pM0 + Object.transform.position);

        Vector3 pM1 = Object.transform.localToWorldMatrix * (_p1);
        p1 = pM1 + Object.transform.position;

        c = Object.transform.position;

        normal =  Object.transform.localToWorldMatrix * CreateNormal();
    }

    Vector3 CreateNormal()
    {
    
        return   Vector3.Normalize(Vector3.Cross(new Vector3(p0.x,p0.y, 0.0f), new Vector3(p1.x, p1.y, 0.0f)));
    }

    public void ApplyTransformMatrix()
    {
        c = storedRefrencePosition;

        normal = Object.transform.localToWorldMatrix * CreateNormal();

        Vector3 pM0 = Object.transform.localToWorldMatrix * (p0pos);
        p0 = pM0 + Object.transform.position;

        Vector3 pM1 = Object.transform.localToWorldMatrix * (p1pos);
        p1 = pM1 + Object.transform.position;

    }

    public void DrawFace()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(p0, 0.05f);
        Gizmos.DrawSphere(p1, 0.05f);
        Gizmos.DrawSphere(c, 0.05f);

        Gizmos.DrawLine(p0, p1);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(c, normal);
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


