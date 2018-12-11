using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardListView : ViewBasic {

    public Transform grid;

    public List<GameObject> list;
    public void OnDisable()
    {
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(list[i]);
           
        }
    }

    public System.Action <BussinessRewardStruct> itemSelect;

    public void BuildRewardList(System.Action<BussinessRewardStruct> callback)
    {
        itemSelect = callback;
        List<BussinessRewardStruct> infos = AndaDataManager.Instance.mainData.bussinessReward;
        int count = infos.Count;
        for(int i = 0 ; i < infos.Count ; i++)
        {
            GameObject item = AndaDataManager.Instance.GetItemInfoPrefab("RewardItem");
            item = Instantiate(item);
            item.transform.parent = grid;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<GiftItem_SelectItem>().SetInfo(i,infos[i], ClickItem);
            list.Add(item);
        }
    }

    public void ClickItem(BussinessRewardStruct value)//itemIndex = 物件游标，不是数据库游标
    {
        if(itemSelect!=null)
        {
            gameObject.SetActive(false);
            itemSelect(value);
        }
    }
}
