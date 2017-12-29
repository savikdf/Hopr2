using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;

namespace SubManager.CameraMan
{
    public class ActionDeathScreenController : BaseSubManager
    {

        public bool isGrayed;

        Image MainImage;
        Material MainImageMaterial;

        public override void OnPostInit()
        {
            MainImage = this.GetComponent<Image>();
            MainImageMaterial = MainImage.material;
        }

        public override void OnGameLoad()
        {

        }

        public override void OnGameStart()
        {
            int i = isGrayed ? 1 : 0;
            MainImageMaterial.SetInt("_IsBnW", 1);
        }

        public override void OnGameEnd()
        {

        }

        public override void OnGameReset()
        {

        }
    }
}
