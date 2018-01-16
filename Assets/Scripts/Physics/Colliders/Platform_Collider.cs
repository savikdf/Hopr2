using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World.Platforms;

public class Platform_Collider : MonoBehaviour {

	//Will expand later with Enumrables and Class contruct
	public uint numSides = 4;
	public Face[] CollisionFaces;
	
	Platform platform;
	// Use this for initialization
	void Start () {
		
		platform = GetComponent<Platform>();
		CollisionFaces = new Face[4];
		for(int i = 0; i < (int)numSides; i++)
		{
			Vector3 c = platform.sides[i].transform.localPosition;
			Vector3 l = c * +1.0f;
			Vector3 r = c * -1.0f;
			CollisionFaces[i] = new Face(l, r, c, "Collider", false, this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < (int)numSides; i++)
		CollisionFaces[i].Update();
	}

	void OnDrawGizmos()
	{
		for(int i = 0; i < (int)numSides; i++)
		CollisionFaces[i].DrawFace();
	}
}
