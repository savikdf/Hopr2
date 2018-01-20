using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Collider : MonoBehaviour
{

    public Face face;
    public VariableManager physicsOptions;
    Vector3[] vertices;
    // Use this for initialization
    void Start()
    {

        physicsOptions = GameObject.Find("GameManager").GetComponent<VariableManager>();

        Vector3 c = transform.localPosition;

        vertices = new Vector3[this.GetComponent<MeshFilter>().mesh.vertices.Length];
        vertices = this.GetComponent<MeshFilter>().mesh.vertices;

        Vector3 l = ((new Vector3(vertices[11].x, vertices[11].y, 0)));
        Vector3 r = ((new Vector3(vertices[1].x, vertices[1].y, 0)));

        //face = new Face(l, r, c, Vector3.up, false, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        face.Update();
    }


    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (physicsOptions.physicsOptions.showDebugs)
            {
                //face.DrawFace();
                for (int j = 0; j < vertices.Length; j++)
                {
                   // Vector3 tPoint = (transform.TransformPoint(vertices[j]));
                  //  Gizmos.DrawSphere(tPoint, 0.05f);
                }

            }
        }
    }
}
