using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.Ad
{
    public class AdSubManager : BaseSubManager
    {
        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.Ad;
           
        }

    }
}

