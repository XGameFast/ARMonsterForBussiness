using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GiftItem_ComposeInfo : MonoBehaviour {
   
    public Image rewardIcon; 

    public Text rewardName;

    public GiftEditionBar giftEditionBar;

    private SonCoupon sonCoupon = new SonCoupon();

    public Toggle selectToggle;
    public InputField inpuCount; 

    public void SetInfo(GiftEditionBar _giftEditorBoard ,BussinessRewardStruct data )
    {
        giftEditionBar = _giftEditorBoard;

        AndaDataManager.Instance.GetRewardImg(data.image, SetRewardIcon);

        rewardName.text = data.title;

        sonCoupon.businessCouponIndex = data.businesscouponIndex;

    }

    public void SetRewardIcon(Sprite sprite)
    {
        rewardIcon.sprite =sprite;
    }

    public void SelectItem( )
    {
        bool isSelect = selectToggle.isOn;
        if (isSelect)
        {
            if(giftEditionBar.composeRewads!=null && giftEditionBar.composeRewads.Count>=1)
            {
                AndaUIManager.Instance.PlayTips("最多选择一张其他奖励券进行合成");
                selectToggle.isOn =false;
                return;
            }
            if(inpuCount.text == "")
            {
                AndaUIManager.Instance.PlayTips("请先填入数量");
                selectToggle.isOn = false;
                return;
            }
            int count = int.Parse(inpuCount.text);
            if(count<=0 ) 
            {
                AndaUIManager.Instance.PlayTips("数值不能小于1");
                selectToggle.isOn = false;
                return ;
            }

            if(count >= 10000000)
            {
                AndaUIManager.Instance.PlayTips("数值不能大于9999999");
                selectToggle.isOn = false;
                return;
            }

            sonCoupon.count = count;
            giftEditionBar.AddComposeItem(sonCoupon);
        }
        else
        {
            giftEditionBar.ReduceComposeItem(sonCoupon);
        }
    }


}
