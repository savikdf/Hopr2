using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Face
{
    public Vector3 p0, p0pos;
    public Vector3 p1, p1pos;
    public Vector3 c, cPos;
    public Vector3 normal;
    public GameObject Object;
    //Probs better to just store the transform itself *shrugs*
    public Vector3 storedRefrenceRotation, storedRefrencePosition;

    //Transform parent;
    public bool isDynamic;

    public Face(Vector3 _p0, Vector3 _p1, Vector3 _c, Vector3 _n,  bool _isDynamic, GameObject _object = null)
    {
        Object = (_object == null) ?  new GameObject("Collider") : _object;
        
		if((_object == null))
		{
			 Object.transform.position = c;
			 Object.transform.rotation = new Quaternion() { eulerAngles = new Vector3(90, 0, 0) };
		}

        storedRefrenceRotation = Object.transform.rotation.eulerAngles;
        storedRefrencePosition = Object.transform.position;
        isDynamic = _isDynamic;

        cPos = _c;
        p0pos = _p0;
        p1pos = _p1;

        Vector3 cM0 = Object.transform.parent.transform.TransformPoint (cPos);
        c =   (cM0);

        Vector3 pM0 =  Object.transform.TransformPoint (p0pos);
        p0 = (pM0);

        Vector3 pM1 = Object.transform.TransformPoint (p1pos);
        p1 =  (pM1);
//

        normal = _n;//Object.transform.parent.localToWorldMatrix * CreateNormal();
        normal.Normalize();
    }

    Vector3 CreateNormal()
    {   
        return  Vector3.Normalize(Vector3.Cross(new Vector3(p0.x, 0.0f,  p0.y), new Vector3(p1.x, 0.0f, p1.y))) ;
    }

    public void ApplyTransformMatrix()
    {
        Vector3 cM0 = Object.transform.parent.transform.TransformPoint (cPos);
        c =   (cM0);

            Vector3 pM0 =  Object.transform.TransformPoint (p0pos);
        p0 = (pM0);

        Vector3 pM1 = Object.transform.TransformPoint (p1pos);
        p1 =  (pM1);
//
//
       // //normal =  Object.transform.parent.localToWorldMatrix * CreateNormal();
        //normal.Normalize();

    }

    public void UpdatePoints(Vector3 _p0, Vector3 _p1)
    {
        p0pos = _p0;
        p1pos = _p1;
    }
    
    public void DrawFace()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(p0, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(p1, new Vector3(0.05f, 0.05f, 0.05f));

        Gizmos.DrawLine(p0, p1);
    
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(c, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(c, new Vector3(0.05f, 0.05f, 0.05f));


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


