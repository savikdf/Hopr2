using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEffect  {

    public string name;
    public float duration;
    public Transform p;

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

    public virtual void Stop()
    {

    }

    public virtual void SetTarget(Transform _p)
    {
        p = _p;
    }

    public virtual void Up(Transform l, Transform r)
    {

    }

    public virtual void Reset(Transform l, Transform r)
    {

    }
}
