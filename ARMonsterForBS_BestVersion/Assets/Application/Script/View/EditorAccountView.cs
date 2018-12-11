using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EditorAccountView : ViewBasic {

    public Image porImg;

    public InputField nickName;
    public InputField companyDescription;
    private byte[] imgValue;

    public override void StartView()
    {
        base.StartView();
        gameObject.SetActive(true);
        FadeIn();
    }

    public void ClickOpenChooseImg()
    {
        AndaDataManager.Instance.ReadTexture2DFromPC(CallBackSelectTexture);
    }

    public void ClickUploadInformation()
    {
        AndaDataManager.Instance.CallServerEditorUserInformation(imgValue,nickName.text, companyDescription.text, FinishUpload);
    }

    private void CallBackSelectTexture(Texture2D value)
    {
        if (value.height > 128 || value.width > 128)
        {
            AndaUIManager.Instance.PlayTips("请保存选择的图像为128大小");
            Destroy(value);
            return;
        }
        Texture2D newT2d = ConvertTool.ConvertToTexture2d(value);
        porImg.sprite = ConvertTool.ConvertToSpriteWithTexture2d(newT2d);
        imgValue = newT2d.EncodeToPNG();
    }

    public void FinishUpload(bool isFinish)
    {
        if(isFinish)
        {
            AndaUIManager.Instance.PlayTips("uploadSuccess");
        }else
        {
            AndaUIManager.Instance.PlayTips("uploadFaild");
        }
    }
}
