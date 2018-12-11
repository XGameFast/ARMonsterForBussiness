using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckConfigView : ViewBasic {

    public override void StartView()
    {
        base.StartView();
        gameObject.SetActive(true);
        FadeIn();
    }

    public override void EndView()
    {
        base.EndView();
        FadeOut();
    }
}
