using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.Social
{
    public class SocialSubManager : BaseSubManager
    {    
        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.Social;
            
        }
    }
}

