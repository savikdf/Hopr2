#region Using Directives
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager.World;
using System;
#endregion

namespace SubManager.World.Platforms
{
    public class Platform : MonoBehaviour
    {
        //hodler of side objects, and their bools
        public List<Side> sides;                          
        public WorldSubManager.PlatformTypes thisPlatformType;


        private void Awake()
        {
            sides = new List<Side>();
            thisPlatformType = WorldSubManager.PlatformTypes.Normal;    //TODO, fix this when more types exist

            try
            {
                //finds sides and sets them in the sides[]
                for (int i = 0; i < 4; i++)
                {
                    //naming starts at S_01--> S_04
                    sides.Add(transform.Find(string.Format("S_0{0}", (i + 1).ToString())).GetComponent<Side>());
                }

                //sets the side colors
                for (int i = 0; i < sides.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        //true, green
                        sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_Y;
                        sides[i].isPassable = true;

                    }
                    else if (i % 2 != 0)
                    {
                        //false, red
                        sides[i].gameObject.GetComponent<Renderer>().material = WorldSubManager.instance.plat_N;
                        sides[i].isPassable = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Platform.Awake(): " + ex.Message);
            }

        }

    }

}
