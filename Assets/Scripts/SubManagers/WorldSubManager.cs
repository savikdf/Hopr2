using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.World
{
    public class WorldSubManager : BaseSubManager
    {                   
        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.World;

        }

    }
}

