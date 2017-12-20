using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Panel_Manager : MonoBehaviour {

    public string text;
    public Color PanelColor;
    public Color PanelShadowColor;
    public Color PanelOutlineColor;
    public Color textColor;
    public bool showCorners;


    GameObject panelBits;
    GameObject panelBitsShadow;
    GameObject panelText;
    GameObject panelOutline;

	// Use this for initialization
	void Start () {
        panelBits = this.gameObject.transform.GetChild(1).gameObject;
        panelBitsShadow = this.gameObject.transform.GetChild(0).gameObject;
        panelOutline = this.gameObject.transform.GetChild(2).gameObject;
        panelText = this.gameObject.transform.GetChild(3).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnValidate()
    {
        if (panelBits != null)
        {
            foreach (Image img in panelBits.GetComponentsInChildren<Image>())
            {
                img.color = PanelColor;
            }
        }

        if (panelBitsShadow != null)
        {
            foreach (Image img in panelBitsShadow.GetComponentsInChildren<Image>())
            {
                img.color = PanelShadowColor;
            }
        }

        if (panelOutline != null)
        {
            foreach (Image img in panelOutline.GetComponentsInChildren<Image>())
            {
                img.color = PanelOutlineColor;
            }
        }


        if (panelText != null) { panelText.GetComponent<Text>().text = text; panelText.GetComponent<Text>().color = textColor; }

        if (panelOutline != null) panelOutline.gameObject.SetActive(showCorners);
        


    }
}
