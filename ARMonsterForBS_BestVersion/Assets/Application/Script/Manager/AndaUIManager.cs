using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndaUIManager {

    private static AndaUIManager _instance = null;

    public static AndaUIManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = new AndaUIManager();
            }
            return _instance;
        }
    }

    public UIController uIController;


    public void OpenWaitBoard(bool state)
    {
        uIController.OpenWatiBoard(state);
    }

    public void PlayTips(string tipsCountent)
    {
        Debug.Log("Tips:" + tipsCountent);
    }
}
