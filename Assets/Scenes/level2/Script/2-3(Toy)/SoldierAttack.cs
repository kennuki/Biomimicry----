using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class SoldierAttack : MonoBehaviour
{
    private CinemachineVirtualCamera Third_Camera;
    private Animator anim;
    private Transform player;
    private Collider cd;
    public Vector3 DeadAngle = new Vector3(0, 90, 0);
    private Dismember dismember;

    public event System.EventHandler<StatuEventArgs> speedchange;
    private void Start()
    {
        Third_Camera = GameObject.Find("CameraGroup").transform.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.Find("Character").transform;
        dismember = player.GetComponent<Dismember>();
        anim = GetComponent<Animator>();
        cd = GetComponent<Collider>();
    }

    private IEnumerator Attack()
    {
        Debug.Log("00");
        Character.ActionProhibit = true;
        Character.AllProhibit = true;
        CameraRotate.cameratotate = false;
        Stop();
        yield return null;
        anim.enabled = false;
        this.transform.LookAt(player);
        StartCoroutine(CharacterLookBoss(3));
        yield return new WaitForSeconds(0.5f);
        anim.enabled = true;
        anim.SetInteger("Attack", 1);
        yield return new WaitForSeconds(0.5f);
        Third_Camera.Priority = 11;
        int layerIndex = LayerMask.NameToLayer("Character");
        Camera.main.cullingMask |= 1 << layerIndex;
        yield return new WaitForSeconds(1.7f);
        dismember.dismember();
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraRotate.cameratotate = false;
        DeadPanel.ActivePanel = true;
        Third_Camera.Priority = 9;
        Camera.main.cullingMask &= ~(1 << layerIndex);
        Time.timeScale = 0;
    }


   
    private IEnumerator CharacterLookBoss(float time)
    {
        Vector3 AngleDifference = new Vector3(0, transform.eulerAngles.y - player.eulerAngles.y, 0);
        Quaternion TargetRotation = player.rotation * Quaternion.Euler(0, DeadAngle.y + AngleDifference.y, DeadAngle.z);
        //Cm1.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = 0;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            player.rotation = Quaternion.Lerp(player.rotation, TargetRotation, 0.5f);
            yield return null;
        }
        //target.rotation = TargetRotation;
    }
    private void Stop()
    {
        speedchange(this, new StatuEventArgs(0,false));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            if(anim.enabled == true)
            {
                StartCoroutine(Attack());
                Destroy(cd);
            }

        }
    }
}
