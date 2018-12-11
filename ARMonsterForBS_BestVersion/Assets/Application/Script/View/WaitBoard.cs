using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBoard : MonoBehaviour {

    public Transform circleItem;
	



	// Update is called once per frame
	void Update () {

        circleItem.Rotate(0,0,-10f);

    }
}
