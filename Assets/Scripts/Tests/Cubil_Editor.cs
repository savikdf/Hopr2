using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Cubil_Editor : MonoBehaviour
{
    public List<GameObject> Cubes = new List<GameObject>();
    public uint index = 0;
    public uint prevIndex = 1;

    bool UseIndex = false;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame

    void FixedUpdate() {

        MainCubeLoop();
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.F1))
            UseIndex = !UseIndex;

        if (UseIndex)
            IndexSelect();


    }

    void IndexSelect()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            index++;
            index = index % 512;
            prevIndex = (index - 1) % 512;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            index--;
            index = index % 512;
            prevIndex = (index + 1) % 512;
        }

        Cubes[(int)index].SetActive(true);
        Cubes[(int)prevIndex].SetActive(false);
    }

    void MainCubeLoop()
    {
        foreach (GameObject g in Cubes)
        {   
            //Have To Check if is Active...for Now
            MatchMousePosition(g);

            if(g.activeSelf)
            {

            }
            //Check if Is Active
            //Do More Shizz
        }
    }

    void MatchMousePosition(GameObject cube)
    {

        Material g_mat = cube.gameObject.GetComponent<MeshRenderer>().material;

        if (Camera_Physics.point == cube.transform.position && !cube.activeSelf)
        {
            cube.gameObject.SetActive(true);
        }
        else if (Camera_Physics.point == cube.transform.position && cube.activeSelf)
        {
            g_mat.color = Color.red;
        }
        else if (Camera_Physics.point != cube.transform.position && cube.activeSelf && g_mat.color == Color.red)
        {
            g_mat.color = Color.white;
        }
    }

}
