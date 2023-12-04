using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class ThirdView : MonoBehaviour
{
    public CinemachineVirtualCamera Third_Camera;
    private Transform player;
    public UnityEvent OnIntChange; 

    private int intValue;
    public int IntValue
    {
        get { return intValue; }
        set
        {
            if (intValue != value)
            {
                intValue = value;
                OnIntChange.Invoke();
            }
        }
    }

    private void Start()
    {
        player = GameObject.Find("Character").transform;

        OnIntChange.AddListener(HandleIntChange);

    }
    private void Update()
    {
        IntValue = Character.LookState;
        if(Character.LookState == 3&&!Character.push)
        {
            PlayerRotate();
        }
    }
    private void PlayerRotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 targetDirection = new Vector3(-horizontalInput, 0f, -verticalInput);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        if (targetDirection.magnitude > 0)
            player.rotation = Quaternion.Lerp(player.rotation, targetRotation, 0.5f);
    }
    private void CameraCullChange()
    {
        Third_Camera.Priority = 11;
        int layerIndex = LayerMask.NameToLayer("Character");
        Camera.main.cullingMask |= 1 << layerIndex;
    }
    private IEnumerator CameraCullRecover()
    {
        Third_Camera.Priority = 9;
        yield return new WaitForSeconds(2);
        int layerIndex = LayerMask.NameToLayer("Character");
        Camera.main.cullingMask &= ~(1 << layerIndex);
    }




    void HandleIntChange()
    {
        if (Character.LookState == 3)
        {
            CameraCullChange();
            PlayerRotate();
        }
        else if (Character.LookState == 1)
        {
            StartCoroutine(CameraCullRecover());
            
        }
    }
}
