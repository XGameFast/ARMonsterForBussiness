using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GiftItem_Addtion : MonoBehaviour {

    public GameObject kong;
    public GameObject info;

    public Image porImg;
    public Text rewardName;
    public Text rewardDescription;
    public Text dropRate;
    public Text dropCoumt;
    public Text allCount;
    public Toggle addtion;
    public GameObject child;

    public GiftEditionBar giftEditionBar;
    public BussinessRewardStruct bussinessRewardStruct ;

    public Image composeItemImage;//图片
    public Text composeItemName;//名字
    public Text composeItemCount;//需要多少张可合成
    public RewardListView rewardListView;


    public System.Action callbackOpenSelectBar;

    public System.Action<int> callbackClickItem;

    private int itemIndex;
    public bool isKong = false;
    public void OnDisable()
    {
        kong.gameObject.SetActive(false);
        info.gameObject.SetActive(false);
    }

    public void SetItemIndex(int _itemIndex)
    {
        itemIndex = _itemIndex;
    }

    public void SetKong()
    {
        isKong =true;
       //itemIndex = _itemIndex;
        kong.gameObject.SetActive(true);
        info.gameObject.SetActive(false);
    }

    public void SetInfo( BussinessRewardStruct _bussinessRewardStruct)
    {
        isKong =false;
        //itemIndex = _itemIndex;
        kong.gameObject.SetActive(false);
        info.gameObject.SetActive(true);
        bussinessRewardStruct = _bussinessRewardStruct;
        AndaDataManager.Instance.GetRewardImg(_bussinessRewardStruct.image, CallBackGetReward);
        rewardName.text = _bussinessRewardStruct.title;
        rewardDescription.text =_bussinessRewardStruct.description;
        dropRate.text = _bussinessRewardStruct.rewardDropRate+"%";
        dropCoumt.text = _bussinessRewardStruct.rewardDropCount + "张";
        allCount.text = 100 + "张";
        if (_bussinessRewardStruct.rewardcomposeID!=null)
        {
            child.gameObject.SetActive(true);
            BussinessRewardStruct composeReward = AndaDataManager.Instance.mainData.GetRewardData(_bussinessRewardStruct.rewardcomposeID[0].businesscouponIndex);
            composeItemName.text = composeReward.title;
            composeItemCount.text = composeReward.rewardDropCount+"张";
            AndaDataManager.Instance.GetRewardImg(composeReward.image ,(result=>
            {
                composeItemImage.sprite = result;
            }));
        }else
        {
            child.gameObject.SetActive(false);
        }
    }

    public void CallBackGetReward(Sprite sprite)
    {
        porImg.sprite = sprite;
    }

    public void SetIsSelect(bool isSelect)
    {
        addtion.isOn = isSelect;
    }

    public void ClickOpenSelectReward()
    {
        if(callbackClickItem!=null)
        {
            callbackClickItem(itemIndex); 
        }
        callbackOpenSelectBar();
    }

    public void SetItem(BussinessRewardStruct _value)
    {
        SetInfo(_value);
    }
    //删除
    public void ReduceItem()
    {
       /* if(callbackReduceItem!=null)
        {
            callbackReduceItem(bussinessRewardStruct);
        }*/
    }
}
