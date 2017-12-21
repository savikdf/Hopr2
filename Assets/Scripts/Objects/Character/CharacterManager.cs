using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;

namespace SubManager.CharacterMan
{

    [System.Serializable]
    public class CharacterManager : BaseSubManager
    {
        public static CharacterManager instance;
        public Character[] characters;
        public Object[] charactersObjectLoad;

        public uint index = 0;

        public static Character ActiveCharacter;

        public override void InitializeSubManager()
        {
            instance = (instance == null) ? this : instance;
            LoadCharacters();
        }

        public void LoadCharacters()
        {
            charactersObjectLoad = Resources.LoadAll("Prefabs/Characters", typeof(GameObject));
            characters = new Character[charactersObjectLoad.Length];

            for (int i = 0; i < charactersObjectLoad.Length; i++)
            {

                characters[i] = new Character(charactersObjectLoad[i].name)
                {
                    Effects = new List<BaseEffect>
                {
                    new ScriptEffects.JumpEffect(2.0f),
                    new ScriptEffects.ArmsMovment(2.0f),
                    new ScriptEffects.FlipEffect(2.0f),
                    new ParticleEffect(2.0f, ParticleEffects.ParticleEffectLoad.PuffLoad()),
                    new TrailEffect(2.0f, TrailEffects.TrailEffectLoad.SmokeTrialLoad()),
                },

                    Model = new Model()
                    {
                        //Will fix this ina bit, just getting systems rolling for now,
                        //have to null check because some characters dont have arms and legs (goo)
                        mainObject = charactersObjectLoad[i] as GameObject,
                        Body = (charactersObjectLoad[i] as GameObject).transform.GetChild(0).gameObject,
                        Larm = ((charactersObjectLoad[i] as GameObject).transform.childCount > 1) ? (charactersObjectLoad[i] as GameObject).transform.GetChild(1).gameObject :
                        new GameObject(),
                        Lleg = ((charactersObjectLoad[i] as GameObject).transform.childCount > 1) ? (charactersObjectLoad[i] as GameObject).transform.GetChild(2).gameObject :
                        new GameObject(),
                        Rarm = ((charactersObjectLoad[i] as GameObject).transform.childCount > 1) ? (charactersObjectLoad[i] as GameObject).transform.GetChild(3).gameObject :
                        new GameObject(),
                        Rleg = ((charactersObjectLoad[i] as GameObject).transform.childCount > 1) ? (charactersObjectLoad[i] as GameObject).transform.GetChild(4).gameObject :
                        new GameObject()

                    }
                };

            }

            ActiveCharacter = characters[index];
        }

        public Character GetCurrentActiveCharacter()
        {
            if (characters != null)
                return characters[index];

            return new Character("No Chars");
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

}