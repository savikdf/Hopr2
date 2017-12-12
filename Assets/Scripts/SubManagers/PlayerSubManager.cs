using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;


namespace SubManager.Player
{
    public class PlayerSubManager : BaseSubManager
    {
        public override void InitializeSubManager()
        {
            thisSubType = GameManager.GameSubManagerTypes.Player;
   
        }
    }

}
