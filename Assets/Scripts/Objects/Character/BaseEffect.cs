using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEffect {

    public string name;
    public float duration;

    public BaseEffect(float _duration)
    {
        duration = _duration;
    }

    public virtual void Play()
    {

    }

    public virtual void Stop()
    {

    }


}
