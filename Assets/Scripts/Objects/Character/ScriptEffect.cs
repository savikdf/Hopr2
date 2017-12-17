using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ScriptEffect : BaseEffect {
    public ScriptEffect(float _duration) : base(_duration)
    {
        base.name = this.GetType().Name;
    }
}
