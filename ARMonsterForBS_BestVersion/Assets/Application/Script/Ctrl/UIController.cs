using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public LoginView  loginView;
    public MapView mapView;

    public WaitBoard waitBoard;
    private void Awake()
    {
        AndaUIManager.Instance.uIController = this;
    }


    public void OpenWatiBoard(bool isOpen)
    {
         Debug.Log("isOpen" +  isOpen);
         waitBoard.gameObject.SetActive(isOpen);
    }


}
