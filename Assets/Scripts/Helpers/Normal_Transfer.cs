using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Transfer : MonoBehaviour {

    MeshFilter renderZone;
    public GameObject[] zoneList;

    void Init()
    {
 
    }
	// Use this for initialization
	void Start () {
        renderZone = zoneList[0].GetComponent<MeshFilter>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (Vector3 n in renderZone.mesh.vertices)
            {
                Vector3 nWS = renderZone.transform.TransformPoint(n);
                Gizmos.DrawCube(nWS, new Vector3(.2f,.2f,.2f));
            }
        }
    }
}
