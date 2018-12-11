using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;

public class MapCtrlData  {

    public List<BusinessStrongholdAttribute> getStrongholdAtrr {get { return locationDataMineStronghold; } }
   
    public List<PlayerStrongholdAttribute> getPlayerStrongholdAttrs{ get {return playerStrongholdList ;} }
    public List<BusinessStrongholdAttribute> getMineStrongholdAttrs{get {return mineStrongholdList ;}}
    public List<BusinessStrongholdAttribute> getOtherStrongholdAttrs { get { return otherStrongholdList; } }
    public List<GameObject> getPlayerStrongholdItems {get {return playerStrongholdItem ;}}
    public List<GameObject> getmineStrongholdItems { get { return mineStrongholdItem; } }
    public List<GameObject> getotherStrongholdItems { get { return otherStrongholdItem; } }
    public MapController getMapController {get {return mapController ;}}
    private MapController mapController;
    public MapView getMapView {get{return mapView ;}}
    private MapView mapView;


    private List<LD_Objs> mineConsumableList = new List<LD_Objs>();


    private List<GameObject> mineStrongholdItem = new List<GameObject>();
    private List<GameObject> otherStrongholdItem =new List<GameObject>();
    private List<GameObject> playerStrongholdItem = new List<GameObject>();
    private List<BusinessStrongholdAttribute> locationDataMineStronghold = new List<BusinessStrongholdAttribute>();
    private List<BusinessStrongholdAttribute> mineStrongholdList = new List<BusinessStrongholdAttribute>();
    private List<BusinessStrongholdAttribute> otherStrongholdList = new List<BusinessStrongholdAttribute>();
    private List<PlayerStrongholdAttribute> playerStrongholdList = new List<PlayerStrongholdAttribute>();

    public List<double> curMapCenterPose;

    public Vector2d curMapCenterV2d { get { return new Vector2d(curMapCenterPose[0],curMapCenterPose[1]);}}
    public Vector2d getMapCenter{get {return new Vector2d(curMapCenterV2d[1] , curMapCenterV2d[0]);}}
    public int lastSelectStrongholdIndex;

    public bool isEndBuildMap = false;

    public void BuildData(BaseController baseController)
    {
        mapController = baseController as MapController;
        AndaDataManager.Instance.mainData.UpdateStrongholdDataEvent += UpdateStrongholdEvent;
    }

    void MainData_UpdateStrongholdDataEvent(BusinessStrongholdAttribute obj)
    {
    }


    public void SetView(ViewBasic viewBasic)
    {
        mapView = viewBasic as MapView;
    }

    public void BuildUserStronghold()
    {
       
        locationDataMineStronghold = AndaDataManager.Instance.mainData.businessStrongholdAttributes;
        if (locationDataMineStronghold == null) return;
        int count = locationDataMineStronghold.Count;
        mapView.BuildMineStrongholdItem(locationDataMineStronghold, mapController.ClickSwitchStronghold);
        curMapCenterPose = locationDataMineStronghold[0].strongholdPosition;

    }

    public void AdditionBussinessStrongholdData(BusinessStrongholdAttribute businessStrongholdAttribute)
    {
        Vector2d v = new Vector2d(businessStrongholdAttribute.strongholdPosition[0], businessStrongholdAttribute.strongholdPosition[1]);
        businessStrongholdAttribute.strongholdInMapPosition  = mapController.map.GeoToWorldPosition(v);
        mineStrongholdList.Add(businessStrongholdAttribute);
    }

    public void SetLocationStrongholdData(List<PlayerStrongholdAttribute> pInfos, List<BusinessStrongholdAttribute> binfos)
    {
        if(mineStrongholdList.Count!=0)mineStrongholdList.Clear();
        if(otherStrongholdList.Count!=0)otherStrongholdList.Clear();
        if(playerStrongholdList.Count!=0)playerStrongholdList.Clear();

        int count = binfos.Count;
        for(int i = 0 ; i < count; i ++)
        {
            Vector2d v = new Vector2d(binfos[i].strongholdPosition[0], binfos[i].strongholdPosition[1]);
            if (binfos[i].hostIndex == AndaDataManager.Instance.mainData.playerData.userIndex)
            {
                binfos[i].strongholdInMapPosition = mapController.map.GeoToWorldPosition(v);
                mineStrongholdList.Add(binfos[i]);

            }else
            {
                binfos[i].strongholdInMapPosition = mapController.map.GeoToWorldPosition(v);
                otherStrongholdList.Add(binfos[i]);
            }
        }

        Debug.Log("MineCount" + mineStrongholdList.Count);

        Debug.Log("OtherCount" + otherStrongholdList.Count);
       

        count = pInfos.Count;
        for(int i = 0 ; i < count; i++)
        {
           
            Vector2d v = new Vector2d(pInfos[i].strongholdPosition[0], pInfos[i].strongholdPosition[1]);
            //Debug.Log("v" + v);
            pInfos[i].strongholdInMapPosition = mapController.map.GeoToWorldPosition(v);
            playerStrongholdList.Add(pInfos[i]);
        }
        Debug.Log("PlayerCount" + playerStrongholdList.Count);
    }

    #region 构建据点物件

    public void BuildStrongholdItem()
    {
        //我方商家据点
        if(mineStrongholdItem.Count!=0)
        {
            int itemCount = mineStrongholdItem.Count;
            for(int i = 0 ; i < itemCount; i++)
            {
                GameObject.Destroy(mineStrongholdItem[i]);
            }
            mineStrongholdItem.Clear();
        }
        int count = mineStrongholdList.Count;

        Debug.Log("MineBussCount" + count);
        for (int i = 0 ; i < count;i++)
        {
            GameObject item = Resources.Load<GameObject>("Prefab/mapItem_BussinessStrongholdItem"); //Resources.Load<GameObject>("Prefab/"+ (30000+ mineStrongholdList[i].strongholdLevel));
            item = GameObject.Instantiate(item);
            item.transform.SetParent(mapView.transform);
            item.transform.localScale = Vector3.one;
            BusinessStrongholdAttribute bs = mineStrongholdList[i];
            Sprite levelBoard = AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(bs.strongholdLevel);
            item.GetComponent<ItemInfo_Img_level_name>().SetInfo(bs.strongholdIndex,AndaDataManager.Instance.mainData.imgPor,levelBoard, bs.strongholdNickName);
            item.GetComponent<ItemInfo_Img_level_name>().callback = OpenStrongholdAddtionBar;
            mineStrongholdItem.Add(item);
            item.name = bs.strongholdNickName;
        }


        //其他商家据点
        if(otherStrongholdItem.Count!=0)
        {
            int itemCount = otherStrongholdItem.Count;
            for (int i = 0; i < itemCount; i++)
            {
                GameObject.Destroy(otherStrongholdItem[i]);
            }
            otherStrongholdItem.Clear();
        }

        count = otherStrongholdList.Count;
        Debug.Log("OtherBussCount" + count);
        for (int i = 0; i < count; i++)
        {
            GameObject item = Resources.Load<GameObject>("Prefab/mapItem_BussinessStrongholdItem");
            item = GameObject.Instantiate(item);
            item.transform.SetParent(mapView.transform);
            item.transform.localScale = Vector3.one;

            /*BusinessStrongholdAttribute bs = mineStrongholdList[i];
            Sprite levelBoard = AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(bs.strongholdLevel);
            item.GetComponent<ItemInfo_Img_level_name>().SetInfo(bs.strongholdIndex, AndaDataManager.Instance.mainData.imgPor, levelBoard, bs.strongholdNickName);
*/
            otherStrongholdItem.Add(item);
        }

        //其他玩家的据点

        if(playerStrongholdItem.Count!=0)
        { 
            int itemCount = playerStrongholdItem.Count;
            for(int i = 0 ; i < itemCount; i++)
            {
                GameObject.Destroy(playerStrongholdItem[i]);
            }
            playerStrongholdItem.Clear();
        }

        count = playerStrongholdList.Count;
        Debug.Log("PlayerCount" + count);
        for (int i = 0 ; i < count; i++)
        {
            GameObject item = Resources.Load<GameObject>("Prefab/mapItem_PlayerStrongholdItem");// + (30000 + playerStrongholdList[i].strongholdLevel));
            item = GameObject.Instantiate(item);
            item.transform.parent = mapView.transform;
            item.transform.localScale = Vector3.one;
            playerStrongholdItem.Add(item);
        }
    }

    #endregion

    #region 打开据点添加奖励面板
    public void OpenStrongholdAddtionBar(int _index)
    {
        if (AndaDataManager.Instance.mainData.bussinessReward.Count <= 0)
        {
            AndaUIManager.Instance.PlayTips("请先去编辑奖励");
            return;
        }
        BusinessStrongholdAttribute businessStrongholdAttribute = AndaDataManager.Instance.mainData.GetAtrongholdAttributes(_index);
        mapController.strongholdAddRewardBar.gameObject.SetActive(true);
        mapController.strongholdAddRewardBar.SetStrongholdInfo(businessStrongholdAttribute);
    }
    #endregion
    public void AdditionBuildMineStrongholdItem()
    {
        GameObject item = Resources.Load<GameObject>("Prefab/mapItem_BussinessStrongholdItem");// + (30000 + playerStrongholdList[i].strongholdLevel));
        item = GameObject.Instantiate(item);
        item.transform.parent = mapView.transform;
        BusinessStrongholdAttribute bs = mineStrongholdList[mineStrongholdList.Count-1];
        Sprite levelBoard = AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(bs.strongholdLevel);
        item.GetComponent<ItemInfo_Img_level_name>().SetInfo(bs.strongholdIndex, AndaDataManager.Instance.mainData.imgPor, levelBoard, bs.strongholdNickName);
        item.GetComponent<ItemInfo_Img_level_name>().callback = OpenStrongholdAddtionBar;
        item.transform.localScale = Vector3.one;
        item.name = bs.strongholdNickName;
        mineStrongholdItem.Add(item);
    }

    #region 构建用于 新建造据点的物件

    public void BuildNewMineStronghold(LD_Objs lD_Objs)
    {
        if(mapView!=null)
        {
            mapView.BuildNewSHItem(lD_Objs);
        }
    }

    #endregion

    public void BuildConsmuable()
    {
        mineConsumableList = AndaDataManager.Instance.mainData.GetMineStrongholdDrawing();

        mapView.BuildMineStrongholdDrawingItem(mineConsumableList, mapController.ClickSelectStrongholdDrawing);
    }

    public void SetCurrentMapCenterpose(List<double> setCurrentPose)
    {
        curMapCenterPose = setCurrentPose;
    }

    public void UpdateStrongholdEvent(BusinessStrongholdAttribute bs)
    {
        int count =  mineStrongholdItem.Count;
        for(int i = 0 ; i < count; i ++)
        {
            if(mineStrongholdItem[i].GetComponent<ItemInfo_Img_level_name>().index == bs.strongholdIndex)
            {
                Sprite levelBoard = AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(bs.strongholdLevel);

                mineStrongholdItem[i].GetComponent<ItemInfo_Img_level_name>().SetInfo(bs.strongholdIndex, AndaDataManager.Instance.mainData.imgPor, levelBoard, bs.strongholdNickName);

                return;
            }
        }
    }

}
