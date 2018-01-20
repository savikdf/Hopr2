using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World.Platforms;
using SubManager.World;
using SubManager.Difficulty;
public class Platform_Collider : MonoBehaviour
{

    //Will expand later with Enumrables and Class contruct
    public uint numSides = 4;
    public Face[] CollisionFaces;
    public Side_Collider[] sideColliders;
    public VariableManager physicsOptions;
    // Use this for initialization

    void Start()
    {
        physicsOptions = GameObject.Find("GameManager").GetComponent<VariableManager>();
        SetUpCollider();
    }

    
    void SetUpCollider()
    {

        if(sideColliders.Length <= 0)
        {
            sideColliders = new Side_Collider[4];
            sideColliders[0] = transform.GetChild(1).GetComponent<Side_Collider>();
            sideColliders[1] = transform.GetChild(2).GetComponent<Side_Collider>();
            sideColliders[2] = transform.GetChild(3).GetComponent<Side_Collider>();
            sideColliders[3] = transform.GetChild(4).GetComponent<Side_Collider>();
        }

        CollisionFaces = new Face[sideColliders.Length];

        for(int i = 0; i < sideColliders.Length; i++)
            CollisionFaces[i] = sideColliders[i].face;
    }

    public void SwitchOff()
    {
         for(int i = 0; i < sideColliders.Length; i++)
         {
            CollisionFaces[i].normal = Vector3.up;
            sideColliders[i].GetComponent<MeshRenderer>().material.color = Color.gray;
         }
    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
        }
    }
}
