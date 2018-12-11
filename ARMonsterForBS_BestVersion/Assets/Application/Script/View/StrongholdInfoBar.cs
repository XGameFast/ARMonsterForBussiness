using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StrongholdInfoBar : ViewBasic {

    public Button comfirmBtn;
    public InputField inputField;
    public Image levelBoard ;
    public Image porSprite;
    public Transform rewardGrid;
    public GiftEditionBar giftEditionBar;

    private byte[] imageByte;

    private System.Action callbackClickCancel;
    private System.Action<string , byte[] > callbackClickSave;

    public GameObject addtionItem;
    private List<GameObject> rewardslist = new List<GameObject>();
    private bool canJumpTips =false;
    private bool wasSave =false;
    #region 清除奖励列表

    private void ClearReardList()
    {
        int count = rewardslist.Count;
        for(int i = 0 ; i < count; i++)
        {
            Destroy(rewardslist[i].gameObject);
        }
    }

    #endregion

    public void OnEnable()
    {
        SetComfirmBtnState(false);
        canJumpTips =false;
        wasSave = false;
        imageByte = null;
    }

    public override void StartView()
    {
        base.StartView();
        gameObject.SetActive(true);
        FadeIn();
    }

    public void RegisterCallback(System.Action<string,byte[]> _callbackClickComfirm,System.Action _callbackClickCancel)
    {
        callbackClickSave = _callbackClickComfirm;
        callbackClickCancel = _callbackClickCancel;
    }

    public void SetLevelBoard(Sprite sprite)
    {
        levelBoard.sprite = sprite;
    }

    public void SetStrongholdInfo(Sprite porIcon,Sprite levelboard,string strongholdname)
    {
        SetLevelBoard(levelboard);
        porSprite.sprite = porIcon;
        inputField.text = strongholdname;
    }

    #region 新建的据点会走这个接口

    public void BuildRewardListWithNewStronghold()
    {
        GameObject _item = Instantiate(addtionItem);
        _item.gameObject.SetActive(true);
        _item.transform.parent = rewardGrid.transform;
        rewardslist.Add(_item);
    }

    #endregion



    #region 更新名字

    public void OnChangeName()
    {
        if(inputField.text == "" || inputField.text.Length > 12 || porSprite.sprite.name == "addtion")
        {
            Debug.Log("inputField.text" + inputField.text);

            Debug.Log("inputField.text.Length" + inputField.text.Length);

            Debug.Log("porSprite.sprite.name" + porSprite.sprite.name);
            SetComfirmBtnState(false);
        }
        else
        {
            SetComfirmBtnState(true);
        }
    }

    #endregion


    #region 点击设置头像


    public void SelectPorTexture()
    {
        AndaDataManager.Instance.ReadTexture2DFromPC(CallBackSelectTexture);
    }

    private void CallBackSelectTexture(Texture2D value)
    {
        if(value.height > 128 || value.width > 128)
        {
            AndaUIManager.Instance.PlayTips("请保存选择的图像为128大小");
            Destroy(value);
            return;
        }
        Texture2D newT2d = ConvertTool.ConvertToTexture2d(value);
        porSprite.sprite = ConvertTool.ConvertToSpriteWithTexture2d(newT2d);

        imageByte = newT2d.EncodeToPNG();
    }

    #endregion


    #region 点击了保存
    #endregion
    public void ClickConfirmSave()
    {
        if(wasSave)return;
        FadeOut();
        if (callbackClickSave!=null)
        {
            callbackClickSave(inputField.text,imageByte);
        }

        //ClearReardList();
    }
    #region 点击了取消
    public void ClickCancelSave()
    {

        if(rewardslist.Count<=1)
        {
            if(!canJumpTips) 
            {
                AndaUIManager.Instance.PlayTips("请编辑奖励，没有奖励的据点将不会被玩家看到，如果您执意退出，请再次点击关闭");
                canJumpTips = true;
            }
           
            return;
        }
        FadeOut();
        ClearReardList();
        if (callbackClickCancel!=null)
        {
            callbackClickCancel();
        }

    }
    #endregion

    #region 点击了新建奖励

    public void ClickAddNewReward()
    {
        if(!wasSave)
        {
            AndaUIManager.Instance.PlayTips("请先保存据点，随后即可编辑奖励");
            return;
        }
        giftEditionBar.StartView();

    }

    #endregion


    #region 让保存按钮失效
    public void SetSaveBtnDisable()
    {
        SetComfirmBtnState(false);

          wasSave = true;
    }
    #endregion

    public void SetComfirmBtnState(bool isOpen)
    {
        comfirmBtn.enabled = isOpen;
    }
}
