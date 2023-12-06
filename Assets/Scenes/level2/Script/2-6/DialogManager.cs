using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public Transform[] TargetDoll;
    public GameObject dialogFrame;
    public GameObject dialog_obj;
    public Dialog dialog;
    private DialogAsset dialogAsset;
    private Transform playerTransform; 
    private GameObject Target;
    private ChooseDialogAsset choose_asset;
    private float DisMin=200;
    private void Start()
    {
        playerTransform = GameObject.Find("Character").transform;
    }
    private GameObject SearchTarget()
    {
        DisMin = 200;
        Target = null;
        Vector3 playerForward = playerTransform.forward.normalized;
        foreach (Transform target in TargetDoll)
        {
            Vector3 toTarget = (target.position - playerTransform.position).normalized;
            float dotProduct = Vector3.Dot(playerForward, toTarget);
            Debug.Log(dotProduct);
            if (Mathf.Abs(dotProduct) <0.5f)
            {
                float Dis = Vector3.Distance(playerTransform.position, target.position);
                if (Dis < DisMin)
                {
                    DisMin = Dis;
                    Target = target.gameObject;
                }
            }
        }
        return Target;
    }
    public IEnumerator SetDialog()
    {

        GameObject target = SearchTarget();
        yield return null;
        if (target != null)
        {
            dialogAsset = target.GetComponent<DialogAsset>();
            dialog_obj.SetActive(true);
            dialogFrame.SetActive(true);
            dialog = dialog_obj.GetComponent<Dialog>();
            dialog.target_material = dialogAsset.material;
            dialog.Alpha = dialogAsset.Alpha;
            dialog.DistortSpeed = dialogAsset.DistortSpeed;
            dialog.lines = dialogAsset.lines;
            dialog.ShaderGlow = dialogAsset.ShaderGlow;
            dialog.color = dialogAsset.color;
            dialog.Choose = dialogAsset.Choose;
            dialog.chooseDialog = dialogAsset.chooseDialog;
            StartCoroutine(dialog.startDialog()); 
        }
        else
        {
            Debug.Log("null");
        }
    }
    public void SetDialog2(DialogAsset dialogInfo)
    {
        dialogAsset = dialogInfo;
        dialog_obj.SetActive(true);
        dialogFrame.SetActive(true);
        dialog = dialog_obj.GetComponent<Dialog>();
        dialog.target_material = dialogAsset.material;
        dialog.Alpha = dialogAsset.Alpha;
        dialog.DistortSpeed = dialogAsset.DistortSpeed;
        dialog.lines = dialogAsset.lines;
        dialog.ShaderGlow = dialogAsset.ShaderGlow;
        dialog.color = dialogAsset.color;
        dialog.Choose = dialogAsset.Choose;
        dialog.chooseDialog = dialogAsset.chooseDialog;
        StartCoroutine(dialog.startDialog());
    }
}
