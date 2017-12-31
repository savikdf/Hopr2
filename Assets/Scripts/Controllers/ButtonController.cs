using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class ButtonController : BaseSubManager, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public bool DebugLogs;

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
    float time, innerTime, exitTime;
    bool HandleMouse, HandleClick, Exit, Click, HandleInnerClick, EndInnerClick;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        innerTime = 0;
        HandleInnerClick = true;
        EndInnerClick = false;
        StartCoroutine(SnoopClick());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        innerTime = 0;
        EndInnerClick = true;
        Debug.Log("Button Up");

        time = 0;
        HandleMouse = true;
        StartCoroutine(Snoop());
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        HandleClick = true;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        time = 0;
        Exit = false;
        HandleMouse = true;
        //Debug.Log("In Button");
        StartCoroutine(Snoop());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exitTime = 0;
        Exit = true;
        StartCoroutine(SnoopExit());
    }

    IEnumerator SnoopExit()
    {
        while (Exit)
        {
            exitTime += Time.deltaTime;
            Log("Snoop Exit is Active");
            ImageColorManage();
            //if We Click Again Outside of the Button
            if (Input.GetMouseButton(0))
            {
                time = 0;
                Click = true;
                StartCoroutine(SnoopExitClick());
            }
            

            yield return null;
        }
    }

    IEnumerator SnoopExitClick()
    {
        while (Click)
        {
            time += Time.deltaTime;
            Log("Snoop Exit Click is Active");

            ImageColorManage();

           if (time > 0.2f)
           {
               Exit = false;
               Click = false;
           }
              
            yield return null;
        }
    }

    IEnumerator SnoopClick()
    {
        while (HandleInnerClick)
        {
            innerTime += Time.deltaTime;

            Log("Snoop Click is Active");



            ImageColorManage();

            if (EndInnerClick)
            {
                if (innerTime > 0.1f)
                {
                    EndInnerClick = false;
                    HandleInnerClick = false;
                }
            }

            yield return null;
        }
    }

    IEnumerator Snoop()
    {

        while (HandleMouse)
        {
            time += Time.deltaTime;
            Log("Snoopin");
            ImageColorManage();

            if (time > 0.1f)
            {
                if(HandleMouse) HandleMouse = false;
            }
            

            yield return null;
        }
   }


    public void Log(string log)
    {
        if (DebugLogs) Debug.Log(log);
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
