using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrailEffect : BaseEffect
{

    public TrailEffect(float _duration) : base(_duration)
    {
        base.name = this.GetType().Name;
    }


    public TrailEffect(float _duration, TrailRenderer _tr) : base(_duration)
    {
        base.name = this.GetType().Name;
        tr = _tr;
    }

    public void SetTrailRenderer(TrailRenderer _tr)
    {
        tr = _tr;
    }
}