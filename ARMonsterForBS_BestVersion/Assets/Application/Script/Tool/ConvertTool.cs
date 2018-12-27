using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

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


    public static double[] BD09ToGCJ02(double lat, double lon)
    {
        double x = lon - 0.0065, y = lat - 0.006;
        double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
        double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
        double tempLon = z * Math.Cos(theta);
        double tempLat = z * Math.Sin(theta);
        double[] gps = { tempLat, tempLon };
        return gps;
    }

    public static double[] GCJ02ToWGS84(double lat, double lon)
    {
        double[] gps = transform(lat, lon);
        double lontitude = lon * 2 - gps[1];
        double latitude = lat * 2 - gps[0];
        return new double[] { latitude, lontitude };
    }









    public static double pi = 3.1415926535897932384626;
    public static double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
    public static double a = 6378245.0;
    public static double ee = 0.00669342162296594323;

    public static double transformLat(double x, double y)
    {
        double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                         + 0.2 * Math.Sqrt(Math.Abs(x));
        ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
        ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
        ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
        return ret;
    }

    public static double transformLon(double x, double y)
    {
        double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                        * Math.Sqrt(Math.Abs(x));
        ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
        ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
        ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0
                                                                   * pi)) * 2.0 / 3.0;
        return ret;
    }
    public static double[] transform(double lat, double lon)
    {
        if (outOfChina(lat, lon))
        {
            return new double[] { lat, lon };
        }
        double dLat = transformLat(lon - 105.0, lat - 35.0);
        double dLon = transformLon(lon - 105.0, lat - 35.0);
        double radLat = lat / 180.0 * pi;
        double magic = Math.Sin(radLat);
        magic = 1 - ee * magic * magic;
        double sqrtMagic = Math.Sqrt(magic);
        dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
        dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
        double mgLat = lat + dLat;
        double mgLon = lon + dLon;
        return new double[] { mgLat, mgLon };
    }
    public static bool outOfChina(double lat, double lon)
    {
        if (lon < 72.004 || lon > 137.8347)
            return true;
        if (lat < 0.8293 || lat > 55.8271)
            return true;
        return false;
    }

}
