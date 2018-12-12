using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubManager.World.Platforms
{
    public class Side : MonoBehaviour
    {

        public bool isPassable = true;
        public BoxCollider2D col;
        public Collider_Information col_info;
        public bool isColliding;

        void Update(){
            isColliding = col_info.isColliding;
        }


    }
}

