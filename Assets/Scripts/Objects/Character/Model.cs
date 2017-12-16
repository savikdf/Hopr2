using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour {

    public GameObject Body { get; set; }

    public GameObject Larm { get; set; }
    public GameObject Rarm { get; set; }
    public GameObject Lleg { get; set; }
    public GameObject Rleg { get; set; }


    public Model(
        GameObject _body, 
        GameObject _larm = null,
        GameObject _rarm = null,
        GameObject _lleg = null, 
        GameObject _rleg = null)
    {
        Body = _body;
        Larm = _larm;
        Rarm = _rarm;
        Lleg = _lleg;
        Rleg = _rleg;
    }
}
