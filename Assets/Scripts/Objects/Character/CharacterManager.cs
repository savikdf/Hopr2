using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using ScriptEffects;

[System.Serializable]
public class CharacterManager : BaseSubManager
{
    public Character[] characters;

    public static uint index = 1;

    public static Character ActiveCharacter;

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

        ActiveCharacter = characters[index];
    }

    public Character GetCurrentActiveCharacter()
    {
        return characters[0];
    }
}
