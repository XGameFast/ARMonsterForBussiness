using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : ViewBasic {

    public Transform  infoBoard;

    public ItemInfo_SpriteAndName strongholdIconItem;
    public Transform strongholdGrid;
    public Transform consumableGrid;
    public Transform shopGrid;
    public float shopPoseY;
    public float mineStrongholdDrawingPoseY;
    public float mineStrongholdPose; 


    public StrongholdInfoBar strongholdInfoBar;

    private List<ItemInfo_SpriteAndName> strongholdIconItems;
    private bool isMoveUp =false;
    private float curMoveupProgress;
    private float targetPose;
    private int curMoveType = 0;
    private GameObject newItem;
    private LD_Objs useStrongholdDrwaing;

    public GiftEditionBar giftEditionBar;

    public override void StartView()
    {
        base.StartView();
        strongholdInfoBar.RegisterCallback(PlayerFinishEditorInformation ,PlayerCancleEditorInformationWithAddtionSH);
        gameObject.SetActive(true);
        FadeIn();
        BuildShopItem();
    }

    public override void EndView()
    {
        FadeOut();
        base.EndView();
    }

    #region 玩家取消在这里建造据点

    public void CancelAddNewItem()
    {
        newItem.GetComponent<Animator>().Play("destroy");
        newItem.GetComponent<ItemInfo_MapNewShItem>().CloseChooseBtn();
        Invoke("InvokeCancelAddNewItem", 0.5f);
    }

    private void InvokeCancelAddNewItem()
    {
        Destroy(newItem);
    }

    #endregion

    #region 玩家同意在这里建造据点

    public void ComfimSetStrongholdHere()
    {
        newItem.GetComponent<Animator>().Play("down");
        newItem.GetComponent<ItemInfo_MapNewShItem>().CloseChooseBtn();
        Invoke("InvokExcuteSetStronghold", 0.51f);
    }
    private void InvokExcuteSetStronghold()
    {
        strongholdInfoBar.StartView();
        //利用建造图纸的等级，提供等级框
        Sprite lvBoard =AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(useStrongholdDrwaing.objSmallID);
        strongholdInfoBar.SetLevelBoard(lvBoard);
        AndaUIManager.Instance.PlayTips("请编辑信息");
    }

    #endregion

    #region 玩家取消了编辑据点

    private void PlayerCancleEditorInformationWithAddtionSH()
    {
        newItem.GetComponent<Animator>().Play("up");
        newItem.GetComponent<ItemInfo_MapNewShItem>().OpenChooseButton();
    }

    #endregion

    #region 玩家编辑完据点信心，上传

    private void PlayerFinishEditorInformation(string shName, byte[] infos)
    {
        AndaUIManager.Instance.PlayTips("正在上传");
        ((MapController)baseController).CallServerInsertStronghold(shName,"tt");
    }

    #endregion


    #region 构建新的据点
    public void BuildNewSHItem(LD_Objs lD_Objs)
    {

        if(newItem!=null)Destroy(newItem);
        useStrongholdDrwaing = lD_Objs;
        newItem = AndaDataManager.Instance.GetItemInfoPrefab("ItemInfo_NewSHItem");
        newItem = Instantiate(newItem);
        newItem.transform.parent = this.transform;
        newItem.transform.localScale = Vector3.one;
        newItem.transform.localPosition = Vector3.zero;
        //-通过图纸来判断该据点的等级
        Sprite lvBoard = AndaDataManager.Instance.GetBussinessStrongholdLevelSprite(lD_Objs.objSmallID);
        Sprite icon = AndaDataManager.Instance.GetIconSprite(100);
        newItem.GetComponent<ItemInfo_MapNewShItem>().SetInfo(lvBoard,icon,"", ComfimSetStrongholdHere, CancelAddNewItem);
        newItem.GetComponent<Animator>().Play("up");
    }

    #endregion

    public void BuildMineStrongholdItem(List<BusinessStrongholdAttribute> infos , System.Action<int> callback)
    {
        int count = infos.Count;
        for(int i = 0; i < count; i ++)
        {
            GameObject _item = Instantiate(strongholdIconItem.gameObject);
            ItemInfo_SpriteAndName itemInfo_Sprite = _item.GetComponent<ItemInfo_SpriteAndName>();
            Sprite sprite = Resources.Load<Sprite>("Sprite/Stronghold/stronghold0" + infos[i].strongholdLevel);
            itemInfo_Sprite.SetInfo(infos[i].strongholdIndex,sprite , infos[i].strongholdNickName);
            itemInfo_Sprite.callback = callback;
            _item.transform.SetParent(strongholdGrid.transform);
            _item.transform.localPosition = Vector3.zero;
            _item.transform.localScale = Vector3.one;
        }
    }

    public void BuildMineStrongholdDrawingItem(List<LD_Objs> infos , System.Action<int> callback)
    {
        int count = infos.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject _item = Instantiate(strongholdIconItem.gameObject);
            ItemInfo_SpriteAndName itemInfo_Sprite = _item.GetComponent<ItemInfo_SpriteAndName>();
            Sprite sprite = Resources.Load<Sprite>("Sprite/Consuamble/" + infos[i].objID);
            itemInfo_Sprite.SetInfo(infos[i].objIndex, sprite, infos[i].objName);
            itemInfo_Sprite.callback = callback;
            _item.transform.SetParent(consumableGrid.transform);
            _item.transform.localPosition = Vector3.zero;
            _item.transform.localScale = Vector3.one;
        }
    }

    private void BuildShopItem()
    {
        int count = 4;
        for (int i = 0; i < count; i++)
        {
            GameObject _item = Instantiate(strongholdIconItem.gameObject);
            ItemInfo_SpriteAndName itemInfo_Sprite = _item.GetComponent<ItemInfo_SpriteAndName>();
            Sprite sprite = Resources.Load<Sprite>("Sprite/Consuamble/" + (42010+i));
             itemInfo_Sprite.SetInfo((42010 + i),sprite, GetStrongholdDrawingName(42010+i));
            _item.transform.SetParent(shopGrid.transform);
            _item.transform.localPosition = Vector3.zero;
            _item.transform.localScale = Vector3.one;
        }
    }


    public void ClickMoveToMineStronghold()
    {
        if(curMoveType == 0)return;
        curMoveType = 0;
        curMoveupProgress = 0;
        //targetPose = mineStrongholdPose;
        targetPose = mineStrongholdPose;
        if (!isMoveUp)
        {
            isMoveUp = true;
            StartCoroutine(ExcuteBoardMoveUp());
        }
    }


    public void ClickMoveToMineStrongholdDrawing()
    {
        if(curMoveType == 1)return;
        curMoveType =1;
        curMoveupProgress = 0;
        targetPose = mineStrongholdDrawingPoseY;
        //targetPose = mineStrongholdDrawingPoseY - infoBoard.transform.localPosition.y;
        if (!isMoveUp)
        {
            isMoveUp = true;
            StartCoroutine(ExcuteBoardMoveUp());
        }
    }

    public void ClickMoveToShopStrongholdDraing()
    {
        if(curMoveType == 2)return;
        curMoveType =2;
        curMoveupProgress = 0;
        targetPose = shopPoseY;
        //targetPose = shopPoseY - infoBoard.transform.localPosition.y;
        if (!isMoveUp)
        {
            isMoveUp = true;
            StartCoroutine(ExcuteBoardMoveUp());    
        }
    }

   



    private IEnumerator ExcuteBoardMoveUp()
    {
       
        while(isMoveUp &&  curMoveupProgress < 1)
        { 
            Vector3 vector3 =  infoBoard.transform.localPosition;
            curMoveupProgress += Time.deltaTime;
            curMoveupProgress = Mathf.Clamp01(curMoveupProgress);
            infoBoard.transform.localPosition = Vector3.Lerp(infoBoard.transform.localPosition ,
                                                             new Vector3(infoBoard.transform.localPosition.x,targetPose, infoBoard.transform.localPosition.z), curMoveupProgress );
            yield return null;
        }

        isMoveUp =false;
      /* Vector3 t = infoBoard.transform.localPosition;
        t.y = targetPose;
        infoBoard.transform.localPosition = t;*/
    }


    public string GetStrongholdDrawingName(int id)
    {
        switch(id)
        {
            case 42010:
                return "萌新建造图纸";
               
            case 42011:
                return "初级建造图纸";
            case 42012:
                return "中级建造图纸";
            case 42013:
                return "高级建造图纸";
            default:
                return "萌新建造图纸";
        }
    }

    public void OpenRewardEditor()
    {
        giftEditionBar.gameObject.SetActive(true);
    }

    public void OpenRewardList()
    {

    }
}
