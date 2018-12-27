using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using System.Linq;
using UnityEngine.EventSystems;
using Mapbox.Unity.Utilities;
using Mapbox.Geocoding;

public class MapController : BaseController {
    private MapCtrlData mapCtrlData;
    public AbstractMap map;
    public GameObject mapCameraTran {get {return mapCamera.gameObject;} }

    public Camera mapCamera;
    public Camera UICamera;
    public float cameraRotateSpeed;

    public float cameraMoveSpeed;
    public StrongholdAddRewardBar strongholdAddRewardBar;
    public override void StartCtrl()
    {
        base.StartCtrl();
        BuildData();
        SetView();
        OpenView();

        mapCtrlData.BuildUserStronghold();
        mapCtrlData.BuildConsmuable();
       

        if(AndaDataManager.Instance.mainData.businessStrongholdAttributes ==null)
        {
            Vector2d v2d = Conversions.StringToLatLon(map.Options.locationOptions.latitudeLongitude);
            mapCtrlData.SetCurrentMapCenterpose(new List<double>{ v2d[0], v2d[1]});
        }
        map.Initialize(mapCtrlData.curMapCenterV2d, 15);

        List<double>tt = new List<double> { mapCtrlData.curMapCenterPose [0] , mapCtrlData.curMapCenterPose [1]};
        //X = 121,  Y  = 28
        CallServerGetLocationStronghold(tt);

        mapCtrlData.getMapView.CallbackSearchLatLon = SeartPOI;

      //  mapCtrlData.getMapView.forwardGeocodeUser.OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
    }

    public override void EndCtrl()
    {
       // mapCtrlData.getMapView.forwardGeocodeUser.OnGeocoderResponse -= ForwardGeocoder_OnGeocoderResponse;
        OpenView(false);
        base.EndCtrl();
    }

    private void BuildData()
    {
        if(mapCtrlData == null)
        {
            mapCtrlData =new MapCtrlData();
            mapCtrlData.BuildData(this);
        }
    }

    #region 打开界面
    private void SetView()
    {
        mapCtrlData.SetView(mainContoller.mapView);
    }
    #endregion 





    #region 打开界面
    private void OpenView(bool isOpen = true)
    {
        if (isOpen)
        {
            mapCtrlData.getMapView.StartView();
        }
        else
        {
            mapCtrlData.getMapView.EndView();
        }
    }
    #endregion

    #region 获取周边的数据

    private void CallServerGetLocationStronghold(List<double> pose)
    {
        mapCtrlData.isEndBuildMap = false;
        AndaDataManager.Instance.GetLocationRangeInfo(pose, FinishGetLocationData);
    }

    private void SeartPOI(List<double> pose)
    {
        mapCtrlData.curMapCenterPose = pose;
        map.UpdateMap(mapCtrlData.curMapCenterV2d);
        List<double> tt = new List<double> { mapCtrlData.curMapCenterPose[0], mapCtrlData.curMapCenterPose[1] };

        /*GameObject item = AndaDataManager.Instance.GetItemInfoPrefab("ItemInfo_NewSHItem");
        item = Instantiate(item);
        item.transform.parent = mapCtrlData.getMapView.transform;
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;
        Vector3 vector3 = map.GeoToWorldPosition(new Vector2d(pose[0], pose[1]));
        Vector2 vector2 = mapCamera.WorldToScreenPoint(vector3);
        Vector3 p = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 90));
        item.transform.position = p;*/


        CallServerGetLocationStronghold(tt);
    }

    #endregion

    #region 成功获取了周围的数据

    private void FinishGetLocationData(List<PlayerStrongholdAttribute> pInfos, List<BusinessStrongholdAttribute> binfos)
    {

        mapCtrlData.SetLocationStrongholdData(pInfos , binfos);

        mapCtrlData.BuildStrongholdItem();

        Invoke("InitCameraPose", 0.5f);
        //----- 

    }

    #endregion

    #region 构建地图



    #endregion

    #region 点击切换据点

    public void ClickSwitchStronghold(int strongholdIndex)
    {
        if(mapCtrlData.lastSelectStrongholdIndex == strongholdIndex)return;

        mapCtrlData.lastSelectStrongholdIndex = strongholdIndex;
        BusinessStrongholdAttribute businessStrongholdAttribute = mapCtrlData.getStrongholdAtrr.FirstOrDefault(s=>s.strongholdIndex == strongholdIndex);
        mapCtrlData.curMapCenterPose = businessStrongholdAttribute.strongholdPosition;
        map.UpdateMap(mapCtrlData.curMapCenterV2d);
        List<double> tt = new List<double> { mapCtrlData.curMapCenterPose[0], mapCtrlData.curMapCenterPose[1] };
        CallServerGetLocationStronghold(tt);
       
       // InitCameraPose();
    }

    #endregion

    #region 选择据点图纸
    public void ClickSelectStrongholdDrawing(int strongholdDrawingIndex)
    {
       //先判断一下图纸够不够
        LD_Objs lD_Objs = AndaDataManager.Instance.mainData.strongholdDrawingList.FirstOrDefault(s => s.objIndex == strongholdDrawingIndex);
        if (lD_Objs.lessCount>=1)
        {
            mapCtrlData.BuildNewMineStronghold(lD_Objs);
        }else
        {
            AndaUIManager.Instance.PlayTips("该图纸数量不够");
        }
    }
    #endregion


    #region 选择购买据点图纸
    public void ClickSelectBuyStrongholdDrawing()
    {

    }
    #endregion

    #region 更新相机位置，每次切换据点或者重新定位，相机都会对焦到相应位置

    private void InitCameraPose()
    {

        Vector3 centerPose = map.GeoToWorldPosition(mapCtrlData.curMapCenterV2d);
        Quaternion quaternion = Quaternion.LookRotation(centerPose - new Vector3(centerPose.x, centerPose.y, centerPose.z - 60));// new Vector3(mapCamera.transform.position.x,0,mapCamera.transform.position.z));
        Vector3 euler = mapCameraTran.transform.eulerAngles;
        euler.y = quaternion.eulerAngles.y;
        mapCameraTran.transform.eulerAngles = euler;
        Vector3 cFwd = mapCameraTran.transform.forward;
        cFwd.y = 0;
        Vector3 pose = centerPose + (centerPose - cFwd).normalized * 60;
        pose.y = mapCameraTran.transform.position.y;
        mapCameraTran.transform.position = pose;

        mapCtrlData.isEndBuildMap =true;

        UpdateUIPose();
    }

    #endregion

    #region 向服务上传据点数据

    public void CallServerInsertStronghold(string nickName, string localName)
    {
        canMove = false;
        Vector3 pose = AndaDataManager.Instance.RayCastHit(mapCamera);
        if(pose.Equals(Vector3.zero))return;
        Vector2d v2d = map.WorldToGeoPosition(pose);
        Debug.Log("v2d" + v2d);
        AndaDataManager.Instance.CallServerInsertStronghold(nickName,localName,v2d.x,v2d.y, CallBackFinishSetStronghold);
    }

    #endregion

    private void CallBackFinishSetStronghold(BusinessStrongholdAttribute businessStrongholdAttribute)
    {
        mapCtrlData.AdditionBussinessStrongholdData(businessStrongholdAttribute);
        mapCtrlData.AdditionBuildMineStrongholdItem();
        mapCtrlData.getMapView.CancelAddNewItem();//把临时的那个物件 消掉
        UpdateUIPose();
        canMove =true;
        //mapCtrlData.getMapView.strongholdInfoBar.SetSaveBtnDisable();//让保存按钮失效
    }

   
   

    private Vector3 startMousePose;
    private bool isUpdateMouseEuler;

   
    private bool isUpdateMousePose;

    private void RotateCamera()
    {


        if(Input.GetMouseButtonDown(1))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                isUpdateMouseEuler = true;
                startMousePose = Input.mousePosition;
            }
           
        }

        if(Input.GetMouseButton(1))
        {
            if(isUpdateMouseEuler)
            {
                Vector3 delta = Input.mousePosition - startMousePose;
                if(delta.x > 5)
                {
                    Vector3 euler = mapCameraTran.transform.eulerAngles;
                    euler.y -= Time.deltaTime * cameraRotateSpeed;
                    mapCameraTran.transform.eulerAngles = euler;
                }else if(delta.x < -5)
                {
                    Vector3 euler = mapCameraTran.transform.eulerAngles;
                    euler.y += Time.deltaTime * cameraRotateSpeed;
                    mapCameraTran.transform.eulerAngles = euler;
                }
                UpdateUIPose();
                startMousePose = Input.mousePosition;
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            isUpdateMouseEuler = false;
        }


    }

    private void MoveCamera()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isUpdateMousePose = true;
                startMousePose = Input.mousePosition;
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(isUpdateMousePose)
            {
                Vector3 delta = Input.mousePosition - startMousePose;//new Vector3(Screen.width/2 , Screen.height/2,0);

                Vector3 vector3 = mapCameraTran.transform.position;
                Vector3 fwd = new Vector3(mapCameraTran.transform.forward.normalized.x, 0, mapCameraTran.transform.forward.normalized.z);
                vector3 += fwd * Time.deltaTime * -delta.y * cameraMoveSpeed;
                vector3 += mapCameraTran.transform.right * Time.deltaTime * -delta.x * cameraMoveSpeed;
                mapCameraTran.transform.position = vector3; 
                startMousePose = Input.mousePosition;
                UpdateUIPose();
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            isUpdateMousePose = false;
        }
    }
    private bool canMove =false;
    private void UpdateUIPose()
    {
        //if(!canMove)return ;
        int count1 = mapCtrlData.getPlayerStrongholdAttrs.Count;

        Debug.Log("PlayerItemCount" + count1);

        int count2 = mapCtrlData.getMineStrongholdAttrs.Count;

        Debug.Log("MineItemCount" + count2);
        int count3 = mapCtrlData.getOtherStrongholdAttrs.Count;

        Debug.Log("OtherItemCount" + count3);

        for (int i = 0 ; i <count2 ; i++)
        {
            int t = i ;
            Vector2 vector2 = mapCamera.WorldToScreenPoint(mapCtrlData.getMineStrongholdAttrs[t].strongholdInMapPosition);
            Vector3 p = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 90));
            mapCtrlData.getmineStrongholdItems[t].transform.position = p;
        }

        for(int i = 0 ; i <count3;i++) 
        {
            int t = i ;
            Vector2 vector2 = mapCamera.WorldToScreenPoint(mapCtrlData.getOtherStrongholdAttrs[t].strongholdInMapPosition);
            Vector3 p = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 90));
            mapCtrlData.getotherStrongholdItems[t].transform.position = p;
        }

        for (int i = 0; i < count1; i++)
        {
            Vector2 vector2 = mapCamera.WorldToScreenPoint(mapCtrlData.getPlayerStrongholdAttrs[i].strongholdInMapPosition);
            Vector3 p = UICamera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, 90));
            mapCtrlData.getPlayerStrongholdItems[i].transform.position = p;
        }

    }



    private void Update()
    {
        if(mapCtrlData!=null)
        {
            if (mapCtrlData.isEndBuildMap)
            {
                RotateCamera();
                MoveCamera();
            }   
        }  
    }

}

