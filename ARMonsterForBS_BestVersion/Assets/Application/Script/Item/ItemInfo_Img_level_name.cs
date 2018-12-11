using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfo_Img_level_name : MonoBehaviour {

    public Image image;
    public Image levelboard;
    public Text text;
    public int index;
    public System.Action<int> callback;
    public void SetInfo(int _index, Sprite sprite, Sprite _levelBoard, string t)
    {
        index = _index;
        image.sprite = sprite;
        levelboard.sprite = _levelBoard;
        text.text = t;
    }

    public void ClickCallback()
    {
        if (callback != null)
        {
            callback(index);
        }
    }
}
