using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class ButtonController : BaseSubManager, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    public Button button;
    Color ButtonBaseColor { get; set; }
    Color ButtonInnerColor { get; set; }
    Color ButtonOuterColor { get; set; }

    public Color buttonbaseColor;
    public Color buttoninnerColor;
    public Color buttonouterColor;

    List<Image> Outlines = new List<Image>();
    List<Image> InnerLines = new List<Image>();
    List<Image> Body = new List<Image>();
    float time;
    bool HandleMouse, HandleClick;

    public override void InitializeSubManager()
    {
    }

    public override void OnPostInit()
    {
    }

    //spawn the player on the platforms
    public override void OnGameLoad()
    {
    }

    //begin input detection
    public override void OnGameStart()
    {          
    }

    //player dies, this runs after
    public override void OnGameEnd()
    {
    }

    //starting positions everyone!
    public override void OnGameReset()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //time = 0;
        //HandleClick = true;
        //Debug.Log("In Button");
        //StartCoroutine(Snoop());
        Debug.Log("Just Clicking");

        if(HandleMouse)
        {
            Debug.Log("Just Clicking");
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        time = 0;
        HandleClick = true;
        Debug.Log("In Button");
        StartCoroutine(Snoop());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        time = 0;
        HandleClick = true;
        Debug.Log("In Button");
        StartCoroutine(Snoop());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HandleClick)
        {
            time = 0;
            HandleMouse = true;
            Debug.Log("In Button");
            StartCoroutine(Snoop());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            time = 0;
            Debug.Log("Out Button");
            HandleMouse = true;
            StartCoroutine(Snoop());
    }


   IEnumerator Snoop()
   {

        while (HandleMouse || HandleClick)
        {
            time += Time.deltaTime;
            Debug.Log("I Shouldnt be Spamming");
            ImageColorManage();

            if (time > 0.1f)
            {
                if(HandleMouse) HandleMouse = false;
                if(HandleClick) HandleClick = false;
            }
            

            yield return null;
        }
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

    void ImageColorManage()
    {
            foreach(Image img in Outlines)

                img.color = button.targetGraphic.canvasRenderer.GetColor() * buttonouterColor;

            foreach (Image img in InnerLines)
                img.color = button.targetGraphic.canvasRenderer.GetColor() * buttoninnerColor;

            foreach (Image img in Body)
                img.color = button.targetGraphic.canvasRenderer.GetColor() * buttonbaseColor;
            
    }

   // public

    void OnValidate()
    {

        if (ButtonBaseColor != buttonbaseColor || ButtonInnerColor != buttoninnerColor || ButtonOuterColor != buttonouterColor)
        {
            if (button == null) button = GetComponent<Button>();
            if (button != null) button.targetGraphic.GetComponent<Image>().color = buttonbaseColor;

            if (Body.Count <= 0)
            {
                ParseNames();
            }
            else
            {
                ImageColorManage();
            }
        }

        ButtonBaseColor  = (ButtonBaseColor != buttonbaseColor) ? buttonbaseColor : ButtonBaseColor;
        ButtonInnerColor = (ButtonInnerColor != buttoninnerColor) ? buttoninnerColor : ButtonInnerColor; 
        ButtonOuterColor = (ButtonOuterColor != buttonouterColor) ? buttonouterColor : ButtonOuterColor;




    }

}
