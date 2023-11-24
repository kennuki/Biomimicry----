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
    public  GameObject DeadPanel;
    public Vector3 DeadAngle = new Vector3(0, 90, 0);

    public event System.EventHandler<StatuEventArgs> speedchange;
    private void Start()
    {
        Third_Camera = GameObject.Find("CameraGroup").transform.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.Find("Character").transform;
        anim = GetComponent<Animator>();
        cd = GetComponent<Collider>();
    }
    private IEnumerator Attack()
    {
        Character.ActionProhibit = true;
        Character.AllProhibit = true;
        CameraRotate.cameratotate = false;
        Stop();
        StartCoroutine(CharacterLookBoss(3));
        anim.enabled = false;
        Third_Camera.Priority = 11;
        int layerIndex = LayerMask.NameToLayer("Character");
        Camera.main.cullingMask |= 1 << layerIndex;
        yield return new WaitForSeconds(0.5f);
        anim.enabled = true;
        anim.SetInteger("Attack", 1);
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraRotate.cameratotate = false;
        DeadPanel.SetActive(true);
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
        speedchange(this, new StatuEventArgs(0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            StartCoroutine(Attack());
            cd.enabled = false;
        }
    }
}
