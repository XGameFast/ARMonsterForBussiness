using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public static class ConvertTool  {

    public static BusinessStrongholdAttribute ConvertToBussinessStrongholdAttribute(BusinessStrongholdGrowUpAttribute bsga)
    {
        BusinessStrongholdAttribute tmp = new BusinessStrongholdAttribute();
        tmp.hostIndex = bsga.hostIndex;
        tmp.strongholdIndex =bsga.strongholdIndex;
        tmp.strongholdNickName = bsga.strongholdNickName;
        tmp.hostType =  bsga.hostType;
        tmp.strongholdID = bsga.strongholdID;
        tmp.strongholdGloryValue = bsga.strongholdGloryValue;
        tmp.hostNickName = AndaDataManager.Instance.mainData.playerData.userName;
        tmp.coupons = bsga.coupons;
        tmp.fightMonsterListIndex = bsga.strongholdFightMonsterList;
        tmp.strongholdPosition = bsga.strongholdPosition;
         
        return tmp;
    }

    public static PlayerStrongholdAttribute ConvertToPlayerstrongholdAttribute(PlayerStrongHoldGrowUpAttribute info)
    {
        PlayerStrongholdAttribute playerStrongholdAttribute = new PlayerStrongholdAttribute();
        playerStrongholdAttribute.strongholdIndex = info.strongholdIndex;
        playerStrongholdAttribute.medalLevel = info.playerStrongholdMedalLevel;
        playerStrongholdAttribute.strongholdNickName = info.strongholdNickName;
        playerStrongholdAttribute.strongholdPosition = info.strongholdPosition;
        playerStrongholdAttribute.coupons = info.coupons;
        return playerStrongholdAttribute;

    }

    public static LD_Objs ConvertToLd_objs(SD_Pag4U sD_Pag4U)
    {
        LD_Objs lD_Objs = new LD_Objs();
        lD_Objs.objID = sD_Pag4U.objectID;
        lD_Objs.objectType = AndaDataManager.Instance.GetObjTypeID(lD_Objs.objID);
        lD_Objs.objSmallID = lD_Objs.objID - lD_Objs.objectType;
        CD_ObjAttr cD_ObjAttr = AndaDataManager.Instance.objectsList.FirstOrDefault(s=>s.objectID == lD_Objs.objectType);
        lD_Objs.objIndex = sD_Pag4U.objectIndex;
        lD_Objs.objName = cD_ObjAttr.objectName[lD_Objs.objSmallID];
        lD_Objs.lessCount = sD_Pag4U.objectCount;
        lD_Objs.giveValue = sD_Pag4U.objectValue;
        lD_Objs.objDescription = cD_ObjAttr.objectDescription[lD_Objs.objSmallID];
        return lD_Objs;
    }



    public static Texture2D ConvertToTexture2d(Texture2D value)
    {
        Texture2D newT2d = new Texture2D(value.height, value.height);
        newT2d.SetPixels(value.GetPixels());
        newT2d.Apply(true);
        newT2d.filterMode = FilterMode.Trilinear;
        return newT2d;
    }

    public static Sprite ConvertToSpriteWithTexture2d(Texture2D texture2)
    {
        return  Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height),
                                         new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// 本地图片文件转Base64字符串
    /// </summary>
    /// <param name="imagepath">本地文件路径</param>
    /// <returns>Base64String</returns>
    public static string bytesToString(byte[] bts)
    {
        return System.Convert.ToBase64String(bts);
    }

    public static byte[] StringToBytes(string str)
    {
        return System.Text.Encoding.Default.GetBytes(str);
    }

}
