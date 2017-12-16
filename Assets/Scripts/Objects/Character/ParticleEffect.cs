using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : Effect {

    public ParticleEmitter ParticleE { get; set; }

    public ParticleEffect(ParticleEmitter _particleE){ ParticleE = _particleE; }
}
