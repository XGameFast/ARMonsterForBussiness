using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftEditionBar : ViewBasic {

    public Image rawardPor;
    public InputField rewardName;
    public InputField rewardDescription;
    public InputField dropCountInput;
    public InputField dropRateText;
    public InputField dropPoolMaxCountInput;

    public Slider rewardDropRateSlider;


    public List<SonCoupon> composeRewads;//由其他构成的奖励
    private List<GameObject> rewardChildItems;

    public Toggle needAnotherRewardcompose;
    public GameObject editorAnotherRewardComposeBar;
    public GameObject editorDropRateComposeBar;

    public Transform composeRewardGrid;
    public GameObject composeItem;

    public Transform point2;
    public Transform point1;

    private string imgData;
    public override void StartView()
    {
        base.StartView();
        gameObject.SetActive(true);
        FadeIn();
    }

    public void OnDisable()
    {
        if(composeRewads!=null)
        {
            composeRewads.Clear();
        }

        if(rewardChildItems!=null)
        {
            int count = composeRewads.Count;
            for(int i = 0 ; i < count;  i++)
            {
                Destroy(rewardChildItems[i]);
            }
        }
    }


    public void ClickOpenAnotherRewardComposeBar(bool isOpen)
    {
        Debug.Log("isOpen" +isOpen);
        if (needAnotherRewardcompose.isOn)
        {
            List<BussinessRewardStruct> datas = AndaDataManager.Instance.mainData.bussinessReward;
            if (datas.Count <= 0)
            {
                AndaUIManager.Instance.PlayTips("您还有没其他奖励券进行编辑，请先完成本张奖励制作");
                needAnotherRewardcompose.isOn = false;
                return;
            }

            BuildConposeItem(datas);
            //needAnotherRewardcompose.isOn = true;
            editorAnotherRewardComposeBar.gameObject.SetActive(true);
           // editorDropRateComposeBar.transform.position = point2.transform.position;
        }
        else
        {
           
            editorAnotherRewardComposeBar.gameObject.SetActive(false);
          //  editorDropRateComposeBar.transform.position = point1.transform.position; 
        }
    }

    public void BuildConposeItem(List<BussinessRewardStruct> _datas)
    {
        int count = _datas.Count;
        for(int i = 0 ; i < count; i++)
        {
            if(rewardChildItems == null) rewardChildItems = new List<GameObject>();
             GameObject item = Instantiate(composeItem);
            item.transform.parent = composeRewardGrid;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<GiftItem_ComposeInfo>().SetInfo(this, _datas[i]);
            rewardChildItems.Add(item);
        }
    }

    public void AddComposeItem(SonCoupon item)
    {
        if(composeRewads == null) composeRewads = new List<SonCoupon>();
        composeRewads.Add(item);
    }

    public void ReduceComposeItem(SonCoupon item)
    {
        if(composeRewads != null)
        {
            composeRewads.Remove(item);
        }
    }

    public void GetRewardPorItem()
    {
       AndaDataManager.Instance.ReadTexture2DFromPC(FinishGetTexture);
    }

    private void FinishGetTexture(Texture2D texture2D)
    {
        Texture2D t2d = ConvertTool.ConvertToTexture2d(texture2D);
        rawardPor.sprite = ConvertTool.ConvertToSpriteWithTexture2d(t2d);
        byte[] bts = t2d.EncodeToPNG();
        imgData = ConvertTool.bytesToString(bts);
    }

    public void ChangeDropRateSlide()
    {
        dropRateText.text = (int)(rewardDropRateSlider.value * 100) + "%";
    }

    public void ChangeDropRateText()
    {
        int t = int.Parse(dropRateText.text);
        float v = t/100f;
        rewardDropRateSlider.value = v;
    }


    public void ClickSave()
    {
        if(rewardName.text == "")
        {
            AndaUIManager.Instance.PlayTips("奖励名字不能为空");
            return ;
        }
        if(rewardDescription.text == "")
        {
            AndaUIManager.Instance.PlayTips("奖励描述不能为空");
            return;
        }
        if(imgData == null)
        {
            AndaUIManager.Instance.PlayTips("奖励图标不能为空");
            return;
        }
        if(dropPoolMaxCountInput.text == "")
        {
            AndaUIManager.Instance.PlayTips("请输入该奖励在奖励池中的最大数量");
            return;
        }
       
        if(dropCountInput.text == "")
        {
            AndaUIManager.Instance.PlayTips("请输入一次最大掉落数量，不得超过奖励池最大数量");
        }
        int dropCount = int.Parse(dropCountInput.text);
        int maxCount = int.Parse(dropPoolMaxCountInput.text);
        if(dropCount>maxCount)
        {
            AndaUIManager.Instance.PlayTips("请确保一次掉落的数量不超过最大数量");
            return;
        }

        BussinessRewardStruct bussinessRewardStruct = new BussinessRewardStruct();
        bussinessRewardStruct.image = imgData;
        bussinessRewardStruct.title = rewardName.text;
        bussinessRewardStruct.description = rewardDescription.text; 
        bussinessRewardStruct.rewardDropRate = (int)(rewardDropRateSlider.value*100); 
        bussinessRewardStruct.rewardDropCount = int.Parse(dropCountInput.text);
        List<SonCoupon> tmpSonCounpon =null;
        if(needAnotherRewardcompose.isOn)
        {
            tmpSonCounpon = composeRewads;
        }
       //bussinessRewardStruct.maxCount = int.Parse(dropPoolMaxCount.text);

        AndaDataManager.Instance.networkController.CallServerUploadReward(bussinessRewardStruct, tmpSonCounpon   , CallBackUplaod);

    }

    public void ClickCancel()
    {
        gameObject.SetActive(false);
    }


    private void CallBackUplaod(bool issuccess)
    {
        if(issuccess)
        {
            AndaUIManager.Instance.PlayTips("Upload Success!");
            gameObject.SetActive(false);
        }else
        {
            AndaUIManager.Instance.PlayTips("Upload Faild");
        }
    }
}


