using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager.Inputs;

public class ChargerController : MonoBehaviour {

	public Sprite[] charges;
	public Image image;

	public Sprite Blank;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
		if(!InputSubManager.instance.MainDragging)
		{
			Debug.Log("Why are you not running??");
			//image.sprite = Blank;
		}
		else
		{
			SetCharge();
		}
	}
	
	public void SetCharge()
	{
		if(InputSubManager.instance.GetDistance() < VariableManager.P_Options.cap)
		image.sprite = charges[(int)(Mathf.Round(InputSubManager.instance.GetDistance()) % 7)];
	}
}
