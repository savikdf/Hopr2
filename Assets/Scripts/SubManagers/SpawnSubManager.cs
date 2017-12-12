using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.Spawn
{
    public class SpawnSubManager : BaseSubManager
    {
        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.Spawn;
           
        }

    }
}

