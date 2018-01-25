using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubManager;
using SubManager.Menu;
using UnityEngine.UI;

namespace SubManager.Score
{
    public class ScoreSubManager : BaseSubManager
    {
		public static ScoreSubManager instance;
        public static float Score;
		float time;
		Text ScoreText;
        public override void InitializeSubManager()
        {
			instance = (instance == null) ? this : instance;

            thisSubType = GameManager.GameSubManagerTypes.None;
            Debug.Log(thisSubType.ToString() + " is not overriding the InitializeSubManager() method.");
        }

        //runs on the post init event from the gamemanager
        //use this to start communicating with other subManagers
        public override void OnPostInit()
        {
            Debug.Log("Some SubManager is running a default event (OnPostInit()), needs to !");
        }

        //runs on the game load event from the gamemanager
        //use this to begin the setup of the game
        public override void OnGameLoad()
        {
            Debug.Log("Some SubManager is running a default event (OnGameLoad()), needs to !");
        }

        //runs on the game start event from the gamemanager
        //use this to begin the process of the game
        public override void OnGameStart()
        {
			ScoreText = MenuSubManager.instance.ScoreObject.GetComponent<Text>();
			ScoreText.text = 0.ToString();
            //Debug.Log("Some SubManager is running a default event (OnGameStart()), needs to !");
        }

		void Update()
		{
			time += Time.deltaTime;
		}
		public void AddScore(float value)
		{
			time = 0;

			if(time > VariableManager.S_Option.ComboWindow)
			
			Score += value;
			ScoreText.text = Score.ToString();
		}
        //runs on the game end event from the gamemanager
        //use this to end the process of the game
        public override void OnGameEnd()
        {
            Debug.Log("Some SubManager is running a default event (OnGameEnd()), needs to !");
        }

        //runs on the game reset event from the gamemanager
        //use this to reset the process of the game
        public override void OnGameReset()
        {
            Debug.Log("Some SubManager is running a default event (OnGameReset()), needs to !");
        }

    }
}
