using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Transfer : MonoBehaviour {

    public GameObject Main;
    MeshRenderer mainMaterial;

    public bool isDebug;

    public static Vector4[] normals;


    int[] DitherPattern = new int[]{0, 32, 8, 40, 2, 34, 10, 42,
                     48, 16, 56, 24, 50, 18, 58, 26,
                     12, 44, 4, 36, 14, 46, 6, 38,
                     60, 28, 52, 20, 62, 30, 54, 22,
                     3, 35, 11, 43, 1, 33, 9, 41,
                     51, 19, 59, 27, 49, 17, 57, 25,
                     15, 47, 7, 39, 13, 45, 5, 37,
                     63, 31, 55, 23, 61, 29, 53, 21 };

    float[] DitherPattern2 = new float[]{0, 48, 12, 60, 3, 51, 15, 63,
                                    32, 16, 44, 28, 35, 19, 47, 31,
                                    8, 56, 4, 52, 11, 59, 7, 55,
                                    40, 24, 36, 20, 43, 27, 39, 23,
                                    2, 50, 14, 62, 1, 49, 13, 61,
                                    34, 18, 46, 30, 33, 17, 45, 29,
                                    10, 58, 6, 54, 9, 57, 5, 53,
                                    42, 26, 38, 22, 41, 25, 37, 21};


    float[] DitherPattern4x4 = new float[] {0, 8, 1, 10,
                                              12, 4, 14, 6,
                                              3, 11, 1, 9,
                                              15, 7, 13, 5

    };

    int[] Dither3x3 = new int[] {
      6, 8, 4,
      1, 0, 3,
      5, 2, 7
    };

    //This is giving Wierd Results at times so will need to test or change later
    Vector4[] GrayScalePallete =  new Vector4[] {new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                                                 new Vector4(.14f, .14f, .14f, 1.0f),
                                                 new Vector4(.28f, .28f, .28f, 1.0f),
                                                 new Vector4(.43f, .43f, .43f, 1.0f),
                                                 new Vector4(.57f, .57f, .57f, 1.0f),
                                                 new Vector4(.71f, .71f, .71f, 1.0f),
                                                 new Vector4(.85f, .85f, .85f, 1.0f), };


        void Init()
    {
        isDebug = false;
    }
    // Use this for initialization
    void Start() 
    {

        mainMaterial = Main.GetComponent<MeshRenderer>();

        normals = new Vector4[7];

        normals[0] = new Vector4(0, 0, 0, 1.0f);

        Vector3 p = Main.GetComponent<MeshFilter>().mesh.normals[2];

        normals[1] = new Vector4(p.x, p.y, p.z, 1.0f);
        normals[2] = new Vector4(0, -1, 0, 1.0f);

        normals[3] = new Vector4(+1, 0, 0, 1.0f);
        normals[4] = new Vector4(-1, 0, 0, 1.0f);

        normals[5] = new Vector4(0, 0, +1, 1.0f);
        normals[6] = new Vector4(0, 0, -1, 1.0f);
        mainMaterial.material.SetVectorArray("_Points", normals);

        mainMaterial.material.SetFloatArray("indexMatrix16x16", DitherPattern2);
        mainMaterial.material.SetVectorArray("palette", GrayScalePallete);
        mainMaterial.material.SetInt("paletteSize", 8);

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnDrawGizmos()
    {
        //GameManager.instance.debugfmode
        if (Application.isPlaying)
        {

           // float minX = float.MaxValue;
           // float maxX = 0;
           // float minY = float.MaxValue;
           // float maxY = 0;
           // float minZ = float.MaxValue;
           // float maxZ = 0;
           //
           // for (int i = 0; i < mainMeshFilter.mesh.vertices.Length; i++)
           // {
           //     Vector3 p = mainMeshFilter.mesh.vertices[i];
           //
           //
           //     if (p.x < minX)
           //         minX = p.x;
           //
           //     if (p.x > maxX)
           //         maxX = p.x;
           //
           //     if (p.y < minY)
           //         minY = p.y;
           //
           //     if (p.y > maxY)
           //         maxY = p.y;
           //
           //     if (p.z < minZ)
           //         minZ = p.z;
           //
           //     if (p.z > maxZ)
           //         maxZ = p.z;
           //     //
           //     Vector3 o = new Vector3(0, 0, 0);
           //     Gizmos.DrawSphere(o, 1f);
           //     //
           //     Vector3 d = Vector3.Normalize(p - o);
           //
           //     Gizmos.DrawLine(o, d * 5);
           // }   //
           //

        }   
    }
}
