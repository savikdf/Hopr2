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


    public virtual bool Play()
    {
        return false;
    }

    public virtual bool Rewind()
    {
        return false;
    }

    public virtual bool Rewind(float delta, float speed)
    {
        return false;
    }

    public virtual bool Play(float delta, float speed)
    {
        return false;
    }

    public virtual bool Play(float delta, float speed, float X, float Y, float Z)
    {
        return false;
    }

    public virtual bool Stop()
    {
        return false;
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
