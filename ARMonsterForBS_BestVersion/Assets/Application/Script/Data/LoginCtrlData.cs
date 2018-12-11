using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginCtrlData  {


    public LoginView getLoginView {get { return loginView ;}}
    private LoginView loginView;
    public LoginController getLoginController {get {return loginController ;}}
    private LoginController loginController;
    public void BuildData(BaseController baseController )
    {
        loginController = baseController as LoginController;
    }

    public void SetView(ViewBasic viewBasic)
    {
        loginView = viewBasic as LoginView;
    }

    public void InitData()
    {

    }


}
