using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticleEffect : BaseEffect
{
    public ParticleSystem p;

    public ParticleEffect(float _duration, GameObject o) : base(_duration)
    {
       base.name = this.GetType().Name;
        GameObject pObject = new GameObject();
       pObject.name = this.GetType().Name;

       p = pObject.AddComponent<ParticleSystem>();
       p.Stop();

       var main = p.main;
       main.duration = base.duration;
       pObject.transform.root.parent = o.transform;
    }
}
