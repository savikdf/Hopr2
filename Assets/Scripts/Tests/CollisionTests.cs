using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTests : MonoBehaviour
{

    public Vector3 Origin, Direction;
    // Use this for initialization
    public GameObject plat;
    void Start()
    {
        plat = GameObject.Find("Platform_Holder").transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
		CycleThroughSides();
    }

    void CycleThroughSides()
    {
        for (int i = 1; i < 5; i++)
        {
            Vector3 interesection = new Vector3(0, 0, 0);

            if (Utils.PointInTriangle(plat.transform.GetChild(i).GetComponent<Side_Collider>().segment[0].p0,
            plat.transform.GetChild(i).GetComponent<Side_Collider>().segment[0].p1, plat.transform.GetChild(i).GetComponent<Side_Collider>().segment[1].p1, Origin)
            && Utils.IsSegmentIntersection(plat.transform.GetChild(i).GetComponent<Side_Collider>().segment[0].p1,
            plat.transform.GetChild(i).GetComponent<Side_Collider>().segment[0].p0, Origin, Origin + Direction, ref interesection))
            {
                Debug.Log("Points in Yo" + i);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Origin, 0.1f);
        Gizmos.DrawLine(Origin, Direction.normalized);
        Gizmos.DrawSphere(Origin + Direction.normalized, 0.1f);
    }
}
