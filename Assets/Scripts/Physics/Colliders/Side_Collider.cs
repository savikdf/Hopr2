using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Collider : MonoBehaviour
{

    public Face[] face;
    public VariableManager physicsOptions;
    Vector3[] vertices;
    // Use this for initialization
    void Start()
    {

        //face = new Face[3];
        //physicsOptions = GameObject.Find("GameManager").GetComponent<VariableManager>();
//
        //Vector3 c = transform.localPosition;
//
        //vertices = new Vector3[this.GetComponent<MeshFilter>().mesh.vertices.Length];
        //vertices = this.GetComponent<MeshFilter>().mesh.vertices;
//
        //Vector3 l = ((new Vector3(vertices[11].x, vertices[11].y, 0)));
        //Vector3 r = ((new Vector3(vertices[1].x, vertices[1].y, 0)));
//
        //face[0] = new Face(l, r, c, Vector3.up, false, this.gameObject);
        //face[1] = new Face(r, transform.InverseTransformPoint(new Vector3(0,0,0)), c, Vector3.up, false, this.gameObject);
        //face[2] = new Face(l, transform.InverseTransformPoint(new Vector3(0,0,0)), c, Vector3.up, false, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        face[0].Update();
        face[1].Update();
        face[2].Update();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (VariableManager.P_Options.showDebugs)
            {
                face[0].DrawFace();
                face[1].DrawFace();
                face[2].DrawFace();
            }
        }
    }
}
