using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptEffects {

    [System.Serializable]
    public class JumpEffect : ScriptEffect
    {
        public Transform target;
        float yValue;
        float storedY;
        float smallStopMargin = 0.01f;
        float bigStopMargin = 0.99f;

        public JumpEffect(float _duration) : base(_duration)
        {
        }

        public override void Play(float delta, float speed)
        {
            storedY = target.localScale.y;
            try
            {
                if (yValue > smallStopMargin)
                {
                    yValue = Mathf.Lerp(target.localScale.y, 0, delta * speed);
                }
                else
                {
                    yValue = 0;
                }
            }
            catch
            {
                Debug.Log("No transform Assigned, Please Use JumpAffect.SetTarget");
            }

            Debug.Log("Making an Effect from JumpEffect");

            target.localScale = new Vector3(target.localScale.x, yValue, target.localScale.z);
        }



        public override void Rewind(float delta, float speed)
        {
            storedY = target.localScale.y;
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
                Debug.Log("No transform Assigned, Please Use JumpAffect.SetTarget");
            }

            Debug.Log("Rewinding an Effect from JumpEffect");

            target.localScale = new Vector3(target.localScale.x, yValue, target.localScale.z);
        }

        public override void SetTarget(Transform _target)
        {
            target = _target;
        }
    }

    public class FlipEffect : ScriptEffect
    {

    }

    public class ArmsMovment : BaseEffect
    {
        public Transform target;

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
    public class PuffEffect : ParticleEffect
    {
        public PuffEffect(float _duration, GameObject _o) : base(_duration, _o)
        {

        }

        public override void Play()
        {
            Debug.Log("Making an Effect from PuffEffect");
        }
    }
}