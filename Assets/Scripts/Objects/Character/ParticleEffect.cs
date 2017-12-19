using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticleEffect : BaseEffect
{
    public ParticleEffect(float _duration) : base(_duration)
    {
        base.name = this.GetType().Name;
    }

    public ParticleEffect(float _duration, ParticleSystem _ps) : base(_duration)
    {
       base.name = this.GetType().Name;
       ps = _ps;
    }

    public override void Play()
    {
        ps.Play();
    }

    public void SetParticleSystem(ParticleSystem _ps)
    {
        ps = _ps;
    }

}
