using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class StrongholdAddRewardBar : MonoBehaviour {

    public Transform rewardGrid;
    public GameObject addtionBtn;
    public GameObject rewardItem;
    public InputField inputStrongholdNickName;
    public Image levelBoard;
    public Image porSprite;
    public GiftEditionBar giftEditionBar;
    public RewardListView rewardListView;
    public Dropdown dropdown;
    public Slider bossPowerSlider;
    public Text bossPowerText;
    public Image bossPor;
    private BusinessStrongholdAttribute businessStrongholdAttribute;
     
    private List<GiftItem_Addtion> rewardList = new List<GiftItem_Addtion>();
    private int currentSelectItemIndex;//当前选择奖励券
    private int currentSelectBussinessRewardStructIndex;//从奖励券池中选择的奖励券index

    private bool isUpdateName;

    private List<MonsterBaseConfig> bossList;

    public void OnDisable()
    {
        if(rewardList!=null)
        {
            int count = rewardList.Count; 
            for(int i = 0 ; i < count; i ++)
            {
                Destroy(rewardList[i].gameObject);
            }
        }
        rewardList.Clear();
        //
        addtionBtn.gameObject.SetActive(false);
    }

    public void BuildBossItem()
    {
        List<int> ids = AndaDataManager.Instance.GetBossList(0);
        int count  = ids.Count;
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        for (int i = 0 ; i < count; i++)
        {
            if(bossList == null) bossList= new List<MonsterBaseConfig>();
            MonsterBaseConfig mbc = AndaDataManager.Instance.GetMonsterBaseConfig(ids[i]);
            bossList.Add(mbc);
            //--
            Dropdown.OptionData optionData =new Dropdown.OptionData();
            optionData.text = mbc.monsterName;
            optionDatas.Add(optionData);
        }
        dropdown.AddOptions(optionDatas);
    }



    public void SetStrongholdInfo(BusinessStrongholdAttribute _businessStrongholdAttribute)
    {

        businessStrongholdAttribute = _businessStrongholdAttribute;
        SetLevelBoard(AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(_businessStrongholdAttribute.strongholdLevel));
        AndaDataManager.Instance.GetUserImg(AndaDataManager.Instance.mainData.playerData.headImg, SetUserImg);
        inputStrongholdNickName.text = _businessStrongholdAttribute.strongholdNickName;
       
        BuildRewardItem();
        BuildBossItem();
    }

    public void SetUserImg(Sprite sprite)
    {
        porSprite.sprite =sprite;

    }
    public void SetLevelBoard(Sprite sprite)
    {
        levelBoard.sprite = sprite;
    }

    public void SelectBoss()
    {
        MonsterBaseConfig bbc = bossList[dropdown.value];
        Sprite bossSp = AndaDataManager.Instance.GetIconSprite(bbc.monsterID);
        bossPor.sprite = bossSp;
        bossPowerSlider.value = 1;
        bossPowerText.text = bbc.monsterBaseBlood.ToString();
    }

    public void BuildRewardItem()
    {
       
        if(rewardList.Count!=0 )
        {
            int rewardListCount = rewardList.Count;
            for(int i = 0 ; i < rewardListCount; i++)
            {
                Destroy(rewardList[i].gameObject);
            }
        }
        rewardList.Clear(); 
        int count = businessStrongholdAttribute.coupons == null? 0 : businessStrongholdAttribute.coupons.Count; 

        for(int i = 0; i < 2; i++)
        {

            if(i < count)
            {
                Debug.Log("new item");
                BussinessRewardStruct bs = AndaDataManager.Instance.mainData.GetRewardData(businessStrongholdAttribute.coupons[i]);
                GameObject item = AndaDataManager.Instance.GetItemInfoPrefab("RewardAddtion");
                item = Instantiate(item);
                item.transform.parent = rewardGrid;
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;
                GiftItem_Addtion giftItem_Addtion = item.GetComponent<GiftItem_Addtion>();
                giftItem_Addtion.SetItemIndex(i);
                giftItem_Addtion.callbackClickItem = SetCurrentSelectItemIndex;
                giftItem_Addtion.SetInfo(bs);
                giftItem_Addtion.callbackOpenSelectBar = OpenRewardView;
                rewardList.Add(giftItem_Addtion);

            }
            else
            {
                Debug.Log("new kong");
                GameObject item = AndaDataManager.Instance.GetItemInfoPrefab("RewardAddtion");
                item = Instantiate(item);
                item.transform.parent = rewardGrid;
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;
                GiftItem_Addtion giftItem_Addtion = item.GetComponent<GiftItem_Addtion>();
                giftItem_Addtion.SetItemIndex(i);
                giftItem_Addtion.callbackClickItem = SetCurrentSelectItemIndex;
                giftItem_Addtion.SetKong();
                giftItem_Addtion.callbackOpenSelectBar = OpenRewardView;
                rewardList.Add(giftItem_Addtion);
               
            }
        }
       
    }

    /// <summary>
    /// 为据点添加奖励
    /// </summary>
    public void AddtionReward(BussinessRewardStruct _data)
    {
        if(businessStrongholdAttribute.coupons == null)
        {
            AndaDataManager.Instance.CallServerInsertStrongholdReward(businessStrongholdAttribute.strongholdIndex, _data.businesscouponIndex, CallBackFinishInsertReward);
        }
        else
        {
            if (businessStrongholdAttribute.coupons.Contains(_data.businesscouponIndex)) return;

            //检查一下这个是位置是否有其他奖励，有的话的要先删除，等回调，更新数据之后，再次添加
            if (rewardList[currentSelectItemIndex].isKong)
            {
                AndaDataManager.Instance.CallServerInsertStrongholdReward(businessStrongholdAttribute.strongholdIndex, _data.businesscouponIndex, CallBackFinishInsertReward);
            }
            else
            {
                currentSelectBussinessRewardStructIndex = _data.businesscouponIndex;
                BussinessRewardStruct br = rewardList[currentSelectItemIndex].bussinessRewardStruct;
                AndaDataManager.Instance.CallServerDeleteStrongholdReward(businessStrongholdAttribute.strongholdIndex, br.businesscouponIndex, AfterDeleteThanUpload);
            }
        }
      



    }

    private void AfterDeleteThanUpload(bool isSuccess,int rewardIndex)
    {
        if(isSuccess)
        {
            businessStrongholdAttribute = AndaDataManager.Instance.mainData.GetAtrongholdAttributes(businessStrongholdAttribute.strongholdIndex);

            AndaDataManager.Instance.CallServerInsertStrongholdReward(businessStrongholdAttribute.strongholdIndex, currentSelectBussinessRewardStructIndex, CallBackFinishInsertReward);
        }
        else
        {
            AndaUIManager.Instance.PlayTips("网络好像有点问题哦，请重试");
        }
    }

    private void CallBackFinishInsertReward(bool isSuccess , int rewardIndex)
    {
        if(isSuccess)
        {
            businessStrongholdAttribute = AndaDataManager.Instance.mainData.GetAtrongholdAttributes(businessStrongholdAttribute.strongholdIndex);

            BussinessRewardStruct bs = AndaDataManager.Instance.mainData.GetRewardData(rewardIndex);

            rewardList[currentSelectItemIndex].SetInfo(bs);

           //BuildRewardItem();
        }
        else
        {
            //将状态回滚
            //rewardList.FirstOrDefault(s => s.bussinessRewardStruct.businesscouponIndex == rewardIndex).SetIsSelect(false);
            AndaUIManager.Instance.PlayTips("网络好像有点问题哦，请重试");
        }
    }

    /// <summary>
    /// 移除奖励
    /// </summary>
    /// <param name="_data">Data.</param>
    public void ReduceReward(BussinessRewardStruct _data)
    {
        if(!businessStrongholdAttribute.coupons.Contains(_data.businesscouponIndex))
        {
            return;
        }
    }

    private void CallBackFinishiRemoveReward(bool isSucess, int rewardIndex)
    {
        if(isSucess)
        {
            businessStrongholdAttribute = AndaDataManager.Instance.mainData.GetAtrongholdAttributes(businessStrongholdAttribute.strongholdIndex);

            AndaDataManager.Instance.mainData.UpdateReduceStrongholdReward(businessStrongholdAttribute.strongholdIndex,rewardIndex);

            rewardList[currentSelectItemIndex].SetKong();//(bs);

          //  BuildRewardItem();
        }
        else
        {
            //将状态回滚
            //rewardList.FirstOrDefault(s => s.bussinessRewardStruct.businesscouponIndex == rewardIndex).SetIsSelect(true);

            AndaUIManager.Instance.PlayTips("网络好像有点问题哦，请重试");
        }
    }

    public void ClickSave()
    {
        if(inputStrongholdNickName.text!=businessStrongholdAttribute.strongholdNickName)
        {
            AndaDataManager.Instance.CallServerUploadStrongholdNickName(inputStrongholdNickName.text,businessStrongholdAttribute.strongholdIndex, CallBackUploadStrongholdInfo);
        }else
        {
            ClickClose();
        }
    }

    public void ClickClose()
    {
        gameObject.SetActive(false);
    }

    public void CallBackUploadStrongholdInfo(bool isSuccess)
    {
        if(isSuccess)
        {
            ClickClose();
        }
        else
        {
            AndaUIManager.Instance.PlayTips("网络好像有点问题哦，请重试");
        }
    }

   

    //---
    public void OpenEditorGiftBar()
    {
       /*if(AndaDataManager.Instance.mainData.strongholdRewardCardList.Count<=0)
        { 
            AndaUIManager.Instance.PlayTips("没有可用的空白编辑券");
            return;
        }
        //打开编辑面板*/
        giftEditionBar.gameObject.SetActive(true);
    }
    //---
    public void FinishUploadReward()
    {

    }


    public void OpenRewardView()
    {

        rewardListView.gameObject.SetActive(true);
        rewardListView.BuildRewardList(AddtionReward);
    }

    public void SetCurrentSelectItemIndex(int _index)
    {
        currentSelectItemIndex = _index;
    }
}