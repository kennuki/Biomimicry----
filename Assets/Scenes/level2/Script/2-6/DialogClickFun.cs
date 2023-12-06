using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogClickFun : MonoBehaviour
{
    public DialogAsset dialogInfo;
    public Dialog dialog;
    public DialogManager manager;
    public void OnClick()
    {
        dialog.Exit();
        manager.SetDialog2(dialogInfo);
    }
}
