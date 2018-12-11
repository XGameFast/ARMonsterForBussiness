using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : BaseController {

    private LoginCtrlData  loginCtrlData ;
    public override void StartCtrl()
    {
        base.StartCtrl();
        BuildCtrlData();
        SetView();
        OpenView();
    }

    public override void EndCtrl()
    {
        OpenView(false);
        base.EndCtrl();
    }

    #region 构建控制器数据
    private void BuildCtrlData()
    {
        if(loginCtrlData == null)
        {
            loginCtrlData = new LoginCtrlData();
            loginCtrlData.BuildData(this);
        }
    }
    #endregion

    #region 打开界面
    private void SetView()
    {
        loginCtrlData.SetView(mainContoller.loginView);
    }
    #endregion 

    #region 打开界面
    private void OpenView(bool isOpen = true)
    {
        if(isOpen)
        {
            loginCtrlData.getLoginView.StartView();
        }
        else
        {
            loginCtrlData.getLoginView.EndView();
        }
       
    }
    #endregion


    #region 点击登录

    public void Login(string account)
    {
        if(account == "" )
        {
            //发出提示，输入账号不合格
            return ;
        }
        AndaDataManager.Instance.Login(LoginResult , account);
    }

    #endregion

    #region 登录回调
    private void LoginResult(bool isSuccess)
    {
         
        if(isSuccess)
        {
            PlayerPrefs.SetString("PlayerAccount",loginCtrlData.getLoginView.accountInput.text);
            mainContoller.SwitchCtrl("mapCtrl"); 
        }else
        {

        }
    }
    #endregion

}
