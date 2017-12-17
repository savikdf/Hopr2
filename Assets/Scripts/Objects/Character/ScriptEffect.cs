using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class ScriptEffect : BaseEffect
{

    public ScriptEffect instance;

    public ScriptEffect() : base()
    {
    }

    public ScriptEffect(float _duration) : base(_duration)
    {
        instance = new ScriptEffect();
        base.name = this.GetType().Name;
    }
}
