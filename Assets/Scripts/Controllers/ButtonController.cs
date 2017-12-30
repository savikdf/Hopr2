using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;

public class ButtonController : BaseSubManager {

    Button button;
    public Color ButtonBaseColor;
    public Color ButtonInnerColor;
    public Color ButtonOuterColor;

    public List<Image> Outlines = new List<Image>();
    public List<Image> InnerLines = new List<Image>();
    public List<Image> Body = new List<Image>();


    public override void InitializeSubManager()
    {
    }

    public override void OnPostInit()
    {
        button = GetComponent<Button>();

    }

    //spawn the player on the platforms
    public override void OnGameLoad()
    {

    }

    //begin input detection
    public override void OnGameStart()
    {
        button.targetGraphic.color = ButtonBaseColor;

        ParseNames();
        StartCoroutine(ImageColorManage());

    }

    //player dies, this runs after
    public override void OnGameEnd()
    {

    }

    //starting positions everyone!
    public override void OnGameReset()
    {
    }

    public void ParseNames()
    {

        Image[] images = new Image[GetComponentsInChildren<Image>().Length];
        images = GetComponentsInChildren<Image>();

        for (int i = 0; i < images.Length; i++)
        {

            if(images[i].gameObject.name.Contains("_0"))
            {
                Outlines.Add(images[i]);
                continue;
            }
            else if(images[i].gameObject.name.Contains("_I"))
            {
                InnerLines.Add(images[i]);
                continue;
            }


            if (images[i] == button.targetGraphic)
                continue;

            Body.Add(images[i]);
        }
    }

    IEnumerator ImageColorManage()
    {
        while(GameManager.instance.currentGameState == GameManager.GameStates.Intra)
        {
            foreach(Image img in Outlines)
                img.color = button.targetGraphic.canvasRenderer.GetColor() * ButtonOuterColor;

            foreach (Image img in InnerLines)
                img.color = button.targetGraphic.canvasRenderer.GetColor() * ButtonInnerColor;

            foreach (Image img in Body)
                img.color = button.targetGraphic.canvasRenderer.GetColor() * ButtonBaseColor;


            yield return null;
        }
    }

   // public

}
