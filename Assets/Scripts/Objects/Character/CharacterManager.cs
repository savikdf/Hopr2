using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

[System.Serializable]
public class CharacterManager : BaseSubManager
{
    public Character[] characters;

    public static uint index = 0;

    public static Character ActiveCharacter;

    public override void InitializeSubManager()
    {
        if(characters != null && characters.Length > 0)
        {
            foreach (Character chars in characters)
            {
                if (chars.Effects.Count == 0)
                {
                    chars.Effects.Add(new ScriptEffects.JumpEffect(2.0f));
                    chars.Effects.Add(new ScriptEffects.ArmsMovment(2.0f));
                    chars.Effects.Add(new ScriptEffects.FlipEffect(2.0f));

                    chars.Effects.Add(new ParticleEffect(2.0f, ParticleEffects.ParticleEffectLoad.PuffLoad()));
                    chars.Effects.Add(new TrailEffect(2.0f, TrailEffects.TrailEffectLoad.SmokeTrialLoad()));
                }

            }

            ActiveCharacter = characters[index];
        }        
    }

    public Character GetCurrentActiveCharacter()
    {
        if(characters != null)
            return characters[0];

        return new Character();
    }


    //runs on the post init event from the gamemanager
    //use this to start communicating with other subManagers
    public override void OnPostInit()
    {

    }

    //runs on the game load event from the gamemanager
    //use this to begin the setup of the game
    public override void OnGameLoad()
    {

    }

    //runs on the game start event from the gamemanager
    //use this to begin the process of the game
    public override void OnGameStart()
    {

    }

    //runs on the game start event from the gamemanager
    //use this to begin the process of the game
    public override void OnGameEnd()
    {

    }
}
