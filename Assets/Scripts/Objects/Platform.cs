using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;


namespace SubManager.World.Platforms
{
    public class Platform : MonoBehaviour
    {
        //hodler of side objects, and their bools
        Transform[] sides;
        bool[] sides_data;

        WorldSubManager.PlatformTypes thisPlatformType;



        private void Awake()
        {
            sides = new Transform[4];
            sides_data = new bool[4] { true, true, true, true };
            thisPlatformType = WorldSubManager.PlatformTypes.Normal;    //TODO, fix this when more types exist

            //finds sides and sets them in the sides[]
            for (int i = 0; i < 4; i++)
            {
                //naming starts at S_01--> S_04
                sides[i] = transform.Find(string.Format("S_0{0}", (i + 1).ToString()));
            }

            //sets the side colors
            for (int i = 0; i < sides.Length; i++)
            {
                if (i % 2 == 0)
                {
                    //true, green
                    sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_Y;
                    sides_data[i] = true;
                }
                else if (i % 2 != 0)
                {
                    //false, red
                    sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_N;
                    sides_data[i] = false;            
                }
            }
        }

    }

}
