using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string name;
    public Model Model;
    public List<BaseEffect> Effects;
    public string Disc;


    public Character(string _name)
    {
        name = _name;
    }
}
