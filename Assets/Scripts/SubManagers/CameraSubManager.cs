using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.Camera
{
    public class CameraSubManager : BaseSubManager
    {
        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.Camera;
  
        }

    }
}

