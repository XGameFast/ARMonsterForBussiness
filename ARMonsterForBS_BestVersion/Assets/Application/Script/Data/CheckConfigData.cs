using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;
public class CheckConfigData  {

    #region 配置文件的名字
    public static string BaseConfigFileName = "ConfigBase";
    public static string LocalConfigFilesNameList = "LocalConfigFilesNameList";//本地已经拥有的文件名
    public static string TipsConfigFileName = "TipsContent.txt";
    public static string BussinesStrongholdConfigFileName = "BussinesStrongholdConfig.txt";
    public static string ChanllengeGameConfigFileName = "ChanllengeGameConfig.txt";
    public static string MallConfigFileName = "MallConfig.txt";
    public static string MedalConfigFileName = "MedalConfig.txt";
    public static string MonsterConfigFileName = "MonsterConfig.txt";
    public static string GameAssetIDTypeConfig = "GameAssetIDTypeConfig.txt";
    public static string NameConfigFileName = "NameConfig.txt";
    public static string ObjectsConfigFileName = "ObjectsConfig.txt";
    public static string ProtectGameConfigFileName = "ProtectGameConfig.txt";
    public static string SkillConfigFileName = "SkillConfig.txt";
    public static string StrongholdConfigFileName = "StrongholdConfig.txt";
    public static string StarConfigFileName = "StarConfig.txt";
    public static string SkillArchievementValueFileName = "SkillArchievementValue.txt";
    #endregion


    public ConfigBase getLocalConfigBase {get {return localConfigBase ;}}
     private CheckConfigController checkConfigController ;
  

    private ConfigBase localConfigBase;
    public void BuildData(BaseController baseController)
    {
        checkConfigController = baseController as CheckConfigController;
        BuildLocalConfigBase();
    }

    public void SetView(ViewBasic viewBasic)
    {
        //CheckConfigMenu = viewBasic as chec;
    }


    public void BuildLocalConfigBase()
    {
        string json = PlayerPrefs.GetString(BaseConfigFileName);
        if(!string.IsNullOrEmpty(json))
        {
            localConfigBase = JsonMapper.ToObject<ConfigBase>(json);
        }
    }
    public void SetLocalConfigBase()
    {
        localConfigBase = new ConfigBase();
    }


    //[更新配置文件的版本信息]
    public void UpdateConfigFilesVersionData(List<LocalConfigFile > localConfigFiles)
    {
        int count = localConfigFiles.Count;
        if(localConfigBase.files == null) //重置配置文件
        {
            localConfigBase.files = new List<ConfigFile>();
            for(int i = 0 ; i  < count; i++)
            {
                localConfigBase.files.Add(new ConfigFile { name = localConfigFiles[i].name , lastWriteTime = localConfigFiles[i].lastWriteTime});
                PlayerPrefs.SetString(localConfigFiles[i].name , localConfigFiles[i].config);
            }


        }else
        {
            for(int i = 0 ; i  < count; i++)
            {
                ConfigFile configFile = localConfigBase.files.FirstOrDefault(s=>s.name == localConfigFiles[i].name);
                if(configFile == null) // 本地没有这个配置文件，那么添加
                {
                    localConfigBase.files.Add(new ConfigFile { name = localConfigFiles[i].name , lastWriteTime = localConfigFiles[i].lastWriteTime});
                }else //有的话就更新
                {
                    configFile.lastWriteTime = localConfigFiles[i].lastWriteTime;
                }
           
                //[写入配置文件]
                PlayerPrefs.SetString(localConfigFiles[i].name , localConfigFiles[i].config);
            }

        }

        //[保存configbase]
        string json = JsonMapper.ToJson(localConfigBase);
        PlayerPrefs.SetString(BaseConfigFileName , json);

    }


}


public class ConfigBase
{
    public string version {get;set;} //0.8.7 .
    public int lastWriteTime { get; set; }
    public List<ConfigFile> files { get; set; }
}

public class ConfigFile
{
    public string name { get; set; }
    public int lastWriteTime { get; set; }
}

public class LocalConfigFile
{
    public string name {get;set;}
    public int lastWriteTime{get;set;}
    public string config{get;set;}
}
