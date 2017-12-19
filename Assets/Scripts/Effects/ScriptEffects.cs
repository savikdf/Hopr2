using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptEffects {

    [System.Serializable]
    public class JumpEffect : ScriptEffect
    {
        float yValue;
        float storedY;
        float smallStopMargin = 0.01f;
        float bigStopMargin = 0.99f;

        public JumpEffect(float _duration) : base(_duration)
        {
        }

        public override void Play(float delta, float speed)
        {
            storedY = t.localScale.y;
            try
            {
                if (yValue > smallStopMargin)
                {
                    yValue = Mathf.Lerp(t.localScale.y, 0, delta * speed);
                }
                else
                {
                    yValue = 0;
                }
            }
            catch
            {
                Debug.Log("No transform Assigned, Please Use JumpAffect.Sett");
            }

            Debug.Log("Making an Effect from JumpEffect");

            t.localScale = new Vector3(((1.0f - Mathf.Sqrt(yValue)) + 1), yValue, ((1.0f - Mathf.Sqrt(yValue)) + 1));
        }



        public override void Rewind(float delta, float speed)
        {
            storedY = t.localScale.y;
            try
            {
                if (yValue < bigStopMargin)
                {
                    yValue = Mathf.Lerp(storedY, 1.0f, delta * speed);
                }
                else
                {
                    yValue = 1.0f;
                }
            }
            catch
            {
                Debug.Log("No transform Assigned, Please Use JumpAffect.Sett");
            }

            //Debug.Log("Rewinding an Effect from JumpEffect");

            t.localScale = new Vector3((1.0f - Mathf.Sqrt(yValue)) + 1, yValue, (1.0f - Mathf.Sqrt(yValue)) + 1);
        }

        public override void Set(Transform _p)
        {
            base.t = _p;
        }
    }

    public class FlipEffect : ScriptEffect
    {

    }

    public class ArmsMovment : ScriptEffect
    {
        public ArmsMovment(float _duration) : base(_duration)
        {

        }

        public override void Up(Transform l, Transform r)
        {
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(180, 0, 0);

            l.rotation = rot;
            r.rotation = rot;
        }

        public override void Reset(Transform l, Transform r)
        {
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, 0, 0);

            l.rotation = rot;
            r.rotation = rot;
        }

    }
}

namespace ParticleEffects
{
    [System.Serializable]
    public static class ParticleEffectLoad
    {
        public static ParticleSystem PuffLoad()
        {
            return (Resources.Load("Prefabs/Effects/PuffEffect", typeof(GameObject)) as GameObject).GetComponent<ParticleSystem>();
        }
    }
}

namespace TrailEffects
{
    [System.Serializable]
    public static class TrailEffectLoad
    {
        public static TrailRenderer SmokeTrialLoad()
        {
            return (Resources.Load("Prefabs/Effects/SmokeTrial", typeof(GameObject)) as GameObject).GetComponent<TrailRenderer>();
        }
    }
}