using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Face
{
    public Segment[] segments = new Segment[4];

    public Face(Vector3 c, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 n, GameObject obj)
    {
        //Top
        segments[0] = new Segment(p0, p1, c, n, false, 1.0f, obj);
        //Bottom
        segments[1] = new Segment(p2, p3, c, n, false, 1.0f, obj);
        //Left
        segments[2] = new Segment(p0, p2, c, n, false, 1.0f, obj);
        //Right
        segments[3] = new Segment(p1, p3, c, n, false, 1.0f, obj);
    }

    public void UpdateFace()
    {
        foreach (Segment s in segments)
            s.UpdateSegment();
    }
    public void DrawFace()
    {
        foreach (Segment s in segments)
            s.DrawSegment();
    }
}


public class Character_Collider : MonoBehaviour
{
    public Face[] faces = new Face[6];
    // Use this for initialization
    void Start()
    {
       //Front Face
       faces[0] = new Face(transform.parent.position + new Vector3(0, 0f, 0),
       new Vector3(-2, 15, 1f), new Vector3(2, 15, 1f),
       new Vector3(-2, 0, 1f), new Vector3(2, 0, 1f),
       new Vector3(0, 1, 0), transform.gameObject);
//
       //Back Face
       faces[1] = new Face(transform.parent.position + new Vector3(0, 0f, 0),
       new Vector3(-2, 15, -2f), new Vector3(2, 15, -2f),
       new Vector3(-2, 0, -2f), new Vector3(2, 0, -2f),
       new Vector3(0, 1, 0), transform.gameObject);

       //Top Face
       faces[2] = new Face(transform.parent.position + new Vector3(0, 0f, 0),
       new Vector3(-2, 15, 1f), new Vector3(2, 15, 1f),
       new Vector3(-2, 15,  -2f), new Vector3(2, 15, -2f),
       new Vector3(0, 1, 0), transform.gameObject);
//
       //Top Face
       faces[3] = new Face(transform.parent.position + new Vector3(0, 0f, 0),
       new Vector3(-2, 0, 1f), new Vector3(2, 0, 1f),
       new Vector3(-2, 0,  -2f), new Vector3(2, 0, -2f),
       new Vector3(0, 1, 0), transform.gameObject);
       
       //Right Face
       faces[4] = new Face(transform.parent.position + new Vector3(0, 0f, 0),
       new Vector3(-2, 15, 1f), new Vector3(-2, 15, -2f),
       new Vector3(-2, 0, 1f), new Vector3(-2, 0, -2f),
       new Vector3(0, 1, 0), transform.gameObject);

       //Left Face
       faces[5] = new Face(transform.parent.position + new Vector3(0, 0f, 0),
       new Vector3(2, 15, 1f), new Vector3(2, 15, -2f),
       new Vector3(2, 0, 1f), new Vector3(2, 0, -2f),
       new Vector3(0, 1, 0), transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Face f in faces)
            f.UpdateFace();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (Face f in faces)
                f.DrawFace();
        }
    }
}
