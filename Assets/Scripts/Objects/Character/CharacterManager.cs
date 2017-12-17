using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using ScriptEffects;

[System.Serializable]
public class CharacterManager : BaseSubManager
{
    public Character[] characters;

    public override void InitializeSubManager()
    {

        foreach (Character chars in characters)
        {
            if(chars.Effects.Count == 0)
            {
                chars.Effects.Add(new ScriptEffects.JumpEffect(2.0f));

                chars.Effects.Add(new ScriptEffects.PuffEffect(2.0f, chars.Model.mainObject));
            }
            
        }
            
    }
}
