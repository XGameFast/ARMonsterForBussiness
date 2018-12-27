using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataStructure  {

}
//玩家基础配置
public class UserBaseConfig
{
    //玩家 = 0  ， 商家 = 1
    public List<int> userTypeList { get; set; }
    public List<int> userTypeName { get; set; }
}
//玩家数据基类
public class UserBaseData
{
    public int userIndex { get; set; }
    public int userType { get; set; }
    public string userName { get; set; }

    public string headImg{get;set;} 
    //玩家所有的宠物的等级之和
    public int userLevel { get; set; }

    public List<MonsterGrowUpAttribute> monsterList { get; set; }

    public List<SkillGrowupAttribute> skillList { get; set; }

    public List<SD_Pag4U> playerObjects { get; set; }


    public string autograph { get; set; }
}
//玩家数据
public class PlayerData :UserBaseData
{
    public List<Exchange> exchangeList { get; set; }
    public List<PlayerStrongHoldGrowUpAttribute> storngholdList { get; set; }
    public List<PlayerCoupon> playerCoupons { get; set; }

}
//商家数据
public class BusinessData : UserBaseData
{
    public List<BusinessStrongholdGrowUpAttribute> strongholdList { get; set; }
    public List<BussinessRewardStruct> businessCoupons { get; set; }
    public List<BusinessActivity> ActiveData { get; set; }
}

public class BussinessRewardStruct
{
    //优惠卷Index
    public int businesscouponIndex { get; set; }
    //商家Index
    public int userIndex { get; set; }
    //所属据点Index
    public int strongholdIndex { get; set; }
    //二维码预留字段
    public string code { get; set; }
    //优惠卷图片链接
    public string image { get; set; }
    //优惠卷标题
    public string title { get; set; }
    //优惠卷描述
    public string description { get; set; }
    //优惠卷状态（0上架1下架2作废）
    public int status { get; set; }
    //开始时间（当开始时间为0时代表无时间限制）
    public int starttime { get; set; }
    //结束时间
    public int endtime { get; set; }
    //持续时间（当持续时间不为0时表示在获得这个优惠卷开始计时一定时间后优惠卷失效）
    public int continueTime { get; set; }
    //图片更新时间
    public int porIsUpdate { get; set; }
    //优惠劵创建时间
    public int createTime { get; set; }
    //奖励掉落概率
    public int rewardDropRate { get; set; }
    //掉落个数 , 这个也用于 合成表里的需要多少个数量合成
    public int rewardDropCount { get; set; }
    //奖励构成，由哪几个奖励合成
    public List<BussinessRewardStruct> rewardcomposeID { get; set; }
}
public class PlayerCoupon
{
    public int playerCouponIndex { get; set; }
    public int businessCouponIndex { get; set; }
    public int count { get; set; }
    //优惠卷状态（0上架1下架2作废）
    public int status { get; set; }
    public int expirationDate { get; set; }
    public int createTime { get; set; }
    public BusinessCouponRequest coupon { get; set; }
}


public class BaiduSearRequest
{
    public int status { get; set; }
    public BaiduSearchResult result { get; set; }

}
public class BaiduSearchResult
{
    public BaiduLocation location {get;set;}
    public int precise{get;set;}
    public int confidence {get;set;}
    public int comprehension {get;set;}
    public string level {get;set;}
}

public class BaiduLocation
{
    public double lng { get; set; }
    public double lat { get; set; }
}
