using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEffect  {

    public string name;
    public float duration;
    public Transform t;

    public ParticleSystem ps;
    public TrailRenderer tr;

    public BaseEffect()
    {

    }

    public BaseEffect(float _duration)
    {
        duration = _duration;
    }


    public virtual void Play()
    {

    }

    public virtual void Rewind()
    {

    }

    public virtual void Rewind(float delta, float speed)
    {

    }

    public virtual void Play(float delta, float speed)
    {

    }


    public virtual void Play(float delta, float speed, ref bool status)
    {

    }



    public virtual void Play(float delta, float speed, float X, float Y, float Z)
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void Set(Transform _t)
    {
        t = _t;
    }

    public virtual void Up(Transform l, Transform r)
    {

    }

    public virtual void Reset()
    {

    }

    public virtual void Reset(Transform l, Transform r)
    {

    }
}
