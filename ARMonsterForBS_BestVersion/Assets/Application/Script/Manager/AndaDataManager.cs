using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using SFB;
using System.Linq;
public class AndaDataManager  {

    private static AndaDataManager _andaDataManager = null;

    public static AndaDataManager Instance
    {
        get 
        {
            if(_andaDataManager == null)
            {
                _andaDataManager = new AndaDataManager();

            }
            return _andaDataManager ;
     
        }
    }
    public MainContoller mainContoller;
    public NetworkController networkController;
     
    public MainData mainData ;


    #region 设置和更新数据
    public void SetUserData(BusinessData playerData , string token)
    {
        if(mainData == null)mainData = new MainData();
        mainData.SetUserInformation(playerData, token);
    }
    #endregion

    #region 从Resources读取物件

    public Sprite GetBussinessStrongholdLevelSprite(int level)
    {
        return Resources.Load<Sprite>("Sprite/LevelBoard/BussinessSH/" + level);
    }

    public Sprite GetIconSprite(int id)
    {
        return Resources.Load<Sprite>("Sprite/Icon/" + id);
    }

    public GameObject GetItemInfoPrefab(string itemName)
    {
        return Resources.Load<GameObject>("Prefab/"+ itemName);
    }

    #endregion

    #region 从本机读取文件


    public void ReadTexture2DFromPC(System.Action<Texture2D> callback)
    {
        mainContoller.StartCoroutine(LoadAssetOutSide(callback));
    }

    private IEnumerator LoadAssetOutSide(System.Action<Texture2D> callback)
    {
        string url = OpenFileWin();
        string path = url;
        WWW www = new WWW(path);
        yield return www;
        callback(www.texture);
        /*Texture2D t2d = new Texture2D(www.texture.height, www.texture.height);
        t2d.SetPixels(www.texture.GetPixels());
        t2d.Apply(true);
        t2d.filterMode = FilterMode.Trilinear;
        callback(www.texture);*/
        /*;

        tmText2d.material.mainTexture = texture;

        s.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                                 new Vector2(0.5f, 0.5f));*/

    }

    public string OpenFileWin()
    {
        //GameObject.FindWithTag("moviePlayer").GetComponent<VideoPlayer>().Pause(); OpenFileName ofn = new OpenFileName(); ofn.structSize = Marshal.SizeOf(ofn); ofn.filter = "All Files\0*.*\0\0"; ofn.file = new string(new char[256]); ofn.maxFile = ofn.file.Length; ofn.fileTitle = new string(new char[64]); ofn.maxFileTitle = ofn.fileTitle.Length; string path = Application.streamingAssetsPath; path = path.Replace('/', '\\'); //默认路径  ofn.initialDir = path; //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  ofn.title = "Open Project"; ofn.defExt = "JPG";//显示文件的类型  //注意 一下项目不一定要全选 但是0x00000008项不要缺少  ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  if (WindowDll.GetOpenFileName(ofn)) { Debug.Log("Selected file with full path: {0}" + ofn.file); } //此处更改了大部分答案的协程方法，在这里是采用unity的videoplayer.url方法播放视频； // /*而且我认为大部分的其他答案，所给的代码并不全，所以，想要其他功能的人，可以仿照下面的代码，直接在此类中写功能。
        string[] s = StandaloneFileBrowser.OpenFilePanel("选择图片作为头像，尺寸为128*128", "Desktop", "png", false);
        Debug.Log("string:" + s[0]);
        return s[0];
    }



    #endregion

    #region 获取奖励图像

    public void GetRewardImg(string path , System.Action<Sprite> action )
    {
        string s = PlayerPrefs.GetString("RW_" + path) ;
        if(s == "")
        {
            CallServerGetRewardImg(path , action);
        }
        else
        {
           
            byte[] vs = ConvertTool.StringToBytes(s);
            Texture2D texture =new Texture2D(128,128);
            texture.LoadImage(vs);
            texture = ConvertTool.ConvertToTexture2d(texture);
            Sprite sprite =ConvertTool.ConvertToSpriteWithTexture2d(texture);
            action(sprite);
        }
    }



    #endregion

    #region 获取用户头像

    public void GetUserImg(string path, System.Action<Sprite> action)
    {
        string s = PlayerPrefs.GetString("SH_" + path);
        if (s == "")
        {
            CallServerGetUserImg(path, action);
        }
        else
        {

            byte[] vs = ConvertTool.StringToBytes(s);
            Texture2D texture = new Texture2D(128, 128);
            texture.LoadImage(vs);
            texture = ConvertTool.ConvertToTexture2d(texture);
            Sprite sprite = ConvertTool.ConvertToSpriteWithTexture2d(texture);
            action(sprite);
        }
    }

    #endregion



    #region 与服务器通信
    #endregion
    #region 登录
    public void Login(System.Action<bool> callback, string name)
    {
        networkController.Login(callback, name);
    }
    #endregion

    #region 向服务器索要奖励的图像
    public void CallServerGetRewardImg(string path , System.Action<Sprite>action)
    {
        networkController.CallServerGetRewardImagePor(path,action);
    }
    #endregion
     
    #region 设置玩家信息

    public void CallServerEditorUserInformation(byte[] img,string nickname, string description, System.Action<bool> callback)
     
    {
        networkController.CallServerUploadUserInformation(img,nickname,description,callback);
    }

    #endregion

    #region 向服务器更新据点的昵称
    public void CallServerUploadStrongholdNickName(string nickname,int shIndex, System.Action<bool> callback)
    {
        networkController.CallServerUploadStrongholdNickname(shIndex,nickname,callback);
    }
    #endregion

    #region 向服务器索要据点头像
    public void CallServerGetUserImg(string path , System.Action<Sprite> callback)
    {
        networkController.CallServerGetImagePor(path,callback);
    }
    #endregion

    #region 放置据点
    public void CallServerInsertStronghold(string nickName, string locationName, double px,double py, System.Action<BusinessStrongholdAttribute >callback)
    {
        networkController.UploadSetplayerstronghold(mainData.playerData.userIndex,nickName,"",px,py,callback);
    }
    #endregion

    #region 向服务插入据点奖励 是否成功，和奖励Index
    public void CallServerInsertStrongholdReward(int shIndex,int rewardIndex, System.Action<bool,int>callback)
    {
        networkController.CallServerInsertStrongholdReward(shIndex,rewardIndex,callback);
    }
    #endregion
    public void CallServerDeleteStrongholdReward(int shIndex, int rewardIndex, System.Action<bool, int> callback)
    {
        networkController.CallServerRemoveStrongholdReward(shIndex,rewardIndex,callback);
    }
    #region 获取用于判断配置文件的 基础配置文件
    public void CallServerGetConfig(System.Action<ConfigBase> action)
    {
        networkController.GetBaseConfig(action);
    }
    #endregion

    #region 下载配置文件
    public void CallServerUpdataConfig(List<ConfigFile> action, System.Action<List<LocalConfigFile>> configFiles)
    {
        networkController.GetConfigFils(action, configFiles);
    }

    #endregion

    #region 获取地图周围的物件
    public void GetLocationRangeInfo(List<double> location , System.Action<List<PlayerStrongholdAttribute>, List<BusinessStrongholdAttribute>> callbackPlayerInfo)
    {
        networkController.GetCurrentLocationRangeOtherData(location, callbackPlayerInfo);
    }
    #endregion

    #region 读取配置文件

    public List<BussinessBossConfig> _bussinessBossConfigs = null;
    public List<BussinessBossConfig> bussinessBossConfigs
    {
        get
        {
            if(_bussinessBossConfigs == null)
            {
                _bussinessBossConfigs = new List<BussinessBossConfig>
                {
                    new BussinessBossConfig
                    {
                        shLevelType = 0 ,
                        bossList = new List<int>(){2000,2001}
                    },
                    new BussinessBossConfig
                    {
                        shLevelType = 1 ,
                        bossList = new List<int>(){2000,2001}
                    },
                    new BussinessBossConfig
                    {
                        shLevelType = 2 ,
                        bossList = new List<int>(){2000,2001}
                    },
                    new BussinessBossConfig
                    {
                        shLevelType = 3 ,
                        bossList = new List<int>(){2000,2001}
                    }
                };
            }
            return _bussinessBossConfigs;
        }
    
    }


    public List<GameAssetIDType> _gameAssetIDTypes = null;
    public List<GameAssetIDType> gameAssetIDTypes
    {

        get
        {
            if (_gameAssetIDTypes == null)
            {
                string json = PlayerPrefs.GetString(CheckConfigData.GameAssetIDTypeConfig);
                _gameAssetIDTypes = JsonMapper.ToObject<List<GameAssetIDType>>(json);
            }

            return _gameAssetIDTypes;
        }
    }

    private  List<CD_ObjAttr> _objectsList = null;
    public  List<CD_ObjAttr> objectsList
    {
        get
        {
            if (_objectsList == null)
            {

                string json = PlayerPrefs.GetString(CheckConfigData.ObjectsConfigFileName);
                _objectsList = JsonMapper.ToObject<List<CD_ObjAttr>>(json);

            }

            return _objectsList;
        }
    }

    private List<MonsterBaseConfig> _monsterBaseConfig =null;
    public List<MonsterBaseConfig> monsterBaseConfig 
    {
        get 
        {
            if(_monsterBaseConfig == null)
            {
                string json = PlayerPrefs.GetString(CheckConfigData.MonsterConfigFileName);
                _monsterBaseConfig = JsonMapper.ToObject<List<MonsterBaseConfig>>(json);
            }
            return _monsterBaseConfig;
        }
    }


    #endregion

    public MonsterBaseConfig GetMonsterBaseConfig(int monsterID)
    {
        return monsterBaseConfig.FirstOrDefault(s=>s.monsterID == monsterID);
    }

    public List<int> GetBossList(int shLevelType)
    {
        return bussinessBossConfigs[shLevelType].bossList;
    }

    public string GetIDTypeName(int objectID)
    {
        GameAssetIDType iDType = GetAssetIDType(objectID, gameAssetIDTypes);
        if (iDType == null)
        {
            return "";
        }
        else
        {
            return iDType.typeName;
        }
    }

    public int GetObjTypeID(int objectID)
    {
        GameAssetIDType iDType = GetAssetIDType(objectID, gameAssetIDTypes);
        if (iDType == null)
        {
            return -1;
        }
        else
        {
            return iDType.type;
        }
    }

    public int GetObjectGroupID(int id)
    {
        int count00 = gameAssetIDTypes.Count;
        for (int i = 0; i < count00; i++)
        {
            if (id >= gameAssetIDTypes[i].idRange[0] && id <= gameAssetIDTypes[i].idRange[1])
            {
                return gameAssetIDTypes[i].type;
            }
        }
        Debug.Log("理论上不会出现在这里的，一定是配置文件有问题");
        return -1;
    }

    private GameAssetIDType GetAssetIDType(int id, List<GameAssetIDType> _info)
    {
        int count00 = _info.Count;
        for (int i = 0; i < count00; i++)
        {
            GameAssetIDType gameAssetIDType = _info[i];

            if (id >= gameAssetIDType.idRange[0] && id <= gameAssetIDType.idRange[1])
            {
                if (gameAssetIDType.subdivides == null)
                {
                    return gameAssetIDType;
                }
                else if (gameAssetIDType.subdivides.Count == 0)
                    return gameAssetIDType;
                else
                {
                    return GetAssetIDType(id, gameAssetIDType.subdivides);
                }
            }
        }
        Debug.Log("理论上不会出现在这里的，一定是配置文件有问题");
        return null;

    }
    public RaycastHit raycastHit;
    public Vector3 RayCastHit(Camera targetCamera )
    {
        if (Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out raycastHit, 500))
        {
            return raycastHit.point;
        }
        return Vector3.zero;
    }
}

public class BussinessBossConfig 
{
    public int shLevelType{get;set;}
    public List<int> bossList{get;set;}
}

