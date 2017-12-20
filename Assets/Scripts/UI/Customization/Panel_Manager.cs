using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Panel_Manager : MonoBehaviour {

    public string text;
    public Color PanelColor;
    public Color PanelShadowColor;


    GameObject panelBits;
    GameObject panelBitsShadow;
    GameObject panelText;

	// Use this for initialization
	void Start () {
        panelBits = this.gameObject.transform.GetChild(1).gameObject;
        panelBitsShadow = this.gameObject.transform.GetChild(0).gameObject;
        panelText = this.gameObject.transform.GetChild(2).gameObject;
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

            foreach (Image img in panelBitsShadow.GetComponentsInChildren<Image>())
            {
                img.color = PanelShadowColor;
            }

            panelText.GetComponent<Text>().text = text;
        }
    }
}
