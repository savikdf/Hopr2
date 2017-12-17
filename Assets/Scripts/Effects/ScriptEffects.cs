using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptEffects {

    public class JumpEffect : ScriptEffect
    {
        public JumpEffect(float _duration) : base(_duration)
        {

        }
    }

    public class PuffEffect : ParticleEffect
    {
        public PuffEffect(float _duration, GameObject _o) : base(_duration, _o)
        {

        }
    }

}