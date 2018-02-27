using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager.Inputs;
using SubManager.Physics;

public class ChargerController : MonoBehaviour {

	public Sprite[] charges;
	public Image image;

	public Sprite Blank;
	// Use this for initialization
	void Start () {
		
	}
	

	// Update is called once per frame
	void Update () 
	{
		
		if(!InputSubManager.instance.MainDragging)
		{
			//Debug.Log("Why are you not running??");
			image.sprite = Blank;
		}
		else
		{
			SetCharge();
		}
	}

	public void SetCharge()
	{
		float val = InputSubManager.instance.GetDistance() * VariableManager.P_Options.force ;
		float normal = Utils.Norm(val, 0, VariableManager.P_Options.cap);
		
		if((int)Mathf.Round((normal) * 7) < 7)
		image.sprite = charges[(int)Mathf.Round((normal) * 7)];
	}
}
