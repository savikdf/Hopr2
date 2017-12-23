using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptEffects {

    [System.Serializable]
    public class JumpEffect : ScriptEffect
    {
        float yValue;
        float storedY;
        float min = 0.2f;
        float smallStopMargin = 0.01f;
        float bigStopMargin = 0.99f;


        public JumpEffect(float _duration) : base(_duration)
        {
        }

        public override void Play(float delta, float speed, bool trigger)
        {
            storedY = t.localScale.y;

            try
            {
                if (yValue > smallStopMargin)
                {
                    yValue = Mathf.Lerp(t.localScale.y, min, delta * speed);
          
                    t.localScale = new Vector3(((1.0f - Mathf.Sqrt(yValue)) + 1), yValue, ((1.0f - Mathf.Sqrt(yValue)) + 1));
                }
                else
                {
                    trigger = true;
                    yValue = min;
                    Debug.Log("WHAT THE FUCK");
                    return;
                }
            }
            catch
            {
                Debug.Log("No transform Assigned, Please Use JumpAffect.Sett");
            }
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
                    t.localScale = new Vector3((1.0f - Mathf.Sqrt(yValue)) + 1, yValue, (1.0f - Mathf.Sqrt(yValue)) + 1);
                }
            }
            catch
            {
                Debug.Log("No transform Assigned, Please Use JumpAffect.Sett");
            }

            //Debug.Log("Rewinding an Effect from JumpEffect");
        }

        public override void Set(Transform _p)
        {
            base.t = _p;
        }
    }

    public class FlipEffect : ScriptEffect
    {
        //float angle;

        public FlipEffect(float _duration) : base(_duration)
        {
        }

        public override void Play(float delta, float speed, float X, float Y, float Z)
        {
            try
            {
                t.rotation =  Quaternion.Euler((delta * speed) * X, ((delta * speed) * Y), (delta * speed) * Z);
                
            }
            catch
            {
                Debug.Log("No transform Assigned, Please Use FlipEffect.Set");
            }

            Debug.Log("Making an Effect from FlipEffect");
        }



        public override void Reset()
        {

            t.rotation = Quaternion.Euler(0, 0, 0);
            //Debug.Log("Resting");
        }

        public override void Set(Transform _p)
        {
            base.t = _p;
        }
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