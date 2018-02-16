using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Collider : MonoBehaviour
{

    public Face[] face;
    public Vector3[] vertices;
    // Use this for initialization
    void Start()
    {

     face = new Face[4];

     Vector3 c = transform.localPosition;
      //
     vertices = new Vector3[this.GetComponent<MeshFilter>().mesh.vertices.Length];
     vertices = this.GetComponent<MeshFilter>().mesh.vertices;
//   
     Vector3 l = ((new Vector3(vertices[11].x, vertices[11].y, 0)));
     Vector3 r = ((new Vector3(vertices[1].x, vertices[1].y, 0)));
//   

     Vector3 innerl = ((new Vector3(vertices[3].x, vertices[3].y, 0)));
     Vector3 innerr = ((new Vector3(vertices[10].x, vertices[10].y, 0)));

     //Vertices index 3 and 10

     // Big Triangle
     face[0] = new Face(l, r, c, Vector3.up, false, 1.0f, this.gameObject);
     face[1] = new Face(r, transform.InverseTransformPoint(transform.parent.position), c, Vector3.up, false, 1.0f, this.gameObject);
     face[2] = new Face(l, transform.InverseTransformPoint(transform.parent.position), c, Vector3.up, false, 1.0f, this.gameObject);

     //centre front traingle

     face[3] = new Face(innerl, innerr, c, Vector3.up, false, 1.0f, this.gameObject);
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
            foreach(Vector3 vert in vertices)
                Gizmos.DrawSphere(transform.TransformPoint(vert), .01f);
            
            if (VariableManager.P_Options.showDebugs)
            {
                face[0].DrawFace();
                face[1].DrawFace();
                face[2].DrawFace();
                face[3].DrawFace();
            }
        }
    }
}
