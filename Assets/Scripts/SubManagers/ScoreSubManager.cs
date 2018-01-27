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
        public float multiplier, window;
        public int multiplierincrement;
        public float multiplierShowTime = 0.20f;
        float time, lerpTime;
        Text ScoreText, MultiText;
        bool showText, resetLerpTime;
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
            ScoreText = MenuSubManager.instance.ScoreObject.GetComponent<Text>();
            ScoreText.text = 0.ToString();

            MultiText = MenuSubManager.instance.MultiObject.GetComponent<Text>();
            MultiText.text = 0.ToString();
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

            //Debug.Log("Some SubManager is running a default event (OnGameStart()), needs to !");
        }

        void Update()
        {


            if (window > 0) window -= Time.deltaTime;
            else
            {
                window = 0;
                multiplier = 0;
                //MultiText.text = "";
                multiplierincrement = 0;

            }

            FadeText(MultiText);
        }

        void FadeText(Text text)
        {
            
            //Debug.Log(lerpTime);
            lerpTime += Time.deltaTime;

            if (showText)
            {
                //MultiText.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), lerpTime * 0.70f);

                if (lerpTime > multiplierShowTime)
                {
                    lerpTime = 0;
                    showText = false;
                }
            }
            else
            {
                MultiText.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), lerpTime * 0.70f);

                if(MultiText.color.a < 0.1f)
                {
                    MultiText.transform.localScale = new Vector3(0.2f, .2f, 1);
                }
            }
        }
        public void AddScore(float value)
        {
            window = VariableManager.S_Option.ComboWindow;

            Debug.Log(window);

            if (window > 0)
            {
                MultiText.text = "";
                multiplierincrement += 1;

                Debug.Log((float)multiplierincrement/10.0f);

                if (multiplierincrement > 1) MultiText.text = multiplierincrement.ToString() + " " + "X";
    
                multiplier = VariableManager.S_Option.ScoreMultipler * multiplierincrement;
                MultiText.color = Color.black;
                MultiText.transform.localScale = new Vector3((0.2f + ((float)multiplierincrement/20.0f)),
                (0.2f  + ((float)multiplierincrement/20.0f)), 1);
                showText = true;
            }

            Score += value * multiplier;
            ScoreText.text = Mathf.RoundToInt(Score).ToString();
        }
        //runs on the game end event from the gamemanager
        //use this to end the process of the game
        public override void OnGameEnd()
        {
        }

        //runs on the game reset event from the gamemanager
        //use this to reset the process of the game
        public override void OnGameReset()
        {
        }

    }
}
