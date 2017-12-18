using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SubManager.Menu;

public class DynamicButton : MonoBehaviour {
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => MenuSubManager.instance.OnButtonPress(gameObject.name));  
    }

}
