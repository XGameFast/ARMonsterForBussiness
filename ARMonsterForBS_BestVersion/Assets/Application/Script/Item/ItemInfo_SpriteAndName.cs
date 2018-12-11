using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfo_SpriteAndName : MonoBehaviour {

    public Image image;
    public Text text;
    public int index;
    public System.Action<int> callback;
    public void SetInfo(int _index, Sprite sprite , string t)
    {
        index = _index;
        image.sprite = sprite;
        text.text = t;
    }

    public void ClickCallback()
    {
        if(callback!=null)
        {
            callback(index);
        }
    }
}
