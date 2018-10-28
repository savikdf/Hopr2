using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Collider : MonoBehaviour
{

    public Segment[] segment;
    //public Vector3[] vertices;
    // Use this for initialization
    //public Vector3 testPoint;
    void Start()
    {

     // segment = new Segment[12];
//
     // Vector3 c = transform.localPosition;
     // //
     // vertices = new Vector3[this.GetComponent<MeshFilter>().mesh.vertices.Length];
     // vertices = this.GetComponent<MeshFilter>().mesh.vertices;
     // //   
     // Vector3 l = ((new Vector3(vertices[11].x, vertices[11].y, 0)));
     // Vector3 r = ((new Vector3(vertices[1].x, vertices[1].y, 0)));
     // //   
//
     // Vector3 innerr = ((new Vector3(vertices[3].x, vertices[3].y, 0)));
     // Vector3 innerl = ((new Vector3(vertices[10].x, vertices[10].y, 0)));
//
     // Vector3 innerc = Utils.MidPoint(l, r);
     // //Vertices index 3 and 10
//
     // // Big Triangle
     // segment[0] = new Segment(l, r, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[1] = new Segment(r, transform.InverseTransformPoint(transform.parent.position), c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[2] = new Segment(l, transform.InverseTransformPoint(transform.parent.position), c, Vector3.up, false, 1.0f, this.gameObject);
//
     // //front traingle
     // segment[3] = new Segment(innerl, innerr, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[4] = new Segment(innerr, innerc, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[5] = new Segment(innerl, innerc, c, Vector3.up, false, 1.0f, this.gameObject);
//
     // //front right traingle
     // segment[6] = new Segment(innerl, l, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[7] = new Segment(innerl, innerc, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[8] = new Segment(l, innerc, c, Vector3.up, false, 1.0f, this.gameObject);
//
//
     // //front left traingle
     // segment[9] = new Segment(innerr, r, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[10] = new Segment(innerr, innerc, c, Vector3.up, false, 1.0f, this.gameObject);
     // segment[11] = new Segment(r, innerc, c, Vector3.up, false, 1.0f, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Segment s in segment)
            s.UpdateSegment();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && VariableManager.P_Options.showDebugs)
        {
            //foreach (Vector3 vert in vertices)
            //    Gizmos.DrawSphere(transform.TransformPoint(vert), .01f);

                for (int i = 0; i < segment.Length; i++)
                {
                    segment[i].DrawSegment();
                }           
        }
    }
}
