using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class NetworkController : MonoBehaviour {
    public string networkAdress3 = "http://106.14.16.150:8085/ConfigTxt/";
    public string networkAdress4 = "http://106.14.16.150:8085/";
    public string networkAdress2 = "http://106.14.16.150:8085/api/";
    private void Awake()
    { 
        AndaDataManager.Instance.networkController = this;
    }

    #region 登录
    public void Login(System.Action<bool> callback, string name)
    {
        var _wForm = new WWWForm();
        _wForm.AddField("acc", name);
        _wForm.AddField("pwd", "000000");
        string path = "http://106.14.16.150:8085/api/Login/login";
        //string path = "http://localhost:57789/api/Login/Login";
        StartCoroutine(ExcuteLogin(path, _wForm, callback));
    }

    private IEnumerator ExcuteLogin(string _url, WWWForm _wForm, System.Action<bool> callBack)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;

        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            var data = JsonMapper.ToObject<PlayerLogin>(postData.text);
            Debug.Log(postData.text);
            if (data.code == "200")
            {
                AndaDataManager.Instance.SetUserData(data.BusinessData,data.token);
                //AndaDataManager.Instance.SetUserData(data.PlayerData);
                //AndaDataManager.Instance.SetUserToken(data.token);
            }
            callBack(data.code == "200");
        }
        AndaUIManager.Instance.OpenWaitBoard(false);
    }

    #endregion


    #region 从服务获取配置文件

    public void GetBaseConfig(System.Action<ConfigBase> callback)
    {
        StartCoroutine(ExcuteGetBaseConfig(callback));
    }

    private IEnumerator ExcuteGetBaseConfig(System.Action<ConfigBase> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW configStr = new WWW(networkAdress3 + "ConfigBase.txt");
        yield return configStr;
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (string.IsNullOrEmpty(configStr.error))
        {
            callback(JsonMapper.ToObject<ConfigBase>(configStr.text));
            //Debug.Log("configStr.text" + configStr.text);
        }
        else
        {
            // Debug.Log("configStr.text is null" + configStr.text);
        }
      
    }


    public void GetConfigFils(List<ConfigFile> files, System.Action<List<LocalConfigFile>> callback)
    {
        StartCoroutine(ExcuteGetConfigfiles(files, callback));
    }

    private IEnumerator ExcuteGetConfigfiles(List<ConfigFile> files, System.Action<List<LocalConfigFile>> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        int count = files.Count;
        List<LocalConfigFile> localConfigs = new List<LocalConfigFile>();
        for (int i = 0; i < count; i++)
        {
            string _name = files[i].name;
            int version = files[i].lastWriteTime;
            WWW link = new WWW(networkAdress3 + _name);
            yield return link;
            if (string.IsNullOrEmpty(link.error))
            {
                localConfigs.Add(new LocalConfigFile
                {
                    name = _name,
                    lastWriteTime = version,
                    config = link.text
                });
            }
        }
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (localConfigs.Count != 0)
        {
            callback(localConfigs);
        }
      //  AndaUIManager.Instance.OpenWaitBoard(false);
    }

    #endregion

    #region 获取周围的数据
    public void GetCurrentLocationRangeOtherData(List<double> location, System.Action<List<PlayerStrongholdAttribute>, List<BusinessStrongholdAttribute>> callback)
    {
        var _wForm = new WWWForm();

        _wForm.AddField("positionx", location[1].ToString());
        _wForm.AddField("positiony", location[0].ToString());

        StartCoroutine(GetCurrentLocationRangeOtherData("http://106.14.16.150:8085/api/Region/GetRegion", _wForm, callback));
    }

    private IEnumerator GetCurrentLocationRangeOtherData(string _url, WWWForm _wForm, System.Action<List<PlayerStrongholdAttribute>, List<BusinessStrongholdAttribute>> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;

        AndaUIManager.Instance.OpenWaitBoard(false);
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {

            var data = JsonMapper.ToObject<Region>(postData.text);

            List<PlayerStrongHoldGrowUpAttribute> tmpPlayerStronghold = data.resRegion.PlayerStrongHoldlist;


            List<BusinessStrongholdGrowUpAttribute> tmpBusinessStronghold = data.resRegion.BusinessStrongHoldlist;

            List<PlayerStrongholdAttribute> playerStrongholds = new List<PlayerStrongholdAttribute>();
            List<BusinessStrongholdAttribute> businessStrongholds = new List<BusinessStrongholdAttribute>();

            foreach (var go in tmpPlayerStronghold)
            {
                PlayerStrongholdAttribute psa = ConvertTool.ConvertToPlayerstrongholdAttribute(go);
                //Debug.Log("go.strongholdGloryValue" + go.strongholdGloryValue);
                playerStrongholds.Add(psa);
            }

            foreach (var go in tmpBusinessStronghold)
            {
                BusinessStrongholdAttribute bsa = ConvertTool.ConvertToBussinessStrongholdAttribute(go);
                bsa.strongholdID = 30005;
                businessStrongholds.Add(bsa);
            }
            callback(playerStrongholds, businessStrongholds);

        }
    }
    #endregion


    #region 放置据点

    public void UploadSetplayerstronghold(int HoldId, string nickName, string locationName, double positionX, double positionY, System.Action<BusinessStrongholdAttribute> callback)
    {
        var _wForm = new WWWForm();
        _wForm.AddField("token", AndaDataManager.Instance.mainData.token);
        _wForm.AddField("HoldId", HoldId);
        _wForm.AddField("NickName", nickName);
        _wForm.AddField("LocationName", locationName);
        Debug.Log("positionX" + positionX);
        Debug.Log("positionY" + positionY);
        //经纬度 顺序  这里跟服务上的相反
        _wForm.AddField("PositionX", positionY.ToString());
        _wForm.AddField("PositionY", positionX.ToString());

        //--
        string path = networkAdress2 + "StrongHold/AddBusinessStrongHold";
        StartCoroutine(ExcuteUploadPlayerstornghold(path, _wForm, callback));
    }

    private IEnumerator ExcuteUploadPlayerstornghold(string _url, WWWForm _wForm, System.Action<BusinessStrongholdAttribute> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        Debug.Log("uploading");
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        AndaUIManager.Instance.OpenWaitBoard(false);
        Debug.Log("finish");
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            BusinessStrongHoldInfo data = JsonMapper.ToObject<BusinessStrongHoldInfo>(postData.text);
            BusinessStrongholdAttribute bss = ConvertTool.ConvertToBussinessStrongholdAttribute(data.BusinessStrongHold);
            AndaDataManager.Instance.mainData.AddBussinessStronghold(bss);
            callback(bss);
        }
    }

    #endregion

    #region 上传编辑的玩家/商家信息
    public void CallServerUploadUserInformation(byte[] imgByte, string nickname, string description,System.Action<bool>  callback)
    {
       
         var _wForm = new WWWForm();
        _wForm.AddField("token", AndaDataManager.Instance.mainData.token);
       // string josn =  ConvertTool.bytesToString(imgByte); // LitJson.JsonMapper.ToJson(imgByte);
       // Debug.Log("josn" + josn);
        _wForm.AddField("headImage", ConvertTool.bytesToString(imgByte));
        _wForm.AddField("name", nickname);
        _wForm.AddField("autograph", description);
        string path = networkAdress2 + "UserInfo/EditInfo";

        StartCoroutine(ExcuteUploadUserInformation(path, _wForm,callback));
    }

    private IEnumerator ExcuteUploadUserInformation(string _url, WWWForm _wForm, System.Action<bool> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        Debug.Log("PostData" + postData.text);
        Result result = JsonMapper.ToObject<Result>(postData.text);
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            callback(result.code == "200");
        }
       
    }
    #endregion


    #region 上传编辑的奖励
    public void CallServerUploadReward(BussinessRewardStruct data ,List<SonCoupon> sonCoupons, System.Action<bool> callback)
    {

        var _wForm = new WWWForm();
        _wForm.AddField("token", AndaDataManager.Instance.mainData.token);

        _wForm.AddField("code", "");
        _wForm.AddField("image", data.image);
        _wForm.AddField("title", data.title);
        _wForm.AddField("description", data.description);
        _wForm.AddField("status", data.status);
        _wForm.AddField("starttime", data.starttime);
        _wForm.AddField("endtime", data.endtime);
        _wForm.AddField("continueTime", data.continueTime);
        _wForm.AddField("porIsUpdate", data.porIsUpdate);
        _wForm.AddField("createTime", data.createTime);
        _wForm.AddField("rewardDropRate", data.rewardDropRate);
        _wForm.AddField("rewardDropCount", data.rewardDropCount);
        string json = "";
        List<SonCoupon> sonCouponList = sonCoupons;
        if(sonCoupons != null)
        {
            json = JsonMapper.ToJson(sonCouponList);
        }
        _wForm.AddField("SonCoupon", json);
       
        string path = networkAdress2 + "BusinessCoupon/Add";
         StartCoroutine(ExcuteCallServerUploadReward(path, _wForm, callback));
    }

    private IEnumerator ExcuteCallServerUploadReward(string _url, WWWForm _wForm, System.Action<bool> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        Debug.Log("PostData" + postData.text);
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            BusinessCouponRequest result = JsonMapper.ToObject<BusinessCouponRequest>(postData.text);
            AndaDataManager.Instance.mainData.AddBussinessRewar(result.data);
            callback(result.code == "200");
        }

    }
    #endregion

    #region 往商家据点里放入奖励
    public void CallServerInsertStrongholdReward(int strongholdIndex, int rewardIndex, System.Action<bool,int> callback)
    {
        var _wForm = new WWWForm();
        _wForm.AddField("token", AndaDataManager.Instance.mainData.token);
        _wForm.AddField("strongholdIndex", strongholdIndex);
        _wForm.AddField("couponIndex", rewardIndex);
        string path = networkAdress2 + "BusinessCoupon/StrongholdAddCoupon";
        StartCoroutine(ExcuteCallServerInsertStrongholdReward(path , _wForm, strongholdIndex,rewardIndex, callback));
    }

    private IEnumerator ExcuteCallServerInsertStrongholdReward(string _url, WWWForm _wForm,int bsIndex, int rewardIndex, System.Action<bool,int> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        Debug.Log("PostData" + postData.text);

        AndaUIManager.Instance.OpenWaitBoard(false);
        if(postData.error!=null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            BusinessCouponRequest result = JsonMapper.ToObject<BusinessCouponRequest>(postData.text);
            //-- 更新本地数据
            //据点里的奖励数据更新
            AndaDataManager.Instance.mainData.UpdateAddStrongholdReward(bsIndex, rewardIndex);
            Debug.Log("Here Run Times");
            callback(result.code == "200", rewardIndex);
        }
      

    }
    #endregion

    #region 商家据点移除奖励
    public void CallServerRemoveStrongholdReward(int strongholdIndex, int rewardIndex, System.Action<bool,int> callback)
    {
        var _wForm = new WWWForm();
        _wForm.AddField("token", AndaDataManager.Instance.mainData.token);
        _wForm.AddField("strongholdIndex", strongholdIndex);
        _wForm.AddField("couponIndex", rewardIndex);
        string path = networkAdress2 + "BusinessCoupon/StrongholdDelCoupon";
        StartCoroutine(ExcuteCallServerRemoveStrongholdReward(path, _wForm, rewardIndex,strongholdIndex, callback));
    }

    private IEnumerator ExcuteCallServerRemoveStrongholdReward(string _url, WWWForm _wForm,int rewardIndex, int shIndex, System.Action<bool,int> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
        Debug.Log("PostData" + postData.text);
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            BusinessCouponRequest result = JsonMapper.ToObject<BusinessCouponRequest>(postData.text);
            AndaDataManager.Instance.mainData.UpdateReduceStrongholdReward(shIndex, rewardIndex);
            callback(result.code == "200" , rewardIndex);
        }


    }
    #endregion

    #region 据点改名字

    public void CallServerUploadStrongholdNickname(int shIndex,string nickName,System.Action<bool> callback)
    {
        var _wForm = new WWWForm();
        _wForm.AddField("token", AndaDataManager.Instance.mainData.token);
        _wForm.AddField("index", shIndex);
        _wForm.AddField("name", nickName);
        string path = networkAdress2 + "StrongHold/EditName";
        StartCoroutine(ExcuteCallServerUploadStrongholdNick(path, _wForm, shIndex, nickName, callback));
    }

    private IEnumerator ExcuteCallServerUploadStrongholdNick(string _url, WWWForm _wForm, int shIndex,string nickName , System.Action<bool> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW postData = new WWW(_url, _wForm);
        yield return postData;
      
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Result result = JsonMapper.ToObject<Result>(postData.text);
            AndaDataManager.Instance.mainData.UpdateStrongholdNickName(shIndex, nickName);
            callback(result.code == "200");
        }
    }
    #endregion

    #region 向服务器要头像数据
    public void CallServerGetImagePor(string address ,System.Action<Sprite> callback)
    {
        string st = networkAdress4 + address;
        StartCoroutine(ExcuteCallServerGetImagePor(st,callback));
    }

    private IEnumerator ExcuteCallServerGetImagePor(string adress,System.Action<Sprite> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW wWW = new WWW(adress);
        yield return wWW;
        AndaUIManager.Instance.OpenWaitBoard(false);
        if(string.IsNullOrEmpty(wWW.error))
        {
            byte[] b = wWW.texture.EncodeToPNG();
            string t = ConvertTool.bytesToString(b);
            PlayerPrefs.SetString("SH_" + adress, t);
            Texture2D texture2D = ConvertTool.ConvertToTexture2d(wWW.texture);
            Sprite sprite = ConvertTool.ConvertToSpriteWithTexture2d(texture2D);
            callback(sprite);
        }else
        {
            AndaUIManager.Instance.PlayTips(wWW.error);
        }

    }


    #endregion

    #region 向服务器索要奖励图片数据
    public void CallServerGetRewardImagePor(string address, System.Action<Sprite> callback)
    {
        string st = networkAdress4 + address;
        StartCoroutine(ExcuteCallServerGetImagePor(st, callback));
    }

    private IEnumerator ExcuteCallServerGetRewardImagePor(string adress, System.Action<Sprite> callback)
    {
        AndaUIManager.Instance.OpenWaitBoard(true);
        WWW wWW = new WWW(adress);
        yield return wWW;
        AndaUIManager.Instance.OpenWaitBoard(false);
        if (string.IsNullOrEmpty(wWW.error))
        {
            byte[] b = wWW.texture.EncodeToPNG();
            string t = ConvertTool.bytesToString(b);
            PlayerPrefs.SetString("RW_"+adress , t);
            Texture2D texture2D = ConvertTool.ConvertToTexture2d(wWW.texture);
            Sprite sprite = ConvertTool.ConvertToSpriteWithTexture2d(texture2D);
            callback(sprite);
        }
        else
        {
            AndaUIManager.Instance.PlayTips(wWW.error);
        }

    }


    #endregion
}
