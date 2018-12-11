using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MainData  {

    public BusinessData playerData ;
    public string token;

    public List<BusinessStrongholdAttribute> businessStrongholdAttributes;
    public List<LD_Objs> strongholdDrawingList = new List<LD_Objs>();
    public List<LD_Objs> strongholdRewardCardList = new List<LD_Objs>();
    public List<BussinessRewardStruct> bussinessReward;
    public Sprite imgPor;


    public System.Action<BusinessStrongholdAttribute> UpdateStrongholdDataEvent;
    public void SetUserInformation(BusinessData _playerData , string _token)
    {
        token =_token;
        playerData = _playerData;
        bussinessReward = playerData.businessCoupons;
        BuildBussinessStrongholdAttrubte();
        BuildStrongholdDawingList();
        GetImg();
    }

    public void BuildBossListConfig()
    {

    }

    #region 向服务器所要头像数据

    private void GetImg()
    {
        AndaDataManager.Instance.CallServerGetUserImg(playerData.headImg, CallBackGetImg);
    }

    private void CallBackGetImg(Sprite _value)
    {
        imgPor = _value;
    }

    #endregion
    #region 构造据点数据
    public void BuildBussinessStrongholdAttrubte()
    {
        int count = playerData.strongholdList.Count;
        for(int i = 0 ; i < count; i++)
        {
            if(businessStrongholdAttributes == null)
            {
                businessStrongholdAttributes = new List<BusinessStrongholdAttribute>();
            }
            BusinessStrongholdAttribute _b = ConvertTool.ConvertToBussinessStrongholdAttribute(playerData.strongholdList[i]);
            businessStrongholdAttributes.Add(_b);
    
        }
    }
    #endregion
    #region 添加据点
    public void AddBussinessStronghold(BusinessStrongholdAttribute business)
    {
        if(businessStrongholdAttributes == null)businessStrongholdAttributes = new List<BusinessStrongholdAttribute>(); 
        businessStrongholdAttributes.Add(business);
    }
    #endregion

    #region 添加奖励券

    public void AddBussinessRewar(BussinessRewardStruct br )
    {
        if(bussinessReward == null) bussinessReward = new List<BussinessRewardStruct>();
        bussinessReward.Add(br);
    }

    #endregion

    #region 构造物品数据
    public void BuildStrongholdDawingList()
    {
        int count = playerData.playerObjects.Count;
        for(int i = 0; i < count; i++)
        {
            int idType = AndaDataManager.Instance.GetObjTypeID(playerData.playerObjects[i].objectID);
            Debug.Log("idType" +idType);
            if(idType == 42010)
            {
                LD_Objs lD_Objs = ConvertTool.ConvertToLd_objs(playerData.playerObjects[i]);
                strongholdDrawingList.Add(lD_Objs);
            }else if(idType == 43000)
            {
                LD_Objs lD_Objs = ConvertTool.ConvertToLd_objs(playerData.playerObjects[i]);
                if(playerData.playerObjects[i].objectID == 43004)
                {
                    strongholdRewardCardList.Add(lD_Objs);
                }
            }
        }
    }
    #endregion

    #region 获取据点数据

    public BusinessStrongholdAttribute GetAtrongholdAttributes(int _index)
    {
        return businessStrongholdAttributes.FirstOrDefault(s=>s.strongholdIndex == _index);
    }

    public void GetAllStrongholdAttributes()
    {

    }

    public List<LD_Objs> GetMineStrongholdDrawing()
    {
        return strongholdDrawingList;
    }
    #endregion

    #region 获取奖励数据

    public BussinessRewardStruct GetRewardData(int _index)
    {
        if(bussinessReward == null)return null;
        return bussinessReward.FirstOrDefault(s=>s.businesscouponIndex == _index);
    }

    #endregion


    #region 更新据点奖励信息

    public void UpdateAddStrongholdReward(int shIndex,int rewardIndex)
    {
        BusinessStrongholdAttribute bs = businessStrongholdAttributes.FirstOrDefault(s => s.strongholdIndex == shIndex);
        Debug.Log("AddBefore" + bs.coupons.Count);
        bs.coupons.Add(rewardIndex);
        Debug.Log("AfterAdd" + bs.coupons.Count);
        //businessStrongholdAttributes.FirstOrDefault(s=>s.strongholdIndex == shIndex).coupons.Add(rewardIndex);
    }

    public void UpdateReduceStrongholdReward(int shIndex,int rewardIndex)
    {
        businessStrongholdAttributes.FirstOrDefault(s => s.strongholdIndex == shIndex).coupons.Remove(rewardIndex);
    }

    public void UpdateStrongholdNickName(int shIndex , string nickName)
    {
        businessStrongholdAttributes.FirstOrDefault(s => s.strongholdIndex == shIndex).strongholdNickName = nickName;
        if(UpdateStrongholdDataEvent!=null)
        {
            UpdateStrongholdDataEvent(businessStrongholdAttributes.FirstOrDefault(s => s.strongholdIndex == shIndex));
        }
    }

    #endregion
}
