using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoginView : ViewBasic {
    public InputField accountInput;
    public override void StartView()
    {
        base.StartView();
        if(PlayerPrefs.GetString("PlayerAccount")!="")
        {
            accountInput.text = PlayerPrefs.GetString("PlayerAccount");
        }
        gameObject.SetActive(true);
        FadeIn();
    }

    public override void EndView()
    {
        FadeOut();
        base.EndView();
    }

    #region 点击登录

    public void ClickLogin()
    {
        ((LoginController)baseController).Login(accountInput.text);
    }

    #endregion
}
