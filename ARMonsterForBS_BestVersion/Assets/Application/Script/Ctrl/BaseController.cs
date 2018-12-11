using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    protected  MainContoller mainContoller;


    public void SetMainController(MainContoller _mainController)
    {
        mainContoller = _mainController;
    }

    public virtual void StartCtrl()
    {

    }

    public virtual void EndCtrl()
    {

    }

}
