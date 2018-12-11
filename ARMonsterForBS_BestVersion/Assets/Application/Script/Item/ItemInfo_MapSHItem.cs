using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfo_MapSHItem : MonoBehaviour {

	
    public Image levelBoard;
    public Image icon;
    public Text shName;


    public void SetInfo(Sprite _lvBoard, Sprite _icon , string _name)
    {
        levelBoard.sprite = _lvBoard;
        icon.sprite =_icon;
        shName.text = _name;
    }
}
