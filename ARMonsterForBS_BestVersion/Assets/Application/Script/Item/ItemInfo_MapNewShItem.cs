using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfo_MapNewShItem : MonoBehaviour {
    public Image levelBoard;
    public Image icon;
    public Text shName;

    public GameObject yesBtn;
    public GameObject noBtn;

    private System.Action callbackClickComfirm;
    private System.Action callbackClickCancel;


    public void SetInfo(Sprite _lvBoard, Sprite _icon, string _name, System.Action _callbackClickComfirm, System.Action _callbackClickCancel)
    {
        callbackClickComfirm =_callbackClickComfirm;
        callbackClickCancel = _callbackClickCancel;
        levelBoard.sprite = _lvBoard;
        icon.sprite = _icon;
        shName.text = _name;
        OpenChooseButton();
    }

    public void OpenChooseButton()
    {
        yesBtn.gameObject.SetActive(true);
        noBtn.gameObject.SetActive(true);
    }

    public void CloseChooseBtn()
    {
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
    }


    public void ClickComfirm()
    {
        if(callbackClickComfirm!=null)
        {
            callbackClickComfirm();
        }
    }

    public void ClickCancel()
    {
        if(callbackClickCancel!=null)
        {
            callbackClickCancel();
        }
    }

}
