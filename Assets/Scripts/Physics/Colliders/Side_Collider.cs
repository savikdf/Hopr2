using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World.Platforms;

public class Side_Collider : MonoBehaviour
{

    public Vector3[] vertices;
    public Color color;
    
    void Start()
    {
        //vertices = this.GetComponent<MeshFilter>().mesh.vertices;
        //Debug.Log(vertices.Length);
    }

    // Update is called once per frame
    void Update()
    {
        ManageCollider();
    }

    void ManageCollider()
    {
        //Lock col Object Rotation to stop col Warping
        this.GetComponent<Side>().col.transform.rotation = Quaternion.identity;

        bool pointsIn = false;
        Vector3 transformedPointCentre = transform.TransformPoint(new Vector3(0, 0, 0));

        //Confirming that points are beyond Z and ready to make a col or not
        //-------------------------------------------------------------------------
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 transformedPoint = transform.TransformPoint(vertices[i]);

            if (transformedPoint.z > 0)
            {
                pointsIn = true;
                break;
            }
        }
        //-------------------------------------------------------------------------

        if (pointsIn)
        {
            if (transformedPointCentre.z > 0)
            {
                this.GetComponent<Side>().col.enabled = true;
            }
            else
                this.GetComponent<Side>().col.enabled = false;
        }

        Vector3 p0 = transform.TransformPoint(vertices[0]);
        Vector3 p1 = transform.TransformPoint(vertices[1]);
        Vector3 p2 = transform.TransformPoint(vertices[2]);
        Vector3 p3 = transform.TransformPoint(vertices[3]);
        //
        p0.z = 1;
        p1.z = 1;
        p2.z = 1;
        p3.z = 1;

        this.GetComponent<Side>().col.size = new Vector2(Mathf.Abs(Vector3.Distance(p2, p3)), Mathf.Abs((p0.y - p2.y)));
    }

    //Keep this here as a back Up System incase checking via centre location proves to be problomatic
    //if (ShowDebugData)
    //{
    //    float dx = transformedPointCentre.x;
    //    float dy = transformedPointCentre.y;
    //
    //    Debug.Log(this.name + ": " + Mathf.Atan2(dy, -dx) * Mathf.Rad2Deg);
    //}

    void OnDrawGizmos()
    {
        //Draw Points and Sort Points
        if (Application.isPlaying && VariableManager.P_Options.showDebugs)
        {
            bool pointsIn = false;
            Vector3 transformedPointCentre = transform.TransformPoint(new Vector3(0, 0, 0));

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 transformedPoint = transform.TransformPoint(vertices[i]);

                if (transformedPoint.z > 0)
                {
                    pointsIn = true;
                    break;
                }
            }

            for (int j = 0; j < vertices.Length; j++)
            {
                if (pointsIn)
                {
                    if (transformedPointCentre.z > 0)
                    {
                        Vector3 transformedPoint = transform.TransformPoint(vertices[j]);
                        Vector3 finalPoint = transformedPoint;

                        finalPoint.z = 1;

                        Gizmos.color = color;
                        Gizmos.DrawCube(finalPoint, new Vector3(0.05f, 0.05f, 0.05f));
                        Gizmos.DrawCube(transformedPointCentre, new Vector3(0.05f, 0.05f, 0.05f));
                        Gizmos.DrawRay(transformedPointCentre, transform.TransformPoint(this.GetComponent<MeshFilter>().mesh.normals[0]) / 100.0f);
                    }
                }
            }
        }
    }
}
